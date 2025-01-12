using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenReservation.Helpers;

namespace OpenReservation.WorkContexts;

[Authorize(Policy = "ReservationManager")]
[Area("Admin")]
public class AdminBaseController(ILogger logger, OperLogHelper operLogHelper) : BaseController(logger)
{
    protected readonly OperLogHelper OperLogHelper = operLogHelper;

    /// <summary>
    /// 管理员姓名
    /// </summary>
    public string UserName => User.Identity?.Name;
}