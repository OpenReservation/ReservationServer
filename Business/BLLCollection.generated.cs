 
using DataAccess;
using Models;
namespace Business
{
	public partial class BLLUser : BaseBLL<User>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALUser();
        }
    }
	public partial class BLLBlockType : BaseBLL<BlockType>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALBlockType();
        }
    }
	public partial class BLLBlockEntity : BaseBLL<BlockEntity>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALBlockEntity();
        }
    }
	public partial class BLLOperationLog : BaseBLL<OperationLog>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALOperationLog();
        }
    }
	public partial class BLLReservation : BaseBLL<Reservation>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALReservation();
        }
    }
	public partial class BLLReservationPlace : BaseBLL<ReservationPlace>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALReservationPlace();
        }
    }
	public partial class BLLSystemSettings : BaseBLL<SystemSettings>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALSystemSettings();
        }
    }
	public partial class BLLNotice : BaseBLL<Notice>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALNotice();
        }
    }
	public partial class BLLDisabledPeriod : BaseBLL<DisabledPeriod>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALDisabledPeriod();
        }
    }
}
