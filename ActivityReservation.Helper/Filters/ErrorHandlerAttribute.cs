using System.Web;
using System.Web.Mvc;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Log;

namespace ActivityReservation.Filters
{
    public class ErrorHandlerAttribute : HandleErrorAttribute
    {
        private static readonly ILogHelper Logger = LogHelper.GetLogHelper<ErrorHandlerAttribute>();

        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext != null)
            {
                var httpEx = new HttpException(null, filterContext.Exception);

                Logger.Error(httpEx);

                var errorCode = httpEx.GetHttpCode();
                ViewResult view = null;
                switch (errorCode)
                {
                    case 404:
                        view = new ViewResult() { ViewName = "NotFound", MasterName = null };
                        break;

                    default:
                        view = new ViewResult() { ViewName = "Error", MasterName = null };
                        break;
                }
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.Result = view;
            }
        }
    }
}
