 
using ActivityReservation.Database;
using ActivityReservation.Models;
using Microsoft.Extensions.DependencyInjection;
using WeihanLi.Common;

namespace ActivityReservation.Business
{
	public partial interface IBLLUser:IBaseBLL<User>{}

	public partial class BLLUser : BaseBLL<User>,  IBLLUser
    {
        public BLLUser(ReservationDbContext dbContext) : base(dbContext)
        {
        }
    }
	public partial interface IBLLBlockType:IBaseBLL<BlockType>{}

	public partial class BLLBlockType : BaseBLL<BlockType>,  IBLLBlockType
    {
        public BLLBlockType(ReservationDbContext dbContext) : base(dbContext)
        {
        }
    }
	public partial interface IBLLBlockEntity:IBaseBLL<BlockEntity>{}

	public partial class BLLBlockEntity : BaseBLL<BlockEntity>,  IBLLBlockEntity
    {
        public BLLBlockEntity(ReservationDbContext dbContext) : base(dbContext)
        {
        }
    }
	public partial interface IBLLOperationLog:IBaseBLL<OperationLog>{}

	public partial class BLLOperationLog : BaseBLL<OperationLog>,  IBLLOperationLog
    {
        public BLLOperationLog(ReservationDbContext dbContext) : base(dbContext)
        {
        }
    }
	public partial interface IBLLReservation:IBaseBLL<Reservation>{}

	public partial class BLLReservation : BaseBLL<Reservation>,  IBLLReservation
    {
        public BLLReservation(ReservationDbContext dbContext) : base(dbContext)
        {
        }
    }
	public partial interface IBLLReservationPlace:IBaseBLL<ReservationPlace>{}

	public partial class BLLReservationPlace : BaseBLL<ReservationPlace>,  IBLLReservationPlace
    {
        public BLLReservationPlace(ReservationDbContext dbContext) : base(dbContext)
        {
        }
    }
	public partial interface IBLLReservationPeriod:IBaseBLL<ReservationPeriod>{}

	public partial class BLLReservationPeriod : BaseBLL<ReservationPeriod>,  IBLLReservationPeriod
    {
        public BLLReservationPeriod(ReservationDbContext dbContext) : base(dbContext)
        {
        }
    }
	public partial interface IBLLSystemSettings:IBaseBLL<SystemSettings>{}

	public partial class BLLSystemSettings : BaseBLL<SystemSettings>,  IBLLSystemSettings
    {
        public BLLSystemSettings(ReservationDbContext dbContext) : base(dbContext)
        {
        }
    }
	public partial interface IBLLNotice:IBaseBLL<Notice>{}

	public partial class BLLNotice : BaseBLL<Notice>,  IBLLNotice
    {
        public BLLNotice(ReservationDbContext dbContext) : base(dbContext)
        {
        }
    }
	public partial interface IBLLDisabledPeriod:IBaseBLL<DisabledPeriod>{}

	public partial class BLLDisabledPeriod : BaseBLL<DisabledPeriod>,  IBLLDisabledPeriod
    {
        public BLLDisabledPeriod(ReservationDbContext dbContext) : base(dbContext)
        {
        }
    }

    public static class BusinessExtensions
    {
        public static IServiceCollection AddBLL(this IServiceCollection services)
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
            return services;
        }
    }
}
