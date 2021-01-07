using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public static class DialogService
    {
        public static bool? ShowLoginDialog(LoginViewModel login, SignUpViewModel signup)
        {
            LoginDialog dialog = new LoginDialog(login, signup);

            return dialog.ShowDialog();
        }

        public static bool? ShowAddBusDialog(AddBusViewModel addBus)
        {
            AddBusDialog dialog = new AddBusDialog(addBus);

            return dialog.ShowDialog();
        }

        public static bool? ShowAddStationDialog(AddStationViewModel addStation)
        {
            AddStationDialog dialog = new AddStationDialog(addStation);

            return dialog.ShowDialog();
        }
    }
}
