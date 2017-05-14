using Business;
namespace ActivityReservation.Helpers
{
    public interface IBusinessHelper
    {
        BLLUser UserHelper { get; }
        BLLBlockType BlockTypeHelper { get; }
        BLLBlockEntity BlockEntityHelper { get; }
        BLLOperationLog OperationLogHelper { get; }
        BLLReservation ReservationHelper { get; }
        BLLReservationPlace ReservationPlaceHelper { get; }
        BLLSystemSettings SystemSettingsHelper { get; }
        BLLNotice NoticeHelper { get; }
        BLLDisabledPeriod DisabledPeriodHelper { get; }
        
    }

    public class BusinessHelper : IBusinessHelper
    {
        private BLLUser _UserHelper;        
        /// <summary>
        /// UserHelper
        /// </summary>
	    public BLLUser UserHelper
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

        private BLLBlockType _BlockTypeHelper;        
        /// <summary>
        /// BlockTypeHelper
        /// </summary>
	    public BLLBlockType BlockTypeHelper
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

        private BLLBlockEntity _BlockEntityHelper;        
        /// <summary>
        /// BlockEntityHelper
        /// </summary>
	    public BLLBlockEntity BlockEntityHelper
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

        private BLLOperationLog _OperationLogHelper;        
        /// <summary>
        /// OperationLogHelper
        /// </summary>
	    public BLLOperationLog OperationLogHelper
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

        private BLLReservation _ReservationHelper;        
        /// <summary>
        /// ReservationHelper
        /// </summary>
	    public BLLReservation ReservationHelper
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

        private BLLReservationPlace _ReservationPlaceHelper;        
        /// <summary>
        /// ReservationPlaceHelper
        /// </summary>
	    public BLLReservationPlace ReservationPlaceHelper
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

        private BLLSystemSettings _SystemSettingsHelper;        
        /// <summary>
        /// SystemSettingsHelper
        /// </summary>
	    public BLLSystemSettings SystemSettingsHelper
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

        private BLLNotice _NoticeHelper;        
        /// <summary>
        /// NoticeHelper
        /// </summary>
	    public BLLNotice NoticeHelper
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

        private BLLDisabledPeriod _DisabledPeriodHelper;        
        /// <summary>
        /// DisabledPeriodHelper
        /// </summary>
	    public BLLDisabledPeriod DisabledPeriodHelper
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