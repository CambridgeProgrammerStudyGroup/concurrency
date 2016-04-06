using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace ByteContentsCounter
{
    class ProducerConsumerBatchCount : IByteContentsCounter
    {
        public IDictionary<byte, int> ComputeCounts(string filePath)
        {
            var results = new ConcurrentDictionary<byte, int>();

            using (var bytesToCount = new BlockingCollection<byte[]>())
            {
                var producer = new Producer(filePath, bytesToCount);
                var producerThread = new Thread(() => producer.ReadFileToCollection());
                var consumerThreads = Enumerable.Range(0, 3).Select(n => new Thread(() =>
                    {
                        var consumer = new Consumer(bytesToCount, results);
                        consumer.CountBytes();
                        consumer.MergeCounts();
                    }))
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
            private readonly IDictionary<byte,int> localCounts = new Dictionary<byte, int>(); 

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
                            int currentCount;
                            localCounts.TryGetValue(b, out currentCount);
                            localCounts[b] = currentCount + 1;
                        }
                    }
                }
            }

            public void MergeCounts()
            {
                foreach (KeyValuePair<byte, int> kvp in localCounts)
                {
                    _results.AddOrUpdate(kvp.Key, kvp.Value, ((existingKey, existingValue) => existingValue + kvp.Value));
                }
            }
        }
    }
}
