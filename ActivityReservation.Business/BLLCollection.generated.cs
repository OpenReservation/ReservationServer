 
using IServiceCollection;
using ActivityReservation.DataAccess;
using ActivityReservation.Models;
using WeihanLi.Common;

namespace ActivityReservation.Business
{
	public partial interface IBLLUser:IBaseBLL<User>{}

	public partial class BLLUser : BaseBLL<User>,  IBLLUser
    {
        protected override void InitDbHandler()
        {
            dbHandler = DependencyResolver.Current.GetService<IDALUser>();
        }
    }
	public partial interface IBLLBlockType:IBaseBLL<BlockType>{}

	public partial class BLLBlockType : BaseBLL<BlockType>,  IBLLBlockType
    {
        protected override void InitDbHandler()
        {
            dbHandler = DependencyResolver.Current.GetService<IDALBlockType>();
        }
    }
	public partial interface IBLLBlockEntity:IBaseBLL<BlockEntity>{}

	public partial class BLLBlockEntity : BaseBLL<BlockEntity>,  IBLLBlockEntity
    {
        protected override void InitDbHandler()
        {
            dbHandler = DependencyResolver.Current.GetService<IDALBlockEntity>();
        }
    }
	public partial interface IBLLOperationLog:IBaseBLL<OperationLog>{}

	public partial class BLLOperationLog : BaseBLL<OperationLog>,  IBLLOperationLog
    {
        protected override void InitDbHandler()
        {
            dbHandler = DependencyResolver.Current.GetService<IDALOperationLog>();
        }
    }
	public partial interface IBLLReservation:IBaseBLL<Reservation>{}

	public partial class BLLReservation : BaseBLL<Reservation>,  IBLLReservation
    {
        protected override void InitDbHandler()
        {
            dbHandler = DependencyResolver.Current.GetService<IDALReservation>();
        }
    }
	public partial interface IBLLReservationPlace:IBaseBLL<ReservationPlace>{}

	public partial class BLLReservationPlace : BaseBLL<ReservationPlace>,  IBLLReservationPlace
    {
        protected override void InitDbHandler()
        {
            dbHandler = DependencyResolver.Current.GetService<IDALReservationPlace>();
        }
    }
	public partial interface IBLLReservationPeriod:IBaseBLL<ReservationPeriod>{}

	public partial class BLLReservationPeriod : BaseBLL<ReservationPeriod>,  IBLLReservationPeriod
    {
        protected override void InitDbHandler()
        {
            dbHandler = DependencyResolver.Current.GetService<IDALReservationPeriod>();
        }
    }
	public partial interface IBLLSystemSettings:IBaseBLL<SystemSettings>{}

	public partial class BLLSystemSettings : BaseBLL<SystemSettings>,  IBLLSystemSettings
    {
        protected override void InitDbHandler()
        {
            dbHandler = DependencyResolver.Current.GetService<IDALSystemSettings>();
        }
    }
	public partial interface IBLLNotice:IBaseBLL<Notice>{}

	public partial class BLLNotice : BaseBLL<Notice>,  IBLLNotice
    {
        protected override void InitDbHandler()
        {
            dbHandler = DependencyResolver.Current.GetService<IDALNotice>();
        }
    }
	public partial interface IBLLDisabledPeriod:IBaseBLL<DisabledPeriod>{}

	public partial class BLLDisabledPeriod : BaseBLL<DisabledPeriod>,  IBLLDisabledPeriod
    {
        protected override void InitDbHandler()
        {
            dbHandler = DependencyResolver.Current.GetService<IDALDisabledPeriod>();
        }
    }

    public static class BusinessExtensions
    {
        public static IServiceCollection AddBLL(this IServiceClollection services)
        {
                services.AddScoped<IBLLUser, BLLUser>();
                services.AddScoped<IBLLBlockType, BLLBlockType>();
                services.AddScoped<IBLLBlockEntity, BLLBlockEntity>();
                services.AddScoped<IBLLOperationLog, BLLOperationLog>();
                services.AddScoped<IBLLReservation, BLLReservation>();
                services.AddScoped<IBLLReservationPlace, BLLReservationPlace>();
                services.AddScoped<IBLLReservationPeriod, BLLReservationPeriod>();
                services.AddScoped<IBLLSystemSettings, BLLSystemSettings>();
                services.AddScoped<IBLLNotice, BLLNotice>();
                services.AddScoped<IBLLDisabledPeriod, BLLDisabledPeriod>();
        }
    }
}
