 
using DataAccess;
using Models;
namespace Business
{
	public partial interface IBLLUser:IBaseBLL<User>{}

	public partial class BLLUser : BaseBLL<User>, IBaseBLL<User>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALUser();
        }
    }
	public partial interface IBLLBlockType:IBaseBLL<BlockType>{}

	public partial class BLLBlockType : BaseBLL<BlockType>, IBaseBLL<BlockType>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALBlockType();
        }
    }
	public partial interface IBLLBlockEntity:IBaseBLL<BlockEntity>{}

	public partial class BLLBlockEntity : BaseBLL<BlockEntity>, IBaseBLL<BlockEntity>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALBlockEntity();
        }
    }
	public partial interface IBLLOperationLog:IBaseBLL<OperationLog>{}

	public partial class BLLOperationLog : BaseBLL<OperationLog>, IBaseBLL<OperationLog>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALOperationLog();
        }
    }
	public partial interface IBLLReservation:IBaseBLL<Reservation>{}

	public partial class BLLReservation : BaseBLL<Reservation>, IBaseBLL<Reservation>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALReservation();
        }
    }
	public partial interface IBLLReservationPlace:IBaseBLL<ReservationPlace>{}

	public partial class BLLReservationPlace : BaseBLL<ReservationPlace>, IBaseBLL<ReservationPlace>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALReservationPlace();
        }
    }
	public partial interface IBLLSystemSettings:IBaseBLL<SystemSettings>{}

	public partial class BLLSystemSettings : BaseBLL<SystemSettings>, IBaseBLL<SystemSettings>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALSystemSettings();
        }
    }
	public partial interface IBLLNotice:IBaseBLL<Notice>{}

	public partial class BLLNotice : BaseBLL<Notice>, IBaseBLL<Notice>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALNotice();
        }
    }
	public partial interface IBLLDisabledPeriod:IBaseBLL<DisabledPeriod>{}

	public partial class BLLDisabledPeriod : BaseBLL<DisabledPeriod>, IBaseBLL<DisabledPeriod>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALDisabledPeriod();
        }
    }
}
