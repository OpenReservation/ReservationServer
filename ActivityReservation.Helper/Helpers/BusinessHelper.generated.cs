using ActivityReservation.Business;
using WeihanLi.Common;

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
        IBLLReservationPeriod ReservationPeriodHelper { get; }
        IBLLSystemSettings SystemSettingsHelper { get; }
        IBLLNotice NoticeHelper { get; }
        IBLLDisabledPeriod DisabledPeriodHelper { get; }
        
    }

    public class BusinessHelper : IBusinessHelper
    {
      
        /// <summary>
        /// UserHelper
        /// </summary>
	    public IBLLUser UserHelper
        => DependencyResolver.Current.ResolveService<IBLLUser>();

      
        /// <summary>
        /// BlockTypeHelper
        /// </summary>
	    public IBLLBlockType BlockTypeHelper
        => DependencyResolver.Current.ResolveService<IBLLBlockType>();

      
        /// <summary>
        /// BlockEntityHelper
        /// </summary>
	    public IBLLBlockEntity BlockEntityHelper
        => DependencyResolver.Current.ResolveService<IBLLBlockEntity>();

      
        /// <summary>
        /// OperationLogHelper
        /// </summary>
	    public IBLLOperationLog OperationLogHelper
        => DependencyResolver.Current.ResolveService<IBLLOperationLog>();

      
        /// <summary>
        /// ReservationHelper
        /// </summary>
	    public IBLLReservation ReservationHelper
        => DependencyResolver.Current.ResolveService<IBLLReservation>();

      
        /// <summary>
        /// ReservationPlaceHelper
        /// </summary>
	    public IBLLReservationPlace ReservationPlaceHelper
        => DependencyResolver.Current.ResolveService<IBLLReservationPlace>();

      
        /// <summary>
        /// ReservationPeriodHelper
        /// </summary>
	    public IBLLReservationPeriod ReservationPeriodHelper
        => DependencyResolver.Current.ResolveService<IBLLReservationPeriod>();

      
        /// <summary>
        /// SystemSettingsHelper
        /// </summary>
	    public IBLLSystemSettings SystemSettingsHelper
        => DependencyResolver.Current.ResolveService<IBLLSystemSettings>();

      
        /// <summary>
        /// NoticeHelper
        /// </summary>
	    public IBLLNotice NoticeHelper
        => DependencyResolver.Current.ResolveService<IBLLNotice>();

      
        /// <summary>
        /// DisabledPeriodHelper
        /// </summary>
	    public IBLLDisabledPeriod DisabledPeriodHelper
        => DependencyResolver.Current.ResolveService<IBLLDisabledPeriod>();

    }
}