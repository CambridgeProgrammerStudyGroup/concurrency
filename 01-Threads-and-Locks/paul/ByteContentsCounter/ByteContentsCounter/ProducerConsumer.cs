using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace ByteContentsCounter
{
    class ProducerConsumer : IByteContentsCounter
    {
        public IDictionary<byte, int> ComputeCounts(string filePath)
        {
            var results = new ConcurrentDictionary<byte, int>();

            using (var bytesToCount = new BlockingCollection<byte[]>(100))
            {
                var producer = new Producer(filePath, bytesToCount);
                var producerThread = new Thread(() => producer.ReadFileToCollection());
                var consumerThreads = Enumerable.Range(0, 3).Select(n =>
                    new Thread(() => new Consumer(bytesToCount, results).CountBytes()))
                    .ToArray();

                producerThread.Start();
                foreach (var c in consumerThreads)
                    c.Start();

                producerThread.Join();
                foreach (var c in consumerThreads)
                    c.Join();
            }

            return results;
        }

        class Producer
        {
            private readonly string _filePath;
            private readonly BlockingCollection<byte[]> _bytesToCount;

            public Producer(string filePath, BlockingCollection<byte[]> bytesToCount)
            {
                _filePath = filePath;
                _bytesToCount = bytesToCount;
            }

            public void ReadFileToCollection()
            {
                using (Stream source = File.OpenRead(_filePath))
                {
                    byte[] buffer = new byte[1024 * 1024]; //1MB buffer
                    int bytesRead;
                    while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        _bytesToCount.Add(buffer.Take(bytesRead).ToArray());
                    }
                    _bytesToCount.CompleteAdding();
                }
            }
        }

        class Consumer
        {
            private readonly BlockingCollection<byte[]> _bytesToCount;
            private readonly ConcurrentDictionary<byte, int> _results;

            public Consumer(BlockingCollection<byte[]> bytesToCount, ConcurrentDictionary<byte, int> results)
            {
                _bytesToCount = bytesToCount;
                _results = results;
            }

            public void CountBytes()
            {
                while (!_bytesToCount.IsCompleted)
                {
                    byte[] localBytes;
                    while (_bytesToCount.TryTake(out localBytes))
                    {
                        foreach (var b in localBytes)
                        {
                            _results.AddOrUpdate(b, 1, ((existingKey, existingValue) => existingValue + 1));
                        }
                    }
                }
            }
        }
    }
}
