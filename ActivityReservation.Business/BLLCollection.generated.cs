 
using ActivityReservation.Database;
using ActivityReservation.Models;
using Microsoft.Extensions.DependencyInjection;
using WeihanLi.EntityFramework;

namespace ActivityReservation.Business
{
	public partial interface IBLLUser: IEFRepository<ReservationDbContext, User>{}

	public partial class BLLUser : EFRepository<ReservationDbContext, User>,  IBLLUser
    {
        public BLLUser(ReservationDbContext dbContext) : base(dbContext)
        {
        }
    }
	public partial interface IBLLBlockType: IEFRepository<ReservationDbContext, BlockType>{}

	public partial class BLLBlockType : EFRepository<ReservationDbContext, BlockType>,  IBLLBlockType
    {
        public BLLBlockType(ReservationDbContext dbContext) : base(dbContext)
        {
        }
    }
	public partial interface IBLLBlockEntity: IEFRepository<ReservationDbContext, BlockEntity>{}

	public partial class BLLBlockEntity : EFRepository<ReservationDbContext, BlockEntity>,  IBLLBlockEntity
    {
        public BLLBlockEntity(ReservationDbContext dbContext) : base(dbContext)
        {
        }
    }
	public partial interface IBLLOperationLog: IEFRepository<ReservationDbContext, OperationLog>{}

	public partial class BLLOperationLog : EFRepository<ReservationDbContext, OperationLog>,  IBLLOperationLog
    {
        public BLLOperationLog(ReservationDbContext dbContext) : base(dbContext)
        {
        }
    }
	public partial interface IBLLReservation: IEFRepository<ReservationDbContext, Reservation>{}

	public partial class BLLReservation : EFRepository<ReservationDbContext, Reservation>,  IBLLReservation
    {
        public BLLReservation(ReservationDbContext dbContext) : base(dbContext)
        {
        }
    }
	public partial interface IBLLReservationPlace: IEFRepository<ReservationDbContext, ReservationPlace>{}

	public partial class BLLReservationPlace : EFRepository<ReservationDbContext, ReservationPlace>,  IBLLReservationPlace
    {
        public BLLReservationPlace(ReservationDbContext dbContext) : base(dbContext)
        {
        }
    }
	public partial interface IBLLReservationPeriod: IEFRepository<ReservationDbContext, ReservationPeriod>{}

	public partial class BLLReservationPeriod : EFRepository<ReservationDbContext, ReservationPeriod>,  IBLLReservationPeriod
    {
        public BLLReservationPeriod(ReservationDbContext dbContext) : base(dbContext)
        {
        }
    }
	public partial interface IBLLSystemSettings: IEFRepository<ReservationDbContext, SystemSettings>{}

	public partial class BLLSystemSettings : EFRepository<ReservationDbContext, SystemSettings>,  IBLLSystemSettings
    {
        public BLLSystemSettings(ReservationDbContext dbContext) : base(dbContext)
        {
        }
    }
	public partial interface IBLLNotice: IEFRepository<ReservationDbContext, Notice>{}

	public partial class BLLNotice : EFRepository<ReservationDbContext, Notice>,  IBLLNotice
    {
        public BLLNotice(ReservationDbContext dbContext) : base(dbContext)
        {
        }
    }
	public partial interface IBLLDisabledPeriod: IEFRepository<ReservationDbContext, DisabledPeriod>{}

	public partial class BLLDisabledPeriod : EFRepository<ReservationDbContext, DisabledPeriod>,  IBLLDisabledPeriod
    {
        public BLLDisabledPeriod(ReservationDbContext dbContext) : base(dbContext)
        {
        }
    }
	public partial interface IBLLWechatMenuConfig: IEFRepository<ReservationDbContext, WechatMenuConfig>{}

	public partial class BLLWechatMenuConfig : EFRepository<ReservationDbContext, WechatMenuConfig>,  IBLLWechatMenuConfig
    {
        public BLLWechatMenuConfig(ReservationDbContext dbContext) : base(dbContext)
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
                services.AddScoped<IBLLWechatMenuConfig, BLLWechatMenuConfig>();
            return services;
        }
    }
}
