using LocalChat.Domain.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LocalChat.Domain.Server
{
    public class ChatServer : IChatServer
    {   
        private static readonly int _port = 8005;
        private static readonly IPEndPoint _serverSocketEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), _port);

        private Socket _serverSocket;
        private volatile bool _isActive;

        /// <summary>
        /// Gets or sets chat server clients.
        /// </summary>
        public List<ChatClient> Clients { get; } = new List<ChatClient>();

        /// <inheritdoc/>
        public event EventHandler<MessageReceiveEventArgs> MessageReceive;

        /// <summary>
        /// Gets a chat server messages history.
        /// </summary>
        public ChatHistory History { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ChatServer"/>.
        /// </summary>
        /// <param name="messagesHistoryCount">Count of the stored messages in history.</param>
        public ChatServer(int messagesHistoryCount)
        {      
            _serverSocket = new Socket(_serverSocketEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            History = new ChatHistory(messagesHistoryCount);
        }

        /// <inheritdoc/>
        public void Start()
        {
            if (_isActive)
            {
                return;
            }

            _serverSocket.Bind(_serverSocketEP);
            try
            {
                _serverSocket.Listen((int)SocketOptionName.MaxConnections);
            }
            // When there is an exception unwind previous actions (bind etc) 
            catch (SocketException)
            {
                Stop();
                throw;
            }
            _isActive = true;
        }

        /// <inheritdoc/>
        public void Stop()
        {
            if (_serverSocket != null)
            {
                _serverSocket.Close();
                _serverSocket = null;
            }
            _isActive = false;
            _serverSocket = new Socket(_serverSocketEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        public ChatClient AcceptClient()
        {
            Socket acceptedSocket = _serverSocket.Accept();
            return new ChatClient(acceptedSocket);
        }

        /// <inheritdoc/>
        public void Process(object chatClient)
        {
            var client = chatClient as ChatClient;

            LoginNewClient(client);

            while (client.IsActive)
            {
                try
                {
                    var messageText = client.ReadMessage();
                    var message = new Message(messageText, DateTime.Now);
                    MessageReceive?.Invoke(this, new MessageReceiveEventArgs { Message = message});
                    BroadcastMessage(messageText);
                    History.AddNewMessage(message);
                }
                catch (SocketException e)
                {
                    if (e.SocketErrorCode != SocketError.ConnectionAborted && e.SocketErrorCode != SocketError.ConnectionReset)
                        throw e;

                    Clients.Remove(client);
                    var message = new Message($"{client.Username} left the chat.", DateTime.Now);
                    MessageReceive?.Invoke(this, new MessageReceiveEventArgs { Message = message });
                    BroadcastMessage(message.Text);
                    History.AddNewMessage(message);
                    break;
                }
            }
        }

        /// <inheritdoc/>
        public void BroadcastMessage(string message)
        {
            foreach (var client in Clients)
            {
                SendMessage(client, message);
            }        
        }

        private void SendMessage(ChatClient client, string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            byte[] dataSize = new byte[4];

            dataSize = BitConverter.GetBytes(data.Length);

            client.ClientSocket.Send(dataSize);
            client.ClientSocket.Send(data);
        }

        private void LoginNewClient(ChatClient client)
        {
            while (client.IsActive)
            {
                var messageText = client.ReadMessage();
                var username = messageText.Replace("Login:", "");

                if (string.IsNullOrEmpty(username))
                {
                    var text = $"Username can not be empty.";
                    if (IsClientSocketConnected(client))
                    {
                        SendMessage(client, text);
                        continue;
                    }
                    
                    else
                        break;
                }

                if (Clients.FirstOrDefault(c => c.Username == username) == null)
                {
                    client.Username = username;
                    Clients.Add(client);

                    var newMessage = new Message($"{username} joined the chat.", DateTime.Now);
                    MessageReceive?.Invoke(this, new MessageReceiveEventArgs { Message = newMessage });
                    BroadcastMessage(newMessage.Text);

                    try
                    {
                        foreach (var m in History.MessagesHistory)
                        {
                            var text = new StringBuilder($"{m.Username}:{m.Text}:DateTime:{m.CreatedDate}");
                            SendMessage(client, text.ToString());
                        }
                        History.AddNewMessage(newMessage);
                        break;
                    }
                    catch(SocketException e)
                    {
                        if (e.SocketErrorCode != SocketError.ConnectionAborted && e.SocketErrorCode != SocketError.ConnectionReset)
                            throw e;

                        Clients.Remove(client);
                        var message = new Message($"{client.Username} left the chat.", DateTime.Now);
                        MessageReceive?.Invoke(this, new MessageReceiveEventArgs { Message = message });
                        BroadcastMessage(message.Text);
                        History.AddNewMessage(message);
                    }   
                }

                else
                {
                    var text = $"The username '{username}' already exists.";
                    if (IsClientSocketConnected(client))
                        SendMessage(client, text);
                    else
                        break;
                }
            }
        }

        private bool IsClientSocketConnected(ChatClient client)
        {
            var socket = client.ClientSocket;
            return !((socket.Poll(1000, SelectMode.SelectRead) && (socket.Available == 0)) || !socket.Connected);
        }           
    }
}
