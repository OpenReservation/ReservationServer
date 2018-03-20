using System;
using Exceptionless;
using WeihanLi.Common.Log;

namespace ActivityReservation.Common
{
    public class ExceptionlessLogProvider : ILogHelperProvider
    {
        static ExceptionlessLogProvider()
        {
            ExceptionlessClient.Default.Configuration.UseTraceLogger();
            ExceptionlessClient.Default.Configuration.UseReferenceIds();
        }

        public ILogHelper CreateLogHelper(string categoryName)
        {
            throw new NotImplementedException();
        }
    }
}
