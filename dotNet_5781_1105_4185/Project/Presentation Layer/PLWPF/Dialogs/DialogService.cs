using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public static class DialogService
    {
        public delegate void RequestClose(object sender, bool? result);

        public static bool? ShowLoginDialog(LoginViewModel login, SignUpViewModel signup)
        {
            LoginDialog dialog = new LoginDialog(login, signup);

            RequestClose close = (sender, result) => dialog.DialogResult = result;
            login.RequestClose += close;
            signup.RequestClose += close;

            return dialog.ShowDialog();
        }

        public static bool? ShowAddBusDialog(AddBusViewModel addBus)
        {
            AddBusDialog dialog = new AddBusDialog(addBus);

            addBus.RequestClose = (sender, result) => dialog.DialogResult = result;

            return dialog.ShowDialog();
        }
    }
}
