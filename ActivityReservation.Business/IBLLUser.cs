using ActivityReservation.Models;

namespace ActivityReservation.Business
{
    public partial interface IBLLUser
    {
        User Login(User u);
    }
}
