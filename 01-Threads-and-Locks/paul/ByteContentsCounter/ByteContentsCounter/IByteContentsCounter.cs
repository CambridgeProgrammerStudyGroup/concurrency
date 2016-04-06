using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ByteContentsCounter
{
    interface IByteContentsCounter
    {
        IDictionary<byte, int> ComputeCounts(string filePath);
    }
}
