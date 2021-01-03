using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLAPI
{
    public interface IDL
    {
        #region User
        DO.User GetUser(string name);
        void AddUser(DO.User user);
        void DeleteUser(DO.User user);
        #endregion
    }
}
