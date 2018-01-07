using Autofac;
using Business;
using System.Web.Mvc;

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

    public class BusinessHelperModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<BusinessHelper>().As<IBusinessHelper>().SingleInstance();
		}
	}        

    public class BusinessHelper : IBusinessHelper
    {
      
        /// <summary>
        /// UserHelper
        /// </summary>
	    public IBLLUser UserHelper
        => DependencyResolver.Current.GetService<IBLLUser>();

      
        /// <summary>
        /// BlockTypeHelper
        /// </summary>
	    public IBLLBlockType BlockTypeHelper
        => DependencyResolver.Current.GetService<IBLLBlockType>();

      
        /// <summary>
        /// BlockEntityHelper
        /// </summary>
	    public IBLLBlockEntity BlockEntityHelper
        => DependencyResolver.Current.GetService<IBLLBlockEntity>();

      
        /// <summary>
        /// OperationLogHelper
        /// </summary>
	    public IBLLOperationLog OperationLogHelper
        => DependencyResolver.Current.GetService<IBLLOperationLog>();

      
        /// <summary>
        /// ReservationHelper
        /// </summary>
	    public IBLLReservation ReservationHelper
        => DependencyResolver.Current.GetService<IBLLReservation>();

      
        /// <summary>
        /// ReservationPlaceHelper
        /// </summary>
	    public IBLLReservationPlace ReservationPlaceHelper
        => DependencyResolver.Current.GetService<IBLLReservationPlace>();

      
        /// <summary>
        /// ReservationPeriodHelper
        /// </summary>
	    public IBLLReservationPeriod ReservationPeriodHelper
        => DependencyResolver.Current.GetService<IBLLReservationPeriod>();

      
        /// <summary>
        /// SystemSettingsHelper
        /// </summary>
	    public IBLLSystemSettings SystemSettingsHelper
        => DependencyResolver.Current.GetService<IBLLSystemSettings>();

      
        /// <summary>
        /// NoticeHelper
        /// </summary>
	    public IBLLNotice NoticeHelper
        => DependencyResolver.Current.GetService<IBLLNotice>();

      
        /// <summary>
        /// DisabledPeriodHelper
        /// </summary>
	    public IBLLDisabledPeriod DisabledPeriodHelper
        => DependencyResolver.Current.GetService<IBLLDisabledPeriod>();

    }
}