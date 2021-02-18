using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    /// <summary>
    /// Defines functions in order to run an action at the main thread.
    /// </summary>
    public interface IContext
    {
        /// <summary>
        /// Checks if already runs on the main thread.
        /// </summary>
        bool IsSynchronized { get; }
        /// <summary>
        /// Runs an action on the main thread synchronous.
        /// </summary>
        void Invoke(Action action);
        /// <summary>
        /// Runs an action on the main thread asynchronous.
        /// </summary>
        void BeginInvoke(Action action);
    }
}
