using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DLAPI;
using DO;

namespace DL
{
    sealed class DLObject : IDL
    {
        // static ctor to ensure instance init is done just before first usage
        static DLObject() { }
        private DLObject() { }
        static readonly DLObject instance = new DLObject();
        public static DLObject Instance { get => instance; }

        public User GetUser(string name)
        {
            throw new NotImplementedException();
        }

        public void AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
