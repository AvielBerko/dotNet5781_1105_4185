using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BLAPI;
using DLAPI;

namespace BL
{
    class BLImp : IBL
    {
        private readonly IDL dl = DLFactory.GetDL();

        public BO.User UserAuthentication(string name, string password)
        {
            try
            {
                var doUser = dl.GetUser(name);

                if (doUser.Password != password)
                    throw new BO.BadAuthenticationException(name, password, $"User with the name {name} doesn't have the password {password}.");

                return (BO.User)doUser.CopyPropertiesToNew(typeof(BO.User));
            }
            catch (DO.BadUserNameException)
            {
                throw new BO.BadAuthenticationException(name, password, $"User with the name {name} not found.");
            }
        }

        public BO.User UserSignUp(string name, string password)
        {
            if (string.IsNullOrEmpty(name)) throw new BO.BadSignUpException(name, password);

            ValidatePassword(password);

            try
            {
                dl.AddUser(new DO.User { Name = name, Password = password, Role = DO.Roles.Normal });
                return new BO.User { Name = name, Password = password, Role = BO.Roles.Normal };
            }
            catch (DO.BadUserNameException)
            {
                throw new BO.BadSignUpException(name, password);
            }
        }

        public void ValidatePassword(string password)
        {
            if (password.Length < 8)
                throw new BO.BadPasswordValidationException(password, "Password should contains at least 8 characters");

            bool hasLower = false;
            bool hasUpper = false;
            bool hasDigit = false;
            bool hasPanctuation = false;

            foreach (char ch in password)
            {
                if (char.IsLower(ch)) hasLower = true;
                if (char.IsUpper(ch)) hasUpper = true;
                if (char.IsDigit(ch)) hasDigit = true;
                if (char.IsPunctuation(ch)) hasPanctuation = true;
            }

            if (!(hasLower && hasUpper && hasDigit && hasPanctuation))
                throw new BO.BadPasswordValidationException(password,
                    "The password should contain at least 1 lower case letter, 1 upper case letter, 1 digit and 1 panctuation");
        }
    }
}
