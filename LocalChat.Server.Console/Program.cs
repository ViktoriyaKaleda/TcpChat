using LocalChat.Domain;
using LocalChat.Domain.Client;
using LocalChat.Domain.Server;
using System.Threading;
using System.Threading.Tasks;

namespace LocalChat.Server.Console
{
    class Program
    {
        static ChatServer chatServer = new ChatServer(5);

        static void Main(string[] args)
        {            
            chatServer.Start();
            chatServer.MessageReceive += PrintReceivedMessageToConsole;
            System.Console.WriteLine("Waiting for connections...");

            while (true)
            {
                ChatClient clientObject = chatServer.AcceptClient();

                // The first homework task: implement the client and server using the “For each client - own processing thread” scheme for the server
                //Thread clientThread = new Thread(new ParameterizedThreadStart(chatServer.Process));
                //clientThread.Start(clientObject);

                // The second task: using Task Parallel Library 
                Task.Run(() => chatServer.Process(clientObject));
            }            
        }

        private static void PrintReceivedMessageToConsole(object sender, MessageReceiveEventArgs messageReceiveEventArgs)
        {
            var message = messageReceiveEventArgs.Message;
            System.Console.WriteLine($"{message.CreatedDate.ToShortTimeString()} {message.Username}: {message.Text}");
        }
    }
}
