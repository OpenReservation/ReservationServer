namespace OpenReservation.Common;

public enum DbType
{
    SqlServer = 0,
    InMemory = 1,
    Sqlite = 2,
    [Obsolete("Do not support MySql officially Since 4.0, use Pgsql instead", true)]
    MySql = 3,
    Npgsql = 4,
}
