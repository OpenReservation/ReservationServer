namespace Business
{
    public partial interface IBLLUser : IBaseBLL<Models.User>
    {
        Models.User Login(Models.User u);
    }
}