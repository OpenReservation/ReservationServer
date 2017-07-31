 
using Models;
namespace DataAccess
{
	public partial interface IDALUser: IBaseDAL<User> { }

	public partial class DALUser: BaseDAL<User>,IDALUser { }
	
	public partial interface IDALBlockType: IBaseDAL<BlockType> { }

	public partial class DALBlockType: BaseDAL<BlockType>,IDALBlockType { }
	
	public partial interface IDALBlockEntity: IBaseDAL<BlockEntity> { }

	public partial class DALBlockEntity: BaseDAL<BlockEntity>,IDALBlockEntity { }
	
	public partial interface IDALOperationLog: IBaseDAL<OperationLog> { }

	public partial class DALOperationLog: BaseDAL<OperationLog>,IDALOperationLog { }
	
	public partial interface IDALReservation: IBaseDAL<Reservation> { }

	public partial class DALReservation: BaseDAL<Reservation>,IDALReservation { }
	
	public partial interface IDALReservationPlace: IBaseDAL<ReservationPlace> { }

	public partial class DALReservationPlace: BaseDAL<ReservationPlace>,IDALReservationPlace { }
	
	public partial interface IDALSystemSettings: IBaseDAL<SystemSettings> { }

	public partial class DALSystemSettings: BaseDAL<SystemSettings>,IDALSystemSettings { }
	
	public partial interface IDALNotice: IBaseDAL<Notice> { }

	public partial class DALNotice: BaseDAL<Notice>,IDALNotice { }
	
	public partial interface IDALDisabledPeriod: IBaseDAL<DisabledPeriod> { }

	public partial class DALDisabledPeriod: BaseDAL<DisabledPeriod>,IDALDisabledPeriod { }
	
}
