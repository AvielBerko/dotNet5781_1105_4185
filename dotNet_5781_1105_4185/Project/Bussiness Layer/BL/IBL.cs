using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLAPI
{
    public interface IBL
    {
        BO.User UserAuthentication(string name, string password);
        BO.User UserSignUp(string name, string password);
        void ValidatePassword(string password);
    }
}
