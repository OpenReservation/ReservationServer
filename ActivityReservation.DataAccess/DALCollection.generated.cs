 
using ActivityReservation.DataAccess;
using ActivityReservation.Models;
using Microsoft.Extensions.DependencyInjection;
using WeihanLi.Common;

namespace ActivityReservation.DataAccess
{
	public partial interface IDALUser: IBaseDAL<User> { }

	public partial class DALUser: BaseDAL<User>,IDALUser { }
	
	public partial interface IDALBlockType: IBaseDAL<BlockType> { }

	public partial class DALBlockType: BaseDAL<BlockType>,IDALBlockType { }
	
	public partial interface IDALBlockEntity: IBaseDAL<BlockEntity> { }

	public partial class DALBlockEntity: BaseDAL<BlockEntity>,IDALBlockEntity { }
	
	public partial interface IDALOperationLog: IBaseDAL<OperationLog> { }

	public partial class DALOperationLog: BaseDAL<OperationLog>,IDALOperationLog { }
	
	public partial interface IDALReservation: IBaseDAL<Reservation> { }

	public partial class DALReservation: BaseDAL<Reservation>,IDALReservation { }
	
	public partial interface IDALReservationPlace: IBaseDAL<ReservationPlace> { }

	public partial class DALReservationPlace: BaseDAL<ReservationPlace>,IDALReservationPlace { }
	
	public partial interface IDALReservationPeriod: IBaseDAL<ReservationPeriod> { }

	public partial class DALReservationPeriod: BaseDAL<ReservationPeriod>,IDALReservationPeriod { }
	
	public partial interface IDALSystemSettings: IBaseDAL<SystemSettings> { }

	public partial class DALSystemSettings: BaseDAL<SystemSettings>,IDALSystemSettings { }
	
	public partial interface IDALNotice: IBaseDAL<Notice> { }

	public partial class DALNotice: BaseDAL<Notice>,IDALNotice { }
	
	public partial interface IDALDisabledPeriod: IBaseDAL<DisabledPeriod> { }

	public partial class DALDisabledPeriod: BaseDAL<DisabledPeriod>,IDALDisabledPeriod { }
	

    
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
