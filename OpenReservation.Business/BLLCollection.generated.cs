using OpenReservation.Database;
using OpenReservation.Models;
using WeihanLi.EntityFramework;

namespace OpenReservation.Business;

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
