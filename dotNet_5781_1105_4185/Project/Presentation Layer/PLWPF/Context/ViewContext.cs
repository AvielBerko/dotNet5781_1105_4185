using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PL
{
    /// <summary>
    /// Implementation of the IContext with a given dispatcher. <br />
    /// This is used when running the view in order to run an action on the UI thread.
    /// </summary>
    public sealed class ViewContext : IContext
    {
        /// <summary>
        /// The main thread dispatcher.
        /// </summary>
        private readonly Dispatcher _dispatcher;

        public bool IsSynchronized => _dispatcher.Thread == Thread.CurrentThread;

        public ViewContext() : this(Dispatcher.CurrentDispatcher)
        {
        }

        public ViewContext(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void Invoke(Action action)
        {
            _dispatcher.Invoke(action);
        }

        public void BeginInvoke(Action action)
        {
            _dispatcher.BeginInvoke(action);
        }
    }
}
