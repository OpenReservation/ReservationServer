 
using Models;
namespace DataAccess
{
	public partial class DALUser: BaseDaL<User> { }
	public partial class DALBlockType: BaseDaL<BlockType> { }
	public partial class DALBlockEntity: BaseDaL<BlockEntity> { }
	public partial class DALOperationLog: BaseDaL<OperationLog> { }
	public partial class DALReservation: BaseDaL<Reservation> { }
	public partial class DALReservationPlace: BaseDaL<ReservationPlace> { }
	public partial class DALSystemSettings: BaseDaL<SystemSettings> { }
	public partial class DALNotice: BaseDaL<Notice> { }
	public partial class DALDisabledPeriod: BaseDaL<DisabledPeriod> { }
}
