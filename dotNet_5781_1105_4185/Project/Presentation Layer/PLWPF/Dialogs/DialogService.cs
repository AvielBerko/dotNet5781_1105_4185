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

        public static bool? ShowAddUpdateStationDialog(AddUpdateStationViewModel addStation)
        {
            AddUpdateStationDialog dialog = new AddUpdateStationDialog(addStation);

            return dialog.ShowDialog();
        }

        public static bool? ShowSelectStationsDialog(SelectStationsViewModel selectStations)
        {
            SelectStationsDialog dialog = new SelectStationsDialog(selectStations);

            return dialog.ShowDialog();
        }

        public static bool? ShowStationDetailsDialog(StationDetailsViewModel stationDetails)
        {
            StationDetailsDialog dialog = new StationDetailsDialog(stationDetails);

            return dialog.ShowDialog();
        }

        public static bool? ShowAddUpdateBusLineDialog(AddUpdateBusLineViewModel addBusline)
        {
            AddUpdateBusLineDialog dialog = new AddUpdateBusLineDialog(addBusline);

            return dialog.ShowDialog();
        }
    }
}
