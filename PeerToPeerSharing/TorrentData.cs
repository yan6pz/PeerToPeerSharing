using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeerToPeerSharing
{
    public class TorrentData
    {
        public byte[] Data { get; set; }
        public long SeqNumber { get; set; }
    }
}
