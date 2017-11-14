using System.Web.Mvc;
using WeihanLi.Common.Helpers;

namespace ActivityReservation.WorkContexts
{
    /// <summary>
    /// 前台基类控制器
    /// </summary>
    public class FrontBaseController : Controller
    {
        protected LogHelper Logger;

        public FrontBaseController(LogHelper logger)
        {
            Logger = logger;
        }
    }
}