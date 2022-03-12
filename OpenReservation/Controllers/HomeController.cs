using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OpenReservation.Business;
using OpenReservation.Common;
using OpenReservation.Events;
using OpenReservation.Helpers;
using OpenReservation.Models;
using OpenReservation.ViewModels;
using OpenReservation.WorkContexts;
using WeihanLi.Web.Pager;
using WeihanLi.Common.Event;
using WeihanLi.Common.Models;
using WeihanLi.Extensions;
using WeihanLi.Redis;

namespace OpenReservation.Controllers;

public class HomeController : FrontBaseController
{
    private readonly IBLLReservation _reservationBLL;

    public HomeController(ILogger<HomeController> logger, IBLLReservation reservationBLL) : base(logger)
    {
        _reservationBLL = reservationBLL;
    }

    [HttpPost]
    public IActionResult SetLanguage(string culture, string returnUrl)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );
        return LocalRedirect(returnUrl);
    }

    public ActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// 预约记录数据页
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    public async Task<ActionResult> ReservationList(SearchHelperModel search)
    {
        Expression<Func<Reservation, bool>> whereLambda = (m => m.ReservationStatus != ReservationStatus.Canceled);
        if (!string.IsNullOrWhiteSpace(search.SearchItem0) && DateTime.TryParse(search.SearchItem0, out var date))
        {
            whereLambda = whereLambda.And(m => m.ReservationForDate == date.Date);
        }
        if (!string.IsNullOrWhiteSpace(search.SearchItem1))
        {
            whereLambda = whereLambda.And(m => m.ReservationPersonPhone == search.SearchItem1.Trim());
        }
        //load data
        var list = await _reservationBLL.GetPagedListResultAsync(
            x => new ReservationListViewModel
            {
                ReservationForDate = x.ReservationForDate,
                ReservationForTime = x.ReservationForTime,
                ReservationId = x.ReservationId,
                ReservationUnit = x.ReservationUnit,
                ReservationTime = x.ReservationTime,
                ReservationPlaceName = x.Place.PlaceName,
                ReservationActivityContent = x.ReservationActivityContent,
                ReservationPersonName = x.ReservationPersonName,
                ReservationPersonPhone = x.ReservationPersonPhone,
                ReservationStatus = x.ReservationStatus,
            },
            queryBuilder => queryBuilder
                .WithPredict(whereLambda)
                .WithOrderBy(query =>
                    query.OrderByDescending(r => r.ReservationForDate).ThenByDescending(r => r.ReservationTime))
                .WithInclude(query => query.Include(r => r.Place))
            , search.PageIndex, search.PageSize);

        var dataList = list.ToPagedList();
        return View(dataList);
    }

    /// <summary>
    /// 预约页面
    /// </summary>
    /// <returns></returns>
    [Authorize]
    public ActionResult Reservate()
    {
        var places = HttpContext.RequestServices.GetRequiredService<IBLLReservationPlace>()
            .Select(s => s.IsDel == false && s.IsActive)
            .OrderBy(_ => _.PlaceIndex)
            .ThenBy(_ => _.PlaceName)
            .ToList();
        return View(places);
    }

    /// <summary>
    /// 预约日期是否可以预约
    /// </summary>
    /// <returns></returns>
    public ActionResult IsReservationForDateValid(DateTime reservationForDate)
    {
        var isValid = HttpContext.RequestServices.GetRequiredService<ReservationHelper>()
            .IsReservationForDateAvailable(reservationForDate, false, out var msg);
        if (isValid)
        {
            return Json(Result.Success(true));
        }
        else
        {
            var jsonResult = Result.Success(false, msg);
            return Json(jsonResult);
        }
    }

    /// <summary>
    /// 根据预约日期和预约地点获取可用的预约时间段
    /// </summary>
    /// <param name="dt">预约日期</param>
    /// <param name="placeId">预约地点id</param>
    /// <returns></returns>
    public ActionResult GetAvailablePeriods(DateTime dt, Guid placeId, [FromServices] ReservationHelper reservationHelper)
    {
        var periodsStatus = reservationHelper.GetAvailablePeriodsByDateAndPlace(dt, placeId);
        return Json(periodsStatus);
    }

    /// <summary>
    /// 预约接口
    /// </summary>
    /// <param name="model">预约信息实体</param>
    /// <param name="captcha">验证码</param>
    /// <param name="captchaType">captchaType</param>
    /// <param name="captchaVerifyHelper"></param>
    /// <param name="localizer"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> MakeReservation(
        [FromBody] ReservationViewModel model,
        [FromHeader] string captcha,
        [FromHeader] string captchaType,
        [FromServices] CaptchaVerifyHelper captchaVerifyHelper,
        [FromServices] IStringLocalizer<HomeController> localizer)
    {
        var result = new Result<bool>();
        var isCodeValid = await captchaVerifyHelper.ValidateVerifyCodeAsync(captchaType, captcha);
        if (!isCodeValid)
        {
            result.Status = ResultStatus.RequestError;
            result.Msg = localizer["InvalidCaptchaInfo"];
            return Json(result);
        }
        try
        {
            if (ModelState.IsValid)
            {
                if (!HttpContext.RequestServices.GetRequiredService<ReservationHelper>()
                        .MakeReservation(model, out var msg))
                {
                    result.Msg = msg;
                    return Json(result);
                }

                result.Data = true;
                result.Status = ResultStatus.Success;
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            result.Status = ResultStatus.ProcessFail;
            result.Msg = ex.Message;
        }
        return Json(result);
    }

    /// <summary>
    /// Print
    /// </summary>
    /// <returns></returns>
    public ActionResult Check(Guid id, string phone)
    {
        if (id == Guid.Empty)
        {
            return Content("请求参数异常，预约id为空");
        }
        if (string.IsNullOrWhiteSpace(phone))
        {
            return Content("请求异常，请验证手机号");
        }
        var r = _reservationBLL.Fetch(re => re.ReservationId == id);
        if (null == r)
        {
            return Content("请求异常，预约不存在");
        }
        if (r.ReservationPersonPhone != phone.Trim())
        {
            return Content("请求异常，或者手机号输入有误");
        }
        r.Place = HttpContext.RequestServices.GetRequiredService<IBLLReservationPlace>()
            .Fetch(p => p.PlaceId == r.ReservationPlaceId);
        return View(r);
    }

    /// <summary>
    /// 公告
    /// </summary>
    /// <returns></returns>
    public ActionResult Notice()
    {
        return View();
    }

    /// <summary>
    /// 公告列表
    /// </summary>
    /// <returns></returns>
    public async Task<ActionResult> NoticeList(SearchHelperModel search, [FromServices] IBLLNotice noticeService)
    {
        Expression<Func<Notice, bool>> whereExpression = (n => !n.IsDeleted && n.CheckStatus);
        if (!string.IsNullOrEmpty(search.SearchItem1))
        {
            whereExpression = whereExpression.And(n => n.NoticeTitle.Contains(search.SearchItem1.Trim()));
        }
        try
        {
            var noticeList = await noticeService.PagedAsync(search.PageIndex, search.PageSize, whereExpression,
                n => n.NoticePublishTime);
            var data = noticeList.ToPagedList();
            return View(data);
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return View(PagedListResult<Notice>.Empty.ToPagedList());
        }
    }

    /// <summary>
    /// 公告详情
    /// </summary>
    /// <param name="path">访问路径</param>
    /// <param name="eventBus"></param>
    /// <param name="cacheClient"></param>
    /// <returns></returns>
    public async Task<ActionResult> NoticeDetails(string path, [FromServices] IEventBus eventBus, [FromServices] ICacheClient cacheClient)
    {
        if (!string.IsNullOrWhiteSpace(path))
        {
            var notice = await cacheClient.GetOrSetAsync(
                $"Notice_{path.Trim()}",
                () => HttpContext.RequestServices.GetRequiredService<IBLLNotice>()
                    .FetchAsync(n => n.NoticeCustomPath == path.Trim()),
                TimeSpan.FromMinutes(1));
            if (notice != null)
            {
                await eventBus.PublishAsync(new NoticeViewEvent { NoticeId = notice.NoticeId });
                return View(notice);
            }
        }
        return RedirectToAction("Notice");
    }

    [HttpGet]
    public async Task<IActionResult> Chat(string msg)
    {
        if (string.IsNullOrWhiteSpace(msg))
        {
            return BadRequest();
        }
        return Ok(new
        {
            text = await HttpContext.RequestServices.GetRequiredService<ChatBotHelper>()
                .GetBotReplyAsync(msg)
        });
    }

    public ActionResult About()
    {
        return View();
    }
}