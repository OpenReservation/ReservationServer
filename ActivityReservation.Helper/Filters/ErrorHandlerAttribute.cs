using System.Web;
using System.Web.Mvc;
using WeihanLi.Common.Helpers;

namespace ActivityReservation.Filters
{
    public class ErrorHandlerAttribute : HandleErrorAttribute
    {        
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext!=null)
            {
                HttpException httpEx = new HttpException(null, filterContext.Exception);
                new LogHelper(typeof(ErrorHandlerAttribute)).Error(httpEx);
                int errorCode = httpEx.GetHttpCode();
                ViewResult view = null;
                switch (errorCode)
                {
                    case 404:
                        view = new ViewResult() { ViewName = "NotFound",MasterName= null };
                        break;
                    default:
                        view = new ViewResult() { ViewName = "Error",MasterName= null };
                        break;
                }
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.Result = view;
            }            
        }
    }
}