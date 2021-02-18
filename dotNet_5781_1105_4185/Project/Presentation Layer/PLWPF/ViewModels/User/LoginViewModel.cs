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
    public class LoginViewModel : BaseViewModel, IDataErrorInfo
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
        public bool AuthFailure
        {
            get => _authFailure;
            set
            {
                _authFailure = value;
                OnPropertyChanged(nameof(AuthFailure));
            }
        }
        private bool _authFailure;

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
                         !string.IsNullOrEmpty(Password));
            Signup = new RelayCommand(
                async window => await _Signup(window),
                (obj) => ValidateSignUpName().IsValid &&
                         ValidateSignUpPassword().IsValid
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
                    AuthFailure = false;
                    DialogService.CloseDialog(window, DialogResult.Ok);
                }
                catch (BO.BadAuthenticationException)
                {
                    AuthFailure = true;
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
                // The signup sould not faile when this function executes.
                User = (BO.User)await BlWorkAsync(bl => bl.UserSignUp(Name, Password));
                DialogService.CloseDialog(window, DialogResult.Ok);
            });
        }

        public string this[string columnName]
        {
            get
            {
                // Returs the validation errors of a given property.
                switch (columnName)
                {
                    case nameof(Name):
                        return ValidateSignUpName().ErrorContent as string;
                    case nameof(Password):
                        return ValidateSignUpPassword().ErrorContent as string;
                    default:
                        return null;
                }
            }
        }

        public string Error => throw new NotImplementedException();

        /// <summary>
        /// Validates the Name to sign up by the bussiness layer.
        /// </summary>
        /// <returns>If the name is invalid a validation result with the error message. Else return valid result</returns>
        private ValidationResult ValidateSignUpName()
        {
            try
            {
                BlWork(bl => bl.ValidateSignUpName(Name));
                return ValidationResult.ValidResult;
            }
            catch (BO.BadNameValidationException ex)
            {
                return new ValidationResult(false, ex.Message);
            }
        }

        /// <summary>
        /// Validates the Password to sign up by the bussiness layer.
        /// </summary>
        /// <returns>If the passord is invalid a validation result with the error message. Else return valid result</returns>
        private ValidationResult ValidateSignUpPassword()
        {
            try
            {
                BlWork(bl => bl.ValidateSignUpPassword(Password));
                return ValidationResult.ValidResult;
            }
            catch (BO.BadPasswordValidationException ex)
            {
                return new ValidationResult(false, ex.Message);
            }
        }
    }
}
