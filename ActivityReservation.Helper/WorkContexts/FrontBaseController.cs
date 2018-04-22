using System.Web.Mvc;
using WeihanLi.Common.Log;

namespace ActivityReservation.WorkContexts
{
    /// <summary>
    /// 前台基类控制器
    /// </summary>
    public class FrontBaseController : Controller
    {
        protected ILogHelper Logger;

        public FrontBaseController(ILogHelper logger)
        {
            Logger = logger;
        }
    }
}
