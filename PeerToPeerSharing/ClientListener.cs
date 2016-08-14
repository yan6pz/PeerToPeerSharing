using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PeerToPeerSharing
{
    public class ClientListener
    {
        private TcpListener Listener { get; set; }
        private TcpClient TcpClient { get; set; }

        public async void Start(string address,int port)
        {
            await Task.Run(()=>FindPeers(address,port));
        }

        private void FindPeers(string address, int port)
        {
            AddNewConnection(address, port);
        }

        private NetworkStream NStream { get; set; }
        private Torrent CurrentTorrent { get; set; }
        public string FileName { get; private set; }

        public ConcurrentDictionary<string, Peer> Peers = new ConcurrentDictionary<string, Peer>();

        private ConcurrentQueue<TorrentData> OutgoingData = new ConcurrentQueue<TorrentData>();
        private ConcurrentQueue<TorrentData> IncomingData = new ConcurrentQueue<TorrentData>();

        private const short fileLength = 4;

        public  ClientListener(int port)
        {
            this.Listener= TcpListener.Create(port);
            this.Listener.Start();
            InstantiateTcpClient();
            this.NStream = this.TcpClient.GetStream();
        }

        private async void InstantiateTcpClient()
        {
            TcpClient client = await this.Listener.AcceptTcpClientAsync();
        }

        public async void GetFileInfo()
        {
            byte[] fileLengthBytes = new byte[fileLength];
            var fileNameBytes = new byte[fileLength];
            await this.NStream.ReadAsync(fileNameBytes, 0, fileNameBytes.Length);
            await this.NStream.ReadAsync(fileLengthBytes, 0, fileLengthBytes.Length);

            this.FileName = ASCIIEncoding.ASCII.GetString(fileNameBytes);
            this.CurrentTorrent = new Torrent(this.FileName, BitConverter.ToInt64(fileLengthBytes, 0));
        }

        public async void SaveFile()
        {
            var fileStream = File.Open(this.FileName, FileMode.Create);
            int read;
            byte[] buffer = new byte[CurrentTorrent.TotalSize/32];

            //Start processing information
            try
            {
                while ((read = await this.NStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, read);
                }
            }
            catch (Exception)
            {
                //Disconnect();
            }
            finally
            {
                fileStream.Dispose();
                this.NStream.Close();
            }
        }

        private void AddNewConnection(string address,int port)
        {
            InstantiateTcpClient();

            new Peer().ConnectPeer(address,port);
        }


    }
}
