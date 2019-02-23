using DevExpress.Mvvm;

namespace LocalChat.Client.Wpf
{
    public class LoginViewModel : BindableBase
    {
        public string Username
        {
            get { return GetProperty(() => Username); }
            set { SetProperty(() => Username, value); }
        }
    }
}