namespace DataAccess
{
    public partial class DALUser : BaseDaL<Models.User> { }

    public partial class DALBlockType : BaseDaL<Models.BlockType> { }

    public partial class DALBlockEntity : BaseDaL<Models.BlockEntity> { }

    public partial class DALOperationLog : BaseDaL<Models.OperationLog> { }

    public partial class DALReservation : BaseDaL<Models.Reservation> { }

    public partial class DALReservationPlace : BaseDaL<Models.ReservationPlace> { }

    public partial class DALSystemSettings : BaseDaL<Models.SystemSettings> { }
}