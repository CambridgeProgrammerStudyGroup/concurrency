using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ByteContentsCounter
{
    public class Sequential : IByteContentsCounter
    {
        public IDictionary<byte, int> ComputeCounts(string filePath)
        {
            var results = new Dictionary<byte, int>();

            using (Stream source = File.OpenRead(filePath))
            {
                byte[] buffer = new byte[1024*1024]; //1MB buffer
                int bytesRead;
                while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    foreach (var b in buffer.Take(bytesRead))
                    {
                        int currentCount;
                        results.TryGetValue(b, out currentCount);
                        results[b] = currentCount + 1;
                    }
                }
            }

            return results;
        }
    }
}
