using ActivityReservation.Models;
using WeihanLi.Common.Helpers;

namespace ActivityReservation.Business
{
    public partial class BLLUser
    {
        /// <summary>
        /// login
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public User Login(User u)
        {
            u.UserPassword = SecurityHelper.SHA256(u.UserPassword);
            var user = Fetch(m => m.UserName.Equals(u.UserName) && m.UserPassword.Equals(u.UserPassword));
            return user;
        }
    }
}
