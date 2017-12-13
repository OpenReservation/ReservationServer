 
using Autofac;
using DataAccess;
using Models;
using WeihanLi.Common;

namespace Business
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

    public class BusinessModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
                builder.RegisterType<BLLUser>().As<IBLLUser>();
                builder.RegisterType<BLLBlockType>().As<IBLLBlockType>();
                builder.RegisterType<BLLBlockEntity>().As<IBLLBlockEntity>();
                builder.RegisterType<BLLOperationLog>().As<IBLLOperationLog>();
                builder.RegisterType<BLLReservation>().As<IBLLReservation>();
                builder.RegisterType<BLLReservationPlace>().As<IBLLReservationPlace>();
                builder.RegisterType<BLLSystemSettings>().As<IBLLSystemSettings>();
                builder.RegisterType<BLLNotice>().As<IBLLNotice>();
                builder.RegisterType<BLLDisabledPeriod>().As<IBLLDisabledPeriod>();
        }
    }
}
