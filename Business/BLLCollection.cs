using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public partial class BLLUser : BaseBLL<Models.User>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALUser();
        }
    }

    public partial class BLLBlockType : BaseBLL<Models.BlockType>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALBlockType();
        }
    }

    public partial class BLLBlockEntity : BaseBLL<Models.BlockEntity>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALBlockEntity();
        }
    }

    public partial class BLLOperationLog : BaseBLL<Models.OperationLog>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALOperationLog();
        }
    }

    public partial class BLLReservation : BaseBLL<Models.Reservation>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALReservation();
        }
    }

    public partial class BLLReservationPlace : BaseBLL<Models.ReservationPlace>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALReservationPlace();
        }
    }

    public partial class BLLSystemSettings : BaseBLL<Models.SystemSettings>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALSystemSettings();
        }
    }

    public partial class BLLNotice : BaseBLL<Models.Notice>
    {
        protected override void InitDbHandler()
        {
            dbHandler = new DALNotice();
        }
    }
}
