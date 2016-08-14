using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PeerToPeerSharing
{
    public delegate void ProccessConnection();
    public partial class Peer
    {
        public event ProccessConnection Connect;
        public event ProccessConnection Disconnect;
        public event ProccessConnection Pause;
        public event ProccessConnection Resume;

        private TcpClient Client { get; set; }
        private FileStream FileStream { get; set; }
        private NetworkStream NetworkStream { get; set; }

        public long ChunkSize
        {
            get
            {
                return FileSize / BlockSize;
            }
        }
        public long BlockSize { get; set; }
        public long FileSize { get; set; }

        public Peer()
        {
            this.Client = new TcpClient();
        }


        public async void ConnectPeer(string ipAddress, int port)
        {
            IPAddress address = IPAddress.Parse(ipAddress);


            try
            {
                await Client.ConnectAsync(address, port);
                Connect();
            }
            catch
            {
                Disconnect();
                throw new Exception("Error during peer connection");


            }
        }


        public async void SendFileInfo(string destination)
        {
            var file = new FileInfo(destination);
            this.FileStream = file.OpenRead();
            this.NetworkStream = this.Client.GetStream();
            byte[] fileLength = BitConverter.GetBytes(file.Length);
            this.FileSize = file.Length;

            //write file length
            await this.NetworkStream.WriteAsync(fileLength, 0, fileLength.Length);
        }

        public async void SeedFile(string destination)
        {
            int read;
            byte[] buffer = new byte[this.ChunkSize];

            //Start processing information
            try
            {
                while ((read = await this.FileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await this.FileStream.WriteAsync(buffer, 0, read);
                }
            }
            catch (Exception)
            {
                Disconnect();
            }
            finally
            {
                this.FileStream.Dispose();
                this.Client.Close();
            }

        }

    }
}
