using BLAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PL
{
    public class LoginViewModel : BaseViewModel
    {
        /// <summary>
        /// The authenticated user.
        /// </summary>
        public BO.User User
        {
            get => _user;
            private set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }
        private BO.User _user;

        /// <summary>
        /// The name of the user for authentication.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string _name;

        /// <summary>
        /// The password of the user for authentication.
        /// </summary>
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        private string _password;

        /// <summary>
        /// Indicates whether a authentication failure occured.
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        private string _errorMessage;

        /// <summary>
        /// Command used in order to sign up a new user.
        /// </summary>
        public RelayCommand Signup { get; }
        /// <summary>
        /// Command used in order to login an existing user.
        /// </summary>
        public RelayCommand Login { get; }

        public LoginViewModel()
        {
            Login = new RelayCommand(
                async window => await _Login(window),
                (obj) => !string.IsNullOrEmpty(Name) &&
                         !string.IsNullOrEmpty(Password)
            );
            Signup = new RelayCommand(async window => await _Signup(window),
                (obj) => !string.IsNullOrEmpty(Name) &&
                         !string.IsNullOrEmpty(Password)
            );
        }

        /// <summary>
        /// Logs in. If successfuly logged in, closes the dialog and sets AuthFailure to false.
        /// Else sets AuthFailure to true.
        /// </summary>
        /// <param name="window">The window object of the dialog to close</param>
        private async Task _Login(object window)
        {
            await Load(async () =>
            {
                try
                {
                    User = (BO.User)await BlWorkAsync(bl => bl.UserAuthentication(Name, Password));
                    DialogService.CloseDialog(window, DialogResult.Ok);
                }
                catch (BO.BadAuthenticationException ex)
                {
                    ErrorMessage = ex.Message;
                }
            });
        }

        /// <summary>
        /// Signs up the user and closes the dialog
        /// </summary>
        /// <param name="window">The window object of the dialog to close</param>
        private async Task _Signup(object window)
        {
            await Load(async () =>
            {
                try
                {
                    User = (BO.User)await BlWorkAsync(bl => bl.UserSignUp(Name, Password));
                    DialogService.CloseDialog(window, DialogResult.Ok);
                }
                catch (BO.BadNameValidationException ex)
                {
                    ErrorMessage = ex.Message;
                }
                catch (BO.BadPasswordValidationException ex)
                {
                    ErrorMessage = ex.Message;
                }
            });
        }
    }
}