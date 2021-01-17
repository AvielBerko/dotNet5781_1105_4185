using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public interface IDialogService
    {
        void CloseDialog(object window, DialogResult result);

        DialogResult ShowLoginDialog(LoginViewModel login, SignUpViewModel signup);

        DialogResult ShowAddBusDialog(AddBusViewModel addBus);

        DialogResult ShowAddUpdateStationDialog(AddUpdateStationViewModel addStation);

        DialogResult ShowSelectStationsDialog(SelectStationsViewModel selectStations);

        DialogResult ShowStationDetailsDialog(StationDetailsViewModel stationDetails);

        DialogResult ShowAddUpdateBusLineDialog(AddUpdateBusLineViewModel addBusline);

        DialogResult ShowYesNoDialog(string message, string title);
    }
}
