using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    /// <summary>
    /// Defines functions in order to run a dialog operations.
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// Closes the given dialog's window with a given result.
        /// </summary>
        /// <param name="window">The dialog's window.</param>
        /// <param name="result">The result to return.</param>
        void CloseDialog(object window, DialogResult result);

        /// <summary>
        /// Shows the login dialog and gives it the view model.
        /// </summary>
        /// <param name="login">The view model for the dialog.</param>
        /// <returns>The result of the dialog. Can be Ok or None.</returns>
        DialogResult ShowLoginDialog(LoginViewModel login);

        /// <summary>
        /// Shows the add bus dialog and gives it the view model.
        /// </summary>
        /// <param name="addBus">The view model for the dialog.</param>
        /// <returns>The result of the dialog. Can be Ok, Cancel or None.</returns>
        DialogResult ShowAddBusDialog(AddBusViewModel addBus);

        /// <summary>
        /// Shows the add/update dialog and gives it the view model.
        /// </summary>
        /// <param name="addBus">The view model for the dialog.</param>
        /// <returns>The result of the dialog. Can be Ok, Cancel or None.</returns>
        DialogResult ShowAddUpdateStationDialog(AddUpdateStationViewModel addStation);

        /// <summary>
        /// Shows the station selection dialog and gives it the view model.
        /// </summary>
        /// <param name="addBus">The view model for the dialog.</param>
        /// <returns>The result of the dialog. Can be Ok, Cancel or None.</returns>
        DialogResult ShowSelectStationsDialog(SelectStationsViewModel selectStations);

        /// <summary>
        /// Shows the station details dialog and gives it the view model.
        /// </summary>
        /// <param name="addBus">The view model for the dialog.</param>
        /// <returns>The result of the dialog. Can be Ok, Cancel or None.</returns>
        DialogResult ShowStationDetailsDialog(StationDetailsViewModel stationDetails);

        /// <summary>
        /// Shows the add/update bus line dialog and gives it the view model.
        /// </summary>
        /// <param name="addBus">The view model for the dialog.</param>
        /// <returns>The result of the dialog. Can be Ok, Cancel or None.</returns>
        DialogResult ShowAddUpdateBusLineDialog(AddUpdateBusLineViewModel addBusline);

        /// <summary>
        /// Shows a yes/no message box with a given title, message and a warning icon.
        /// </summary>
        /// <param name="message">The message of the message box</param>
        /// <param name="title">The title of the message box</param>
        /// <returns>The result of the message box. Can be Yes, No or None</returns>
        DialogResult ShowYesNoDialog(string message, string title);
    }
}
