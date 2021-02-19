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
    /// <summary>
    /// The base view model every view model will inherit.
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        private readonly IBL bl = BLFactory.GetBL("1");

        /// <summary>
        /// Synchronous calls to the bussiness layer.
        /// </summary>
        /// <param name="work">The call to the bussiness layer.</param>
        /// <returns>The value that the bussiness layer returned.</returns>
        protected object BlWork(Func<IBL, object> work) => work(bl);
        /// <summary>
        /// Synchronous calls to the bussiness layer.
        /// </summary>
        /// <param name="work">The call to the bussiness layer.</param>
        protected void BlWork(Action<IBL> work) => work(bl);

        /// <summary>
        /// Asynchronous calls the the bussiness layer.
        /// </summary>
        /// <param name="work">The call to the bussiness layer.</param>
        /// <returns>A task that will contain the value that the bussiness layer returned.</returns>
        protected async Task<object> BlWorkAsync(Func<IBL, object> work) => await Task.Run(() => work(bl));
        /// <summary>
        /// Asynchronous calls the the bussiness layer.
        /// </summary>
        /// <param name="work">The call to the bussiness layer.</param>
        protected async Task BlWorkAsync(Action<IBL> work) => await Task.Run(() => work(bl));

        /// <summary>
        /// Context used to run calls through the dispacher.
        /// </summary>
        public static IContext Context { get; set; }
        /// <summary>
        /// A service to run dialog operations.
        /// </summary>
        public static IDialogService DialogService { get; set; }

        /// <summary>
        /// Used in order to set the state for loading while doing an asynchronous task.
        /// </summary>
        /// <param name="load">Asynchronous task</param>
        protected async Task Load(Func<Task> load)
        {
            IsLoading = true;
            await load();
            IsLoading = false;
        }

        /// <summary>
        /// Used in order to set the state for loading while doing an asynchronous task that returns an object.
        /// </summary>
        /// <param name="load">Asynchronous task</param>
        /// <returns>A task that contains the object that the asynchronous task will return.</returns>
        protected async Task<object> Load(Func<Task<object>> load)
        {
            IsLoading = true;
            object result = await load();
            IsLoading = false;

            return result;
        }

        /// <summary>
        /// Indicates that the view model is in middle of an asynchronous task. <br />
        /// It can be used to show a loading indicator.
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
        private bool _isLoading;

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
