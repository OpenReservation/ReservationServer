using System.Web;
using System.Web.Mvc;

namespace Common.Filters
{
    public class ErrorHandlerAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext!=null)
            {
                HttpException httpEx = new HttpException(null, filterContext.Exception);
                new Common.LogHelper("GlobalError").Error(httpEx);
                int errorCode = httpEx.GetHttpCode();
                ViewResult view = null;
                switch (errorCode)
                {
                    case 404:
                        view = new ViewResult() { ViewName = "NotFound",MasterName= "_Layout" };
                        break;
                    default:
                        view = new ViewResult() { ViewName = "Error",MasterName= "_Layout" };
                        break;
                }
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.Result = view;
            }
            
        }
    }
}
