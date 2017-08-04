using Business;
namespace ActivityReservation.Helpers
{
    public interface IBusinessHelper
    {
        IBLLUser UserHelper { get; }
        IBLLBlockType BlockTypeHelper { get; }
        IBLLBlockEntity BlockEntityHelper { get; }
        IBLLOperationLog OperationLogHelper { get; }
        IBLLReservation ReservationHelper { get; }
        IBLLReservationPlace ReservationPlaceHelper { get; }
        IBLLSystemSettings SystemSettingsHelper { get; }
        IBLLNotice NoticeHelper { get; }
        IBLLDisabledPeriod DisabledPeriodHelper { get; }
        
    }

    public class BusinessHelper : IBusinessHelper
    {
        private IBLLUser _UserHelper;        
        /// <summary>
        /// UserHelper
        /// </summary>
	    public IBLLUser UserHelper
        {
            get
            {
                if(_UserHelper == null)
                {
                    _UserHelper = new BLLUser();
                }
                return _UserHelper;
            }
        }

        private IBLLBlockType _BlockTypeHelper;        
        /// <summary>
        /// BlockTypeHelper
        /// </summary>
	    public IBLLBlockType BlockTypeHelper
        {
            get
            {
                if(_BlockTypeHelper == null)
                {
                    _BlockTypeHelper = new BLLBlockType();
                }
                return _BlockTypeHelper;
            }
        }

        private IBLLBlockEntity _BlockEntityHelper;        
        /// <summary>
        /// BlockEntityHelper
        /// </summary>
	    public IBLLBlockEntity BlockEntityHelper
        {
            get
            {
                if(_BlockEntityHelper == null)
                {
                    _BlockEntityHelper = new BLLBlockEntity();
                }
                return _BlockEntityHelper;
            }
        }

        private IBLLOperationLog _OperationLogHelper;        
        /// <summary>
        /// OperationLogHelper
        /// </summary>
	    public IBLLOperationLog OperationLogHelper
        {
            get
            {
                if(_OperationLogHelper == null)
                {
                    _OperationLogHelper = new BLLOperationLog();
                }
                return _OperationLogHelper;
            }
        }

        private IBLLReservation _ReservationHelper;        
        /// <summary>
        /// ReservationHelper
        /// </summary>
	    public IBLLReservation ReservationHelper
        {
            get
            {
                if(_ReservationHelper == null)
                {
                    _ReservationHelper = new BLLReservation();
                }
                return _ReservationHelper;
            }
        }

        private IBLLReservationPlace _ReservationPlaceHelper;        
        /// <summary>
        /// ReservationPlaceHelper
        /// </summary>
	    public IBLLReservationPlace ReservationPlaceHelper
        {
            get
            {
                if(_ReservationPlaceHelper == null)
                {
                    _ReservationPlaceHelper = new BLLReservationPlace();
                }
                return _ReservationPlaceHelper;
            }
        }

        private IBLLSystemSettings _SystemSettingsHelper;        
        /// <summary>
        /// SystemSettingsHelper
        /// </summary>
	    public IBLLSystemSettings SystemSettingsHelper
        {
            get
            {
                if(_SystemSettingsHelper == null)
                {
                    _SystemSettingsHelper = new BLLSystemSettings();
                }
                return _SystemSettingsHelper;
            }
        }

        private IBLLNotice _NoticeHelper;        
        /// <summary>
        /// NoticeHelper
        /// </summary>
	    public IBLLNotice NoticeHelper
        {
            get
            {
                if(_NoticeHelper == null)
                {
                    _NoticeHelper = new BLLNotice();
                }
                return _NoticeHelper;
            }
        }

        private IBLLDisabledPeriod _DisabledPeriodHelper;        
        /// <summary>
        /// DisabledPeriodHelper
        /// </summary>
	    public IBLLDisabledPeriod DisabledPeriodHelper
        {
            get
            {
                if(_DisabledPeriodHelper == null)
                {
                    _DisabledPeriodHelper = new BLLDisabledPeriod();
                }
                return _DisabledPeriodHelper;
            }
        }

    }
}