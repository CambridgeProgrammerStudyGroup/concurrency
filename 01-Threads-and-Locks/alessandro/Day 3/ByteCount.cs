using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace ByteCount
{
	static class ByteCount
	{
        // Count occurrences of byte values in a file
		static void Main(string[] args)
		{
			if (args.Length == 0) {
				Console.WriteLine ("Need a file name!");
				return;
			}

			var fileName = args [0];

			var funcs = new Dictionary<string, Func<ByteTable>> {
				{ "Stream", () => CountFromStream (fileName) },     // Process one byte at a time 
				{ "Memory", () => CountFromMemory(fileName) },      // Read all in memory and then process
				{ "Chunks", () => CountFromChunks(fileName) },      // Read and process one chunk (1MiB) at a time
				{ "Parallel", () => CountFromChunksParallel(fileName) }, // Read and process chunks (1MiB) in parallel (PLINQ)

				{ "PC", () => CountProducerConsumer (fileName) },       // Producer (read from disk) and 1 Consumer (count bytes) 
				{ "P2C", () => CountProducerNConsumers (fileName, 2) }, // ... 2 Consumers
				{ "P3C", () => CountProducerNConsumers (fileName, 3) }, // ... 3 Consumers
				{ "P8C", () => CountProducerNConsumers (fileName, 8) }, // ... 8 Consumers
				{ "P16C", () => CountProducerNConsumers (fileName, 16) } // ... 16 Consumers
			};

			ByteTable baseline = null;
			foreach (var tuple in funcs) {
				var name = tuple.Key;
				var func = tuple.Value;

				Console.Write("{0}...", name);
				var watch = new Stopwatch();
				watch.Start();
				var result = func();
				Console.Write("\tTime: {0}ms", watch.ElapsedMilliseconds);

				if (baseline == null) {
					baseline = result;
					Console.WriteLine ("\t[Baseline]");
				} else {
					bool ok = baseline.Equals (result);
					Console.WriteLine ("\t[{0}]", ok ? "OK" : "Failed");
				}
			}
		}

		class ByteTable
		{
			private readonly long[] table = new long[256];

			public long this[byte index] 
			{
				get { return table [index]; }
				set { table [index] = value; }
			}

			public ByteTable add(ByteTable other)
			{
				for (int i = 0; i < 256; i++) {
					table[i] += other.table[i];
				}
				return this;
			}

			public bool Equals(ByteTable other)
			{
				for (int i = 0; i < 256; i++) {
					if (table [i] != other.table [i]) {
						return false;
					}
				}
				return true;
			}
		}

		private static ByteTable CountFromStream(string fileName)
		{
			using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				var table = new ByteTable ();
				while (true)
				{
					int read = file.ReadByte();
					if (read == -1)
					{
						return table;
					}
					table [(byte)read]++;
				}
			}
		}

		private static ByteTable CountFromMemory(string fileName)
		{
			byte[] buffer;
			using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				int count = (int)file.Length;
				buffer = new byte[count];
				file.Read(buffer, 0, count);
			}

			return buffer.CountBytes();
		}

		private static ByteTable CountFromChunks(string fileName)
		{
			return ReadFileChunks(fileName).Select(chunk => chunk.CountBytes()).Sum();
		}


		private static ByteTable CountFromChunksParallel(string fileName)
		{
			return ReadFileChunks(fileName).AsParallel().Select(chunk => chunk.CountBytes()).Sum();
		}

		private static ByteTable CountProducerConsumer(string fileName)
		{
			var queue = new BlockingCollection<byte[]>(100);

			var producer = new Thread (() => {

				foreach (var chunk in ReadFileChunks(fileName))
				{
					queue.Add(chunk);
				}
				queue.Add(null);
			});

			var table = new ByteTable();

			var consumer = new Thread (() => {
				while (true) {
					var buffer = queue.Take();
					if (buffer == null)
					{
						return;
					}
					table.add(buffer.CountBytes());
				}
			});

			producer.Start ();
			consumer.Start ();

			producer.Join ();
			consumer.Join ();

			return table;
		}

		private static ByteTable CountProducerNConsumers(string fileName, int n)
		{
			var queue = new BlockingCollection<byte[]>(100);

			var producer = new Thread (() => {

				foreach (var chunk in ReadFileChunks(fileName))
				{
					queue.Add(chunk);
				}
			});
				
			var table = new ByteTable();

			var consumers = Enumerable.Range(0, n).Select(_ => {
				return new Thread (() => {
					while (true) {
						var buffer = queue.Take();
						if (buffer == null)
						{
							return;
						}
						var count = buffer.CountBytes();

						lock(table)
						{
							table.add(count);
						}
					}
				});
			}).ToArray();


			producer.Start ();
			foreach (var c in consumers) c.Start ();

			producer.Join ();

			foreach (var c in consumers) queue.Add(null);
			foreach (var c in consumers) c.Join ();

			return table;
		}
			
		private static ByteTable CountBytes(this byte[] buffer)
		{
			var table = new ByteTable ();
			for (int i = 0; i < buffer.Length; i++) {
				table[buffer [i]]++;
			}
			return table;
		}

		private static ByteTable Sum(this IEnumerable<ByteTable> tables)
		{
			return tables.Aggregate(new ByteTable(), (a,b) => a.add(b));
		}

		private static IEnumerable<byte[]> ReadFileChunks(string fileName, int chunkSize = 1024 * 1024)
		{
			using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				while (true)
				{
					var buffer = new byte[chunkSize];
					var read = file.Read(buffer, 0, chunkSize);
					if (read < chunkSize)
					{
						yield return buffer.Take(read).ToArray();
						yield break;
					}
					yield return buffer;
				}
			}
		}
	}
}
