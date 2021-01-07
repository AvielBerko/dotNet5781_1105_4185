using BLAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class LoginViewModel : BaseDialogViewModel
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
            Login = new RelayCommand(_Login,
                (obj) => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Password));
        }

        private void _Login(object window)
        {
            try
            {
                BO.User user = (BO.User)BlWork(bl => bl.UserAuthentication(Name, Password));
                AuthFailure = false;
                OnLoggedIn(user);
                CloseDialog(window, true);
            }
            catch (BO.BadAuthenticationException)
            {
                AuthFailure = true;
            }
        }

        public delegate void LoggedInEventHandler(object sender, BO.User user);
        public event LoggedInEventHandler LoggedIn;
        protected virtual void OnLoggedIn(BO.User user) => LoggedIn?.Invoke(this, user);
    }
}
