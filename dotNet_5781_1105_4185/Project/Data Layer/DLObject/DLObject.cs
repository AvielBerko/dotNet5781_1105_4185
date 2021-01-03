using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DLAPI;
using DS;

namespace DL
{
    sealed class DLObject : IDL
    {
        static readonly Lazy<DLObject> lazy = new Lazy<DLObject>(() => new DLObject());
        public static DLObject Instance => lazy.Value;

        private DLObject() { }

        public DO.User GetUser(string name)
        {
            return DataSet.Users.Find(user => user.Name == name).Clone();
        }

        public void AddUser(DO.User user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(DO.User user)
        {
            throw new NotImplementedException();
        }
    }
}
