using Microsoft.Extensions.Logging;

namespace OpenReservation.WorkContexts;

/// <summary>
/// 前台基类控制器
/// </summary>
public abstract class FrontBaseController : BaseController
{
    protected FrontBaseController(ILogger logger) : base(logger)
    {
    }
}