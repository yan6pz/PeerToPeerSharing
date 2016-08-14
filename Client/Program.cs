using PeerToPeerSharing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            const int port = 1234;
            const string address = "192.168.56.1";
            var client = new ClientListener(port);
            client.Start(address,port);
        }
    }
}
