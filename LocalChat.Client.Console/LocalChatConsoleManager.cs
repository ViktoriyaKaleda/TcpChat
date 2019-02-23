using LocalChat.Domain;
using LocalChat.Domain.Client;
using System;
using System.Collections.Generic;
using System.Threading;

namespace LocalChat.Client.Console
{
    public class LocalChatConsoleManager
    {
        private List<Message> Messages { get; }

        private readonly ChatClient _localChat;

        private string _userInput;

        public LocalChatConsoleManager()
        {
            Messages = new List<Message>();            

            _localChat = new ChatClient("127.0.0.1", 8005);            

            _localChat.MessageReceive += PrintReceivedMessageToConsole;
        }

        public void StartChating()
        {
            string username;
            do
            {
                System.Console.Write("Enter your username, please:");
                username = System.Console.ReadLine();
            }
            while (_localChat.TrySetUsername(username) != true);

            System.Console.WriteLine($"You logged in as '{username}'.");

            Thread clientThread = new Thread(new ThreadStart(_localChat.Process));
            clientThread.Start();
            ShowMessageInput();
        }

        private void ShowMessageInput()
        {
            ConsoleKeyInfo e;

            while (_localChat.IsActive)
            {
                e = System.Console.ReadKey();

                if (e.Key == ConsoleKey.Enter)
                {
                    if (string.IsNullOrEmpty(_userInput) || string.IsNullOrWhiteSpace(_userInput))
                    {
                        _userInput = "";
                        continue;
                    }

                    _localChat.SendMessage(_userInput);

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
