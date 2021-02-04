using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using BLAPI;

namespace PL
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        readonly IBL bl = BLFactory.GetBL("1");

        protected object BlWork(Func<IBL, object> work) => work(bl);
        protected void BlWork(Action<IBL> work) => work(bl);

        public static string DialogServiceType { get; set; } = "view";
        protected IDialogService DialogService => DialogServiceFactory.GetDialogService(DialogServiceType);

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [Conditional("Debug")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real, 
            // public, instance property on this object. 
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                Debug.Fail($"Invalid property name: {propertyName}");
            }
        }
    }
}
