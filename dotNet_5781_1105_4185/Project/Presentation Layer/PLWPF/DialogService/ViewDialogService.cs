using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL
{
    public class ViewDialogService : IDialogService
    {
        static readonly Lazy<ViewDialogService> lazy = new Lazy<ViewDialogService>(() => new ViewDialogService());
        public static ViewDialogService Instance => lazy.Value;

        private ViewDialogService() { }

        public void CloseDialog(object window, DialogResult result)
        {
            switch (result)
            {
                case DialogResult.Ok:
                case DialogResult.Yes:
                    ((Window)window).DialogResult = true;
                    break;
                case DialogResult.No:
                case DialogResult.Cancel:
                    ((Window)window).DialogResult = false;
                    break;
                default:
                case DialogResult.None:
                    ((Window)window).DialogResult = null;
                    break;
            }
        }

        public DialogResult ShowLoginDialog(LoginViewModel login, SignUpViewModel signup)
        {
            LoginDialog dialog = new LoginDialog(login, signup);

            switch (dialog.ShowDialog())
            {
                case true:
                    return DialogResult.Ok;
                case false:
                    return DialogResult.Cancel;
                default:
                    return DialogResult.None;
            }
        }

        public DialogResult ShowAddBusDialog(AddBusViewModel addBus)
        {
            AddBusDialog dialog = new AddBusDialog(addBus);

            switch (dialog.ShowDialog())
            {
                case true:
                    return DialogResult.Ok;
                case false:
                    return DialogResult.Cancel;
                default:
                    return DialogResult.None;
            }
        }

        public DialogResult ShowAddUpdateStationDialog(AddUpdateStationViewModel addStation)
        {
            AddUpdateStationDialog dialog = new AddUpdateStationDialog(addStation);

            switch (dialog.ShowDialog())
            {
                case true:
                    return DialogResult.Ok;
                case false:
                    return DialogResult.Cancel;
                default:
                    return DialogResult.None;
            }
        }

        public DialogResult ShowSelectStationsDialog(SelectStationsViewModel selectStations)
        {
            SelectStationsDialog dialog = new SelectStationsDialog(selectStations);

            switch (dialog.ShowDialog())
            {
                case true:
                    return DialogResult.Ok;
                case false:
                    return DialogResult.Cancel;
                default:
                    return DialogResult.None;
            }
        }

        public DialogResult ShowStationDetailsDialog(StationDetailsViewModel stationDetails)
        {
            StationDetailsDialog dialog = new StationDetailsDialog(stationDetails);

            switch (dialog.ShowDialog())
            {
                case true:
                    return DialogResult.Ok;
                case false:
                    return DialogResult.Cancel;
                default:
                    return DialogResult.None;
            }
        }

        public DialogResult ShowAddUpdateBusLineDialog(AddUpdateBusLineViewModel addBusline)
        {
            AddUpdateBusLineDialog dialog = new AddUpdateBusLineDialog(addBusline);

            switch (dialog.ShowDialog())
            {
                case true:
                    return DialogResult.Ok;
                case false:
                    return DialogResult.Cancel;
                default:
                    return DialogResult.None;
            }
        }

        public DialogResult ShowYesNoDialog(string message, string title)
        {
            switch (MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Exclamation))
            {
                case MessageBoxResult.Yes:
                    return DialogResult.Yes;
                case MessageBoxResult.No:
                    return DialogResult.No;
                default:
                    return DialogResult.None;
            }
        }
    }
}
