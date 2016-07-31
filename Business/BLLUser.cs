using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public partial class BLLUser
    {
        /// <summary>
        /// login
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public Models.User Login(Models.User u)
        {
            u.UserPassword = Common.SecurityHelper.SHA256_Encrypt(u.UserPassword);
            Models.User user = dbHandler.GetOne(m => m.UserName.Equals(u.UserName) && m.UserPassword.Equals(u.UserPassword));
            return user;
        }
    }
}
