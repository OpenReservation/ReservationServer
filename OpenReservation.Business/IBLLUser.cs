using OpenReservation.Models;

namespace OpenReservation.Business
{
    public partial interface IBLLUser
    {
        User Login(User u);
    }
}
