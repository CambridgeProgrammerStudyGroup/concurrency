using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ByteContentsCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            
            if (args.Length == 0)
            {
                Console.WriteLine("Please pass an argument of the full path to a large binary file.");
                Environment.Exit(1);
            }
            //var filePath = @"C:\git\motor.journey\.git\objects\pack\pack-30066a72be03446d6b9dbb90a2d4552841b0f841.pack";
            //var filePath = @"C:\Windows\Installer\11d9f5.msi";
            var filePath = args[0];

            var sequential = new Sequential();
            TimedFunction(() => sequential.ComputeCounts(filePath), "Sequential");

            var lockContention = new LockContentionThreadPool();
            TimedFunction(() => lockContention.ComputeCounts(filePath), "LockContentionThreadPool");

            var concurrentDictionaryThreadPool = new ConcurrentDictionaryThreadPool();
            TimedFunction(() => concurrentDictionaryThreadPool.ComputeCounts(filePath), "ConcurrentDictionaryThreadPool");

            var producerConsumer = new ProducerConsumer();
            TimedFunction(() => producerConsumer.ComputeCounts(filePath), "ProducerConsumer");

            var producerConsumerBatchCount = new ProducerConsumerBatchCount();
            TimedFunction(() => producerConsumerBatchCount.ComputeCounts(filePath), "ProducerConsumerBatchCount");
        }

        static void TimedFunction(Func<IDictionary<byte,int>> function, string name)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = function();
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds/10:00}";
            Console.WriteLine("{0} - time: {1} ", name, elapsedTime);
            Console.WriteLine("{0} - result (0): {1} ", name, result[0]);
            Console.WriteLine("{0} - result (42): {1} ", name, result[42]);
            Console.WriteLine("{0} - result (255): {1} ", name, result[255]);
        }
    }
}
