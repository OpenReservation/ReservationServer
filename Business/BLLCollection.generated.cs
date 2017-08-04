 
using DataAccess;
using Models;
namespace Business
{
	public partial interface IBLLUser:IBaseBLL<User>{}

	public partial class BLLUser : BaseBLL<User>,  IBLLUser
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALUser();
        }
    }
	public partial interface IBLLBlockType:IBaseBLL<BlockType>{}

	public partial class BLLBlockType : BaseBLL<BlockType>,  IBLLBlockType
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALBlockType();
        }
    }
	public partial interface IBLLBlockEntity:IBaseBLL<BlockEntity>{}

	public partial class BLLBlockEntity : BaseBLL<BlockEntity>,  IBLLBlockEntity
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALBlockEntity();
        }
    }
	public partial interface IBLLOperationLog:IBaseBLL<OperationLog>{}

	public partial class BLLOperationLog : BaseBLL<OperationLog>,  IBLLOperationLog
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALOperationLog();
        }
    }
	public partial interface IBLLReservation:IBaseBLL<Reservation>{}

	public partial class BLLReservation : BaseBLL<Reservation>,  IBLLReservation
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALReservation();
        }
    }
	public partial interface IBLLReservationPlace:IBaseBLL<ReservationPlace>{}

	public partial class BLLReservationPlace : BaseBLL<ReservationPlace>,  IBLLReservationPlace
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALReservationPlace();
        }
    }
	public partial interface IBLLSystemSettings:IBaseBLL<SystemSettings>{}

	public partial class BLLSystemSettings : BaseBLL<SystemSettings>,  IBLLSystemSettings
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALSystemSettings();
        }
    }
	public partial interface IBLLNotice:IBaseBLL<Notice>{}

	public partial class BLLNotice : BaseBLL<Notice>,  IBLLNotice
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALNotice();
        }
    }
	public partial interface IBLLDisabledPeriod:IBaseBLL<DisabledPeriod>{}

	public partial class BLLDisabledPeriod : BaseBLL<DisabledPeriod>,  IBLLDisabledPeriod
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALDisabledPeriod();
        }
    }
}
