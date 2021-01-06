using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL
{
    public class BaseDialogViewModel : BaseViewModel
    {
        protected virtual void CloseDialog(object window, bool? result)
        {
            ((Window)window).DialogResult = result;
        }
    }
}
