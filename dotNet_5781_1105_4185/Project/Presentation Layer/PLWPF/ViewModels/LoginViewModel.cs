using BLAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class LoginViewModel : BaseViewModel, IDialogHelper
    {
        string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        string password;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        bool authFailure;
        public bool AuthFailure
        {
            get => authFailure;
            set
            {
                authFailure = value;
                OnPropertyChanged(nameof(AuthFailure));
            }
        }

        public RelayCommand Login { get; }

        public LoginViewModel()
        {
            Login = new RelayCommand((obj) => _Login(),
                (obj) => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Password));
        }

        private void _Login()
        {
            try
            {
                BO.User user = (BO.User)BlWork(bl => bl.UserAuthentication(Name, Password));
                AuthFailure = false;
                OnLoggedIn(user);
                OnRequestClose(true);
            }
            catch (BO.BadAuthenticationException)
            {
                AuthFailure = true;
            }
        }

        public delegate void LoggedInEventHandler(object sender, BO.User user);
        public event LoggedInEventHandler LoggedIn;

        protected virtual void OnLoggedIn(BO.User user) => LoggedIn?.Invoke(this, user);

        public event DialogService.RequestClose RequestClose;
        protected virtual void OnRequestClose(bool? result) => RequestClose?.Invoke(this, result);
    }
}
