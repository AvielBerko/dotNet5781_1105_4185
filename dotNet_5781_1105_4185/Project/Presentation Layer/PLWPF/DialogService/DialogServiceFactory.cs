using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public static class DialogServiceFactory
    {
        public static IDialogService GetDialogService(string type)
        {
            switch (type)
            {
                default:
                case "view":
                    return ViewDialogService.Instance;
            }
        }
    }
}
