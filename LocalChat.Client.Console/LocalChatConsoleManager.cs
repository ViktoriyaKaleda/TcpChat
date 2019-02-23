using LocalChat.Domain;
using LocalChat.Domain.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LocalChat.Client.Console
{
    public class LocalChatConsoleManager
    {
        private List<Message> Messages { get; }

        private readonly ChatClient _chatClient;

        private string _userInput;

        public LocalChatConsoleManager()
        {
            Messages = new List<Message>();            

            _chatClient = new ChatClient("127.0.0.1", 8005);            

            _chatClient.MessageReceive += PrintReceivedMessageToConsole;
        }

        public void StartChating()
        {
            string username;
            do
            {
                System.Console.Write("Enter your username, please:");
                username = System.Console.ReadLine();
            }
            while (_chatClient.TrySetUsername(username) != true);

            System.Console.WriteLine($"You logged in as '{username}'.");

            // The first homework task: implement the client and server using the “For each client - own processing thread” scheme for the server
            //Thread clientThread = new Thread(new ThreadStart(_chatClient.Process));
            //clientThread.Start();

            // The second task: using Task Parallel Library 
            Task.Run(() => _chatClient.Process());

            ShowMessageInput();
        }

        private void ShowMessageInput()
        {
            ConsoleKeyInfo e;

            while (_chatClient.IsActive)
            {
                e = System.Console.ReadKey();

                if (e.Key == ConsoleKey.Enter)
                {
                    if (string.IsNullOrEmpty(_userInput) || string.IsNullOrWhiteSpace(_userInput))
                    {
                        _userInput = "";
                        continue;
                    }

                    _chatClient.SendMessage(_userInput);

                    _userInput = "";
                }

                else if (e.Key == ConsoleKey.Backspace)
                {
                    if (_userInput.Length == 0)
                        continue;

                    System.Console.Write(" \b");
                    _userInput = _userInput.Substring(0, _userInput.Length - 1);
                }

                else
                {
                    _userInput += e.KeyChar;
                }
            }
        }

        private void PrintReceivedMessageToConsole(object sender, MessageReceiveEventArgs messageReceiveEventArgs)
        {
            Messages.Add(messageReceiveEventArgs.Message);

            System.Console.Clear();

            foreach (var m in Messages)
            {
                System.Console.WriteLine($"{m.CreatedDate.ToShortTimeString()} {m.Username}: {m.Text}");
            }

            if (!string.IsNullOrEmpty(_userInput))
                System.Console.Write(_userInput);
        }
    }
}
