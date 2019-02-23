using DevExpress.Mvvm;
using LocalChat.Client.Wpf.Views;
using LocalChat.Domain;
using LocalChat.Domain.Client;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace LocalChat.Client.Wpf
{
    public class MainViewModel : BindableBase
    {
        public ObservableCollection<Message> Messages { get; } = new ObservableCollection<Message>();

        public string CurrentMessage
        {
            get { return GetProperty(() => CurrentMessage); }
            set { SetProperty(() => CurrentMessage, value); }
        }

        private ChatClient _chatClient;
        private Thread _listeningThread;

        public ICommand WindowLoadedCommand
            => new DelegateCommand(async () =>
            {
                var view = new LoginDialog
                {
                    DataContext = new LoginViewModel()
                };

                var username = (string)await DialogHost.Show(view, "RootDialog");

                _chatClient = new ChatClient("127.0.0.1", 8005);

                while (!_chatClient.TrySetUsername(username))
                {
                    MessageBox.Show($"The username '{username}' already exists.");
                    username = (string)await DialogHost.Show(view, "RootDialog");
                }

                _chatClient.MessageReceive += OnMessageReceived;

                _listeningThread = new Thread(new ThreadStart(_chatClient.Process));
                _listeningThread.Start();
            });

        public ICommand SendMessage
            => new DelegateCommand(() =>
            {
                if (string.IsNullOrEmpty(CurrentMessage) || string.IsNullOrWhiteSpace(CurrentMessage))
                    return;

                _chatClient.SendMessage(CurrentMessage);

                CurrentMessage = "";
            });

        public ICommand WindowClosingCommand
            => new DelegateCommand(() => Environment.Exit(0));

        public void OnMessageReceived(object sender, MessageReceiveEventArgs messageReceiveEventArgs)
        {
            System.Windows.Application.Current.Dispatcher
                .Invoke(() => Messages.Add(messageReceiveEventArgs.Message));
        }
    }
}
