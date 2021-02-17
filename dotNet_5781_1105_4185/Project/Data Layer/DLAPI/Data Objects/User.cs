using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// <br>Entity of a user using the system</br>
    /// <br>Saves the users' name password and role</br>
    /// </summary>
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }
    }
}
