using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeerToPeerSharing.Interfaces
{
    public interface IDecode
    {
        int EncodeInteger(string item);
        string EncodeString(string item);
        double EncodeDouble(string item);
    }
}
