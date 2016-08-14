using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeerToPeerSharing.Interfaces
{
    public interface IEncoding
    {
        string EncodeInteger(int item);
        string EncodeString(string item);
        string EncodeDouble(double item);
    }
}
