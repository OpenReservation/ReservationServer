using ActivityReservation.Models;

namespace ActivityReservation.Business
{
    public partial interface IBLLUser : IBaseBLL<User>
    {
        User Login(User u);
    }
}
