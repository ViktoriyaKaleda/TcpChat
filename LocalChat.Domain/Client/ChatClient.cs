using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LocalChat.Domain.Client
{
    /// <summary>
    /// Represents a chat client.
    /// </summary>
    public class ChatClient : IChatClient
    {
        /// <inheritdoc/>
        public event EventHandler<MessageReceiveEventArgs> MessageReceive;

        /// <summary>
        /// Gets or sets a username.
        /// </summary>
        public string Username { get; internal set; }

        /// <summary>
        /// Gets or sets is a chat client active.
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Gets or sets a chat client socket.
        /// </summary>
        internal Socket ClientSocket { get; private set; }       

        /// <summary>
        /// Gets or sets a chat client data stream.
        /// </summary>
        internal NetworkStream DataStream { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatClient"/>.
        /// </summary>
        /// <param name="socket">A socket.</param>
        internal ChatClient(Socket socket)
        {
            ClientSocket = socket;
            IsActive = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatClient"/>.
        /// </summary>
        /// <param name="hostname">A hostname string.</param>
        /// <param name="port">A port number.</param>
        public ChatClient(string hostname, int port)
        {
            if (hostname == null)
            {
                throw new ArgumentNullException(nameof(hostname));
            }

            if (!ValidateTcpPort(port))
            {
                throw new ArgumentOutOfRangeException("port");
            }

            try
            {
                ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(hostname), port);
                ClientSocket.Connect(ipPoint);
                IsActive = true;
            }

            catch (Exception e)
            {
                if (e is ThreadAbortException || e is StackOverflowException || e is OutOfMemoryException)
                {
                    throw;
                }

                if (ClientSocket != null)
                {
                    ClientSocket.Close();
                }
                throw e;
            }
        }

        /// <inheritdoc/>
        public void Process()
        {
            while (IsActive)
            {
                try
                {
                    string message = ReadMessage();
                    MessageReceive?.Invoke(this, new MessageReceiveEventArgs { Message = new Message(message, DateTime.Now) });
                }
                catch(Exception e)
                {
                    MessageReceive?.Invoke(this, new MessageReceiveEventArgs { Message = new Message("Server disconnected.", DateTime.Now) });
                    Close();
                    break;
                }                
            }
        }

        /// <inheritdoc/>
        public void SendMessage(string message)
        {
            if (!string.IsNullOrEmpty(Username))
                message = Username + ":" + message;

            var data = Encoding.Unicode.GetBytes(message);
            
            byte[] dataSize = new byte[4];

            dataSize = BitConverter.GetBytes(data.Length);

            var stream = GetStream();
            stream.Write(dataSize, 0, dataSize.Length);
            stream.Write(data, 0, data.Length);
        }

        /// <inheritdoc/>
        public bool TrySetUsername(string username)
        {           
            SendMessage($"Login:{username}");
            try
            {
                string message = ReadMessage();
                if (message.Contains("joined the chat."))
                {
                    Username = username;
                    return true;
                }
                    
                MessageReceive?.Invoke(this, new MessageReceiveEventArgs { Message = new Message(message, DateTime.Now) });
            }
            catch
            {
                MessageReceive?.Invoke(this, new MessageReceiveEventArgs { Message = new Message("Server disconnected.", DateTime.Now) });
                Close();                
            }
            return false;
        }

        /// <inheritdoc/>
        public void Close()
        {
            ClientSocket.Close();
            IsActive = false;
        }

        internal string ReadMessage()
        {
            byte[] sizeInfo = new byte[4];

            //read the size of the message
            int totalRead = 0, currentTread = 0;

            currentTread = totalRead = ClientSocket.Receive(sizeInfo);

            while (totalRead < sizeInfo.Length && currentTread > 0)
            {
                currentTread = ClientSocket.Receive(sizeInfo, totalRead, sizeInfo.Length - totalRead, SocketFlags.None);
                totalRead += currentTread;
            }

            int messageSize = 0;

            messageSize = BitConverter.ToInt32(sizeInfo, 0);
            
            byte[] data = new byte[messageSize];

            //read the first chunk of data
            totalRead = 0;
            currentTread = totalRead = ClientSocket.Receive(data, totalRead, data.Length - totalRead, SocketFlags.None);

            //if we didn't get the entire message, read some more until we do
            while (totalRead < messageSize && currentTread > 0)
            {
                currentTread = ClientSocket.Receive(data, totalRead, data.Length - totalRead, SocketFlags.None);
                totalRead += currentTread;
            }

            return Encoding.Unicode.GetString(data, 0, totalRead);
        }

        private NetworkStream GetStream()
        {
            if (!ClientSocket.Connected)
            {
                throw new InvalidOperationException("Socket is not connected.");
            }
            if (DataStream == null)
            {
                DataStream = new NetworkStream(ClientSocket, true);
            }
            return DataStream;
        }

        private static bool ValidateTcpPort(int port)
        {
            return port >= IPEndPoint.MinPort && port <= IPEndPoint.MaxPort;
        }
    }
}
