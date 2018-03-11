using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using WeihanLi.Common.Helpers;

namespace Models
{
    internal class ReservationLogFormatter : DatabaseLogFormatter
    {
        private static readonly LogHelper Logger = LogHelper.GetLogHelper<ReservationLogFormatter>();

        public ReservationLogFormatter(Action<string> writeAction) : base(writeAction)
        {
        }

        public override void LogCommand<TResult>(DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            base.LogCommand(command, interceptionContext);
        }

        public override void LogParameter<TResult>(DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext, DbParameter parameter)
        {
            base.LogParameter(command, interceptionContext, parameter);
        }

        public override void LogResult<TResult>(DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            base.LogResult(command, interceptionContext);
        }

        public ReservationLogFormatter(DbContext context, Action<string> writeAction) : base(context, writeAction)
        {
        }
    }

    internal class ReservationDbConfiguration : DbConfiguration
    {
        internal ReservationDbConfiguration()
        => SetDatabaseLogFormatter(
                (context, writeAction) => new ReservationLogFormatter(context, writeAction));
    }
}