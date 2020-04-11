using ActivityReservation.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ActivityReservation.WorkContexts
{
    [Authorize(Policy = "ReservationManager")]
    [Area("Admin")]
    public class AdminBaseController : BaseController
    {
        public AdminBaseController(ILogger logger, OperLogHelper operLogHelper) : base(logger)
        {
            OperLogHelper = operLogHelper;
        }

        protected readonly OperLogHelper OperLogHelper;

        /// <summary>
        /// 管理员姓名
        /// </summary>
        public string UserName => User.Identity.Name;
    }
}
