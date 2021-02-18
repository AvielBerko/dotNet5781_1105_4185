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

        protected async Task<object> BlWorkAsync(Func<IBL, object> work) => await Task.Run(() => work(bl));
        protected async Task BlWorkAsync(Action<IBL> work) => await Task.Run(() => work(bl));

        public static IContext Context { get; set; }
        public static IDialogService DialogService { get; set; }

        protected async Task Load(Func<Task> load)
        {
            IsLoading = true;
            await load();
            IsLoading = false;
        }

        protected async Task<object> Load(Func<Task<object>> load)
        {
            IsLoading = true;
            object result = await load();
            IsLoading = false;
            return result;
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

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
