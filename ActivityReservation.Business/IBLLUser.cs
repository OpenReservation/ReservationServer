using Models;

namespace Business
{
    public partial interface IBLLUser : IBaseBLL<User>
    {
        User Login(User u);
    }
}