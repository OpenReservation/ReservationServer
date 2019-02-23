using System;
using System.Linq;
using System.Linq.Expressions;
using ActivityReservation.Business;
using ActivityReservation.Common;
using ActivityReservation.Helpers;
using ActivityReservation.Models;
using ActivityReservation.ViewModels;
using ActivityReservation.WorkContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WeihanLi.AspNetMvc.MvcSimplePager;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Models;
using WeihanLi.Extensions;

namespace ActivityReservation.Controllers
{
    public class HomeController : FrontBaseController
    {
        private readonly IBLLReservation _reservertionBLL;

        public HomeController(ILogger<HomeController> logger, IBLLReservation reservertionBLL) : base(logger)
        {
            _reservertionBLL = reservertionBLL;
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
            Expression<Func<Reservation, bool>> whereLambda = (m =>
                EF.Functions.DateDiffDay(DateTime.Today, m.ReservationForDate) <= 7 &&
                EF.Functions.DateDiffDay(DateTime.Today, m.ReservationForDate) >= 0);
            //补充查询条件
            //根据预约日期查询
            if (!string.IsNullOrEmpty(search.SearchItem0))
            {
                whereLambda = m =>
                    EF.Functions.DateDiffDay(DateTime.Parse(search.SearchItem0), m.ReservationForDate) == 0;
            }
            //根据预约人联系方式查询
            if (!string.IsNullOrEmpty(search.SearchItem1))
            {
                whereLambda = m => m.ReservationPersonPhone.Contains(search.SearchItem1);
            }
            //load data
            var list = _reservertionBLL.GetReservationList(search.PageIndex, search.PageSize, out var rowsCount,
                whereLambda, m => m.ReservationForDate, m => m.ReservationTime, false, false);
            var dataList = list.ToPagedList(search.PageIndex, search.PageSize, rowsCount);
            return View(dataList);
        }

        /// <summary>
        /// 预约页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Reservate()
        {
            var places = HttpContext.RequestServices.GetService<IBLLReservationPlace>().Select(s => s.IsDel == false && s.IsActive).OrderBy(_ => _.PlaceIndex).ToList();
            return View(places);
        }

        /// <summary>
        /// 预约日期是否可以预约
        /// </summary>
        /// <returns></returns>
        public ActionResult IsReservationForDateValid(DateTime reservationForDate)
        {
            var jsonResult = new JsonResultModel<bool>() { Status = JsonResultStatus.Success };
            var isValid = HttpContext.RequestServices.GetService<ReservationHelper>().IsReservationForDateAvailable(reservationForDate, false, out var msg);
            if (isValid)
            {
                jsonResult.SetSuccessResult(true);
            }
            else
            {
                jsonResult.Result = false;
                jsonResult.ErrorMsg = msg;
            }
            return Json(jsonResult);
        }

        /// <summary>
        /// 根据预约日期和预约地点获取可用的预约时间段
        /// </summary>
        /// <param name="dt">预约日期</param>
        /// <param name="placeId">预约地点id</param>
        /// <returns></returns>
        public ActionResult GetAvailablePeriods(DateTime dt, Guid placeId)
        {
            var periodsStatus = HttpContext.RequestServices.GetService<ReservationHelper>().GetAvailablePeriodsByDateAndPlace(dt, placeId);
            return Json(periodsStatus);
        }

