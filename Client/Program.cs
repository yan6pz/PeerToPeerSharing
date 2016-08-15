using PeerToPeerSharing;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            const int port = 1234;
            //const string address = "192.168.56.1";
            var client = new ClientListener(port);
            client.Start();

        }
    }
}
