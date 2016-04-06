using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace ByteContentsCounter
{
    public class LockContentionThreadPool : IByteContentsCounter
    {
        public IDictionary<byte, int> ComputeCounts(string filePath)
        {
            var results = new Dictionary<byte, int>();

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
            private readonly IDictionary<byte, int> _results;
            private static readonly object LockObject = new object();

            public ByteCounter(byte[] bytes, CountdownEvent countdownEvent, IDictionary<byte, int> results)
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
                    lock (LockObject)
                    {
                        int currentCount;
                        _results.TryGetValue(b, out currentCount);
                        _results[b] = currentCount + 1;
                    }
                }
                _countdownEvent.Signal();
            }
        }
    }
}