        /// <summary>
        /// 预约接口
        /// </summary>
        /// <param name="model">预约信息实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MakeReservation([FromBody]ReservationViewModel model)
        {
            var result = new JsonResultModel();
            try
            {
                if (ModelState.IsValid)
                {
                    string msg;
                    if (!HttpContext.RequestServices.GetService<ReservationHelper>().IsReservationAvailable(model, out msg))
                    {
                        result.ErrorMsg = msg;
                        return Json(result);
                    }

                    var reservation = new Reservation()
                    {
                        ReservationForDate = model.ReservationForDate,
                        ReservationForTime = model.ReservationForTime,
                        ReservationPlaceId = model.ReservationPlaceId,

                        ReservationUnit = model.ReservationUnit,
                        ReservationActivityContent = model.ReservationActivityContent,
                        ReservationPersonName = model.ReservationPersonName,
                        ReservationPersonPhone = model.ReservationPersonPhone,

                        ReservationFromIp = HttpContext.Connection.RemoteIpAddress.ToString(), //记录预约人IP地址

                        UpdateBy = model.ReservationPersonName,
                        UpdateTime = DateTime.Now,
                        ReservationId = Guid.NewGuid()
                    };
                    //TODO:验证最大可预约时间段，同一个手机号，同一个IP地址
                    // 需要验证这种预约判断是否可以通用，可能有bug
                    foreach (var item in model.ReservationForTimeIds.Split(',').Select(_ => Convert.ToInt32(_)))
                    {
                        reservation.ReservationPeriod += (1 << item);
                    }
                    var bValue = _reservertionBLL.Insert(reservation);
                    if (bValue > 0)
                    {
                        result.Result = true;
                        result.Status = JsonResultStatus.Success;
                    }
                    else
                    {
                        result.ErrorMsg = "预约失败";
                        result.Status = JsonResultStatus.ProcessFail;
                    }
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                result.Status = JsonResultStatus.ProcessFail;
                result.ErrorMsg = ex.Message;
            }
            return Json(result);
        }

        /// <summary>
        /// Print
        /// </summary>
        /// <returns></returns>
        [HttpPost]
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
            var r = _reservertionBLL.Fetch(re => re.ReservationId == id);
            if (r.ReservationPersonPhone != phone.Trim())
            {
                return Content("请求异常，或者手机号输入有误");
            }
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
        public ActionResult NoticeList(SearchHelperModel search)
        {
            Expression<Func<Notice, bool>> whereLamdba = (n => !n.IsDeleted && n.CheckStatus);
            if (!string.IsNullOrEmpty(search.SearchItem1))
            {
                whereLamdba = n => n.NoticeTitle.Contains(search.SearchItem1) && n.CheckStatus;
            }
            try
            {
                var noticeList = HttpContext.RequestServices.GetService<IBLLNotice>().Paged(search.PageIndex, search.PageSize, whereLamdba,
                    n => n.NoticePublishTime, false);
                var data = noticeList.ToPagedList(search.PageIndex, search.PageSize, noticeList.TotalCount);
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
        /// <returns></returns>
        public ActionResult NoticeDetails(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return RedirectToAction("Notice");
            }
            try
            {
                var notice = HttpContext.RequestServices.GetService<IBLLNotice>().Fetch(n => n.NoticeCustomPath == path);
                if (notice != null)
                {
                    return View(notice);
                }
                else
                {
                    return RedirectToAction("Notice");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// 获取Geetest验证码
        /// </summary>
        /// <returns></returns>
        public JsonResult GetGeetestValidCode()
        {
            var helper = HttpContext.RequestServices.GetRequiredService<GeetestHelper>();
            var userIp = HttpContext.Connection.RemoteIpAddress.ToString();
            var gtServerStatus = helper.PreProcess(userIp);
            HttpContext.Session.SetString(GeetestConsts.GeetestUserId, userIp);
            HttpContext.Session.SetString(GeetestConsts.GtServerStatusSessionKey, gtServerStatus.ToString());
            return Json(helper.Response);
        }

        /// <summary>
        /// 验证Geetest验证码
        /// </summary>
        /// <returns></returns>
        public JsonResult ValidateGeetestCode()
        {
            var geetestRequest = new GeetestRequestModel
            {
                challenge = Request.Form[GeetestConsts.FnGeetestChallenge],
                validate = Request.Form[GeetestConsts.FnGeetestValidate],
                seccode = Request.Form[GeetestConsts.FnGeetestSeccode]
            };

            return Json(HttpContext.RequestServices.GetRequiredService<GeetestHelper>()
                .ValidateRequest(geetestRequest,
                    HttpContext.Session.GetString(GeetestConsts.GeetestUserId)?.ToString() ?? "",
                    Convert.ToByte(HttpContext.Session.GetString(GeetestConsts.GtServerStatusSessionKey)),
                () => { HttpContext.Session.Remove(GeetestConsts.GeetestUserId); }));
        }

        /// <summary>
        /// 验证谷歌验证码
        /// </summary>
        /// <param name="response">response</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidateGoogleRecaptchaResponse(string response)
        {
            var helper = HttpContext.RequestServices.GetRequiredService<GoogleRecaptchaHelper>();
            return Json(helper.IsValidRequest(response));
        }
    }
}
