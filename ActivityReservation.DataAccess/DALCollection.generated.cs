 
using ActivityReservation.DataAccess;
using ActivityReservation.Models;
using Microsoft.Extensions.DependencyInjection;
using WeihanLi.Common;

namespace ActivityReservation.DataAccess
{
	public partial interface IDALUser: IBaseDAL<User> { }

	public partial class DALUser: BaseDAL<User>,IDALUser { 

        public DALUser(ReservationDbContext dbContext):base(dbContext)
        {
        }
}
	
	public partial interface IDALBlockType: IBaseDAL<BlockType> { }

	public partial class DALBlockType: BaseDAL<BlockType>,IDALBlockType { 

        public DALBlockType(ReservationDbContext dbContext):base(dbContext)
        {
        }
}
	
	public partial interface IDALBlockEntity: IBaseDAL<BlockEntity> { }

	public partial class DALBlockEntity: BaseDAL<BlockEntity>,IDALBlockEntity { 

        public DALBlockEntity(ReservationDbContext dbContext):base(dbContext)
        {
        }
}
	
	public partial interface IDALOperationLog: IBaseDAL<OperationLog> { }

	public partial class DALOperationLog: BaseDAL<OperationLog>,IDALOperationLog { 

        public DALOperationLog(ReservationDbContext dbContext):base(dbContext)
        {
        }
}
	
	public partial interface IDALReservation: IBaseDAL<Reservation> { }

	public partial class DALReservation: BaseDAL<Reservation>,IDALReservation { 

        public DALReservation(ReservationDbContext dbContext):base(dbContext)
        {
        }
}
	
	public partial interface IDALReservationPlace: IBaseDAL<ReservationPlace> { }

	public partial class DALReservationPlace: BaseDAL<ReservationPlace>,IDALReservationPlace { 

        public DALReservationPlace(ReservationDbContext dbContext):base(dbContext)
        {
        }
}
	
	public partial interface IDALReservationPeriod: IBaseDAL<ReservationPeriod> { }

	public partial class DALReservationPeriod: BaseDAL<ReservationPeriod>,IDALReservationPeriod { 

        public DALReservationPeriod(ReservationDbContext dbContext):base(dbContext)
        {
        }
}
	
	public partial interface IDALSystemSettings: IBaseDAL<SystemSettings> { }

	public partial class DALSystemSettings: BaseDAL<SystemSettings>,IDALSystemSettings { 

        public DALSystemSettings(ReservationDbContext dbContext):base(dbContext)
        {
        }
}
	
	public partial interface IDALNotice: IBaseDAL<Notice> { }

	public partial class DALNotice: BaseDAL<Notice>,IDALNotice { 

        public DALNotice(ReservationDbContext dbContext):base(dbContext)
        {
        }
}
	
	public partial interface IDALDisabledPeriod: IBaseDAL<DisabledPeriod> { }

	public partial class DALDisabledPeriod: BaseDAL<DisabledPeriod>,IDALDisabledPeriod { 

        public DALDisabledPeriod(ReservationDbContext dbContext):base(dbContext)
        {
        }
}
	

    
    public static class DataAccessExtensions
    {
        public static IServiceCollection AddDAL(this IServiceCollection services)
        {
                services.AddScoped<IDALUser, DALUser>();
                services.AddScoped<IDALBlockType, DALBlockType>();
                services.AddScoped<IDALBlockEntity, DALBlockEntity>();
                services.AddScoped<IDALOperationLog, DALOperationLog>();
                services.AddScoped<IDALReservation, DALReservation>();
                services.AddScoped<IDALReservationPlace, DALReservationPlace>();
                services.AddScoped<IDALReservationPeriod, DALReservationPeriod>();
                services.AddScoped<IDALSystemSettings, DALSystemSettings>();
                services.AddScoped<IDALNotice, DALNotice>();
                services.AddScoped<IDALDisabledPeriod, DALDisabledPeriod>();
            return services;
        }
    }

}
