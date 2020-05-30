using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ActivityReservation.Business;
using ActivityReservation.Common;
using ActivityReservation.Events;
using ActivityReservation.Helpers;
using ActivityReservation.Models;
using ActivityReservation.ViewModels;
using ActivityReservation.WorkContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using WeihanLi.AspNetMvc.MvcSimplePager;
using WeihanLi.Common.Event;
using WeihanLi.Common.Models;
using WeihanLi.EntityFramework;
using WeihanLi.Extensions;
using WeihanLi.Redis;

namespace ActivityReservation.Controllers
{
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
        public ActionResult ReservationList(SearchHelperModel search)
        {
            Expression<Func<Reservation, bool>> whereLambda = (m => true);
            if (!string.IsNullOrWhiteSpace(search.SearchItem0) && DateTime.TryParse(search.SearchItem0, out var date))
            {
                whereLambda = m => m.ReservationForDate == date.Date;
            }
            if (!string.IsNullOrWhiteSpace(search.SearchItem1))
            {
                whereLambda = m => m.ReservationPersonPhone == search.SearchItem1.Trim();
            }
            //load data
            var list = _reservationBLL.GetPagedListResult(
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
            var places = HttpContext.RequestServices.GetService<IBLLReservationPlace>()
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
            var isValid = HttpContext.RequestServices.GetService<ReservationHelper>().IsReservationForDateAvailable(reservationForDate, false, out var msg);
            if (isValid)
            {
                return Json(ResultModel.Success(true));
            }
            else
            {
                var jsonResult =
                    new ResultModel<bool> { Status = ResultStatus.Success, Result = false, ErrorMsg = msg };
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
            var result = new ResultModel<bool>();
            var isCodeValid = await captchaVerifyHelper.ValidateVerifyCodeAsync(captchaType, captcha);
            if (!isCodeValid)
            {
                result.Status = ResultStatus.RequestError;
                result.ErrorMsg = localizer["InvalidCaptchaInfo"];
                return Json(result);
            }
            try
            {
                if (ModelState.IsValid)
                {
                    if (!HttpContext.RequestServices.GetService<ReservationHelper>()
                        .MakeReservation(model, out var msg))
                    {
                        result.ErrorMsg = msg;
                        return Json(result);
                    }

                    result.Result = true;
                    result.Status = ResultStatus.Success;
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                result.Status = ResultStatus.ProcessFail;
                result.ErrorMsg = ex.Message;
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
            if (r.ReservationPersonPhone != phone.Trim())
            {
                return Content("请求异常，或者手机号输入有误");
            }
            r.Place = HttpContext.RequestServices.GetService<IBLLReservationPlace>()
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
        public ActionResult NoticeList(SearchHelperModel search, [FromServices] IBLLNotice noticeService)
        {
            Expression<Func<Notice, bool>> whereLamdba = (n => !n.IsDeleted && n.CheckStatus);
            if (!string.IsNullOrEmpty(search.SearchItem1))
            {
                whereLamdba = whereLamdba.And(n => n.NoticeTitle.Contains(search.SearchItem1.Trim()));
            }
            try
            {
                var noticeList = noticeService.Paged(search.PageIndex, search.PageSize, whereLamdba,
                    n => n.NoticePublishTime, false);
                var data = noticeList.ToPagedList();
                return View(data);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
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
                    () => HttpContext.RequestServices.GetService<IBLLNotice>()
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
                text = await HttpContext.RequestServices.GetService<ChatBotHelper>()
                    .GetBotReplyAsync(msg)
            });
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
