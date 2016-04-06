using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace ByteContentsCounter
{
    public class ConcurrentDictionaryThreadPool : IByteContentsCounter
    {
        public IDictionary<byte, int> ComputeCounts(string filePath)
        {
            var results = new ConcurrentDictionary<byte, int>();

            using (var countdownEvent = new CountdownEvent(1))
            {
                using (Stream source = File.OpenRead(filePath))
                {
                    // use the threadpool below to create many threads working on counting bytes each 1kb
                    byte[] buffer = new byte[1024];
                    int bytesRead;
                    while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        countdownEvent.AddCount();

                        var bytesToCount = buffer.Take(bytesRead).ToArray();
                        var counter = new ByteCounter(bytesToCount, countdownEvent, results);

                        ThreadPool.QueueUserWorkItem(counter.Count);
                    }
                    countdownEvent.Signal();
                }
                countdownEvent.Wait();
            }

            return results;
        }

        private class ByteCounter
        {
            private readonly byte[] _bytes;
            private readonly CountdownEvent _countdownEvent;
            private readonly ConcurrentDictionary<byte, int> _results;

            public ByteCounter(byte[] bytes, CountdownEvent countdownEvent, ConcurrentDictionary<byte, int> results)
            {
                _bytes = bytes;
                _countdownEvent = countdownEvent;
                _results = results;
            }

            public void Count(object context)
            {
                foreach (var b in _bytes)
                {
                    // cause deliberate contention by updating the same dictonary here on many threads
                    _results.AddOrUpdate(b, 1, ((existingKey, existingValue) => existingValue + 1));
                }
                _countdownEvent.Signal();
            }
        }
    }
}
