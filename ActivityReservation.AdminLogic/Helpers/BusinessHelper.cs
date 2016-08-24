using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityReservation.Helpers
{
    public interface IBusinessHelper
    {
        BLLUser UserHelper { get; }

        BLLOperationLog OperLogHelper { get; }

        BLLReservation ReservationHelper { get;  }

        BLLReservationPlace ReservationPlaceHelper { get; }

        BLLSystemSettings SettingsHelper { get;  }

        BLLBlockEntity BlockEntityHelper { get; }

        BLLBlockType BlockTypeHelper { get; }
    }

    public class BusinessHelper : IBusinessHelper
    {
        private BLLBlockEntity blockEntityHelper = null;
        private BLLBlockType blockTypeHelper = null;

        private BLLOperationLog operLogHelper = null;

        private BLLReservation reservationHelper = null;
        private BLLReservationPlace reservationPlaceHelper = null;

        private BLLSystemSettings settingsHelper = null;
        private BLLUser userHelper = null;

        public BLLBlockEntity BlockEntityHelper
        {
            get
            {
                if (blockEntityHelper == null)
                {
                    blockEntityHelper = new BLLBlockEntity();
                }
                return blockEntityHelper;
            }
        }

        public BLLBlockType BlockTypeHelper
        {
            get
            {
                if (blockTypeHelper == null)
                {
                    blockTypeHelper = new BLLBlockType();
                }
                return blockTypeHelper;
            }
        }

        public BLLOperationLog OperLogHelper
        {
            get
            {
                if (operLogHelper == null)
                {
                    operLogHelper = new BLLOperationLog();
                }
                return operLogHelper;
            }
        }

        public BLLReservation ReservationHelper
        {
            get
            {
                if (reservationHelper == null)
                {
                    reservationHelper = new BLLReservation();
                }
                return reservationHelper;
            }
        }

        public BLLReservationPlace ReservationPlaceHelper
        {
            get
            {
                if (reservationPlaceHelper == null)
                {
                    reservationPlaceHelper = new BLLReservationPlace();
                }
                return reservationPlaceHelper;
            }
        }

        public BLLSystemSettings SettingsHelper
        {
            get
            {
                if (settingsHelper == null)
                {
                    settingsHelper = new BLLSystemSettings();
                }
                return settingsHelper;
            }
        }

        public BLLUser UserHelper
        {
            get
            {
                if (userHelper == null)
                {
                    userHelper = new BLLUser();
                }
                return userHelper;
            }
        }
    }
}