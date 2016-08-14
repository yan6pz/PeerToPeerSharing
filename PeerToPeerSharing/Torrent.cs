using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeerToPeerSharing
{
    public class Torrent
    {

        public Torrent(string fileName, long size)
        {
            this.Name = fileName;
            this.TotalSize = size;
        }

        public string Name { get; set; }
        public string Directory { get; set; }
        public long Size { get; set; }
        public long TotalSize { get; set; }
        public long BlockSize { get; set; }
    }
}
