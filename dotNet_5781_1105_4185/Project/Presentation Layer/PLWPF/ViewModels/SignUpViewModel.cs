﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PL
{
    public class SignUpViewModel : BaseViewModel, IDataErrorInfo
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
                OnPropertyChanged(nameof(Confirm));
            }
        }

        string confirm;
        public string Confirm
        {
            get => confirm;
            set
            {
                confirm = value;
                OnPropertyChanged(nameof(Confirm));
            }
        }

        public RelayCommand SignUp { get; }

        public SignUpViewModel()
        {
            SignUp = new RelayCommand((obj) => _SignUp(), (obj) =>
                ValidateName().IsValid &&
                ValidatePassword().IsValid &&
                ValidateConfirm().IsValid);
        }

        private void _SignUp()
        {
            BO.User user = (BO.User)BlWork(bl => bl.UserSignUp(Name, Password));
            OnSignedUp(user);
        }

        public delegate void SignedUpEventHandler(object sender, BO.User user);
        public event SignedUpEventHandler SignedUp;
        protected virtual void OnSignedUp(BO.User user) => SignedUp?.Invoke(this, user);

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Name):
                        return ValidateName().ErrorContent as string;
                    case nameof(Password):
                        return ValidatePassword().ErrorContent as string;
                    case nameof(Confirm):
                        return ValidateConfirm().ErrorContent as string;
                    default:
                        return null;
                }
            }
        }

        public string Error => throw new NotImplementedException();

        private ValidationResult ValidateName()
        {
            try
            {
                BlWork(bl => { bl.ValidateSignUpName(Name); return null; });
                return ValidationResult.ValidResult;
            }
            catch (BO.BadNameValidationException ex)
            {
                return new ValidationResult(false, ex.Message);
            }
        }

        private ValidationResult ValidatePassword()
        {
            try
            {
                BlWork(bl => { bl.ValidateSignUpPassword(Password); return null; });
                return ValidationResult.ValidResult;
            }
            catch (BO.BadPasswordValidationException ex)
            {
                return new ValidationResult(false, ex.Message);
            }
        }

        private ValidationResult ValidateConfirm()
        {
            if (Password == Confirm)
            {
                return ValidationResult.ValidResult;
            }

            return new ValidationResult(false, "Confirm is not equals to password");
        }

    }
}
