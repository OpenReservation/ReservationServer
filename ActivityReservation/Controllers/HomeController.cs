using ActivityReservation.Helpers;
using MvcSimplePager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Common;
using System.Threading.Tasks;

namespace ActivityReservation.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// logger
        /// </summary>
        private static Common.LogHelper logger = new Common.LogHelper(typeof(HomeController));
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
            Expression<Func<Models.Reservation , bool>> whereLambda = (m => System.Data.Entity.DbFunctions.DiffDays(DateTime.Today , m.ReservationForDate) <= 7 && System.Data.Entity.DbFunctions.DiffDays(DateTime.Today , m.ReservationForDate) >= 0);
            int rowsCount = 0;
            //补充查询条件
            //根据预约日期查询
            if (!String.IsNullOrEmpty(search.SearchItem0))
            {
                whereLambda = (m => System.Data.Entity.DbFunctions.DiffDays(DateTime.Parse(search.SearchItem0) , m.ReservationForDate) == 0);
            }
            //根据预约人联系方式查询
            if (!String.IsNullOrEmpty(search.SearchItem1))
            {
                whereLambda=(m => m.ReservationPersonPhone.Contains(search.SearchItem1));
            }
            //load data
            List<Models.Reservation> list = new Business.BLLReservation().GetReservationList(search.PageIndex , search.PageSize , out rowsCount , whereLambda , m => m.ReservationForDate , m => m.ReservationTime , false , false);
            IPagedListModel<Models.Reservation> dataList = list.ToPagedList(search.PageIndex , search.PageSize , rowsCount);
            return View(dataList);
        }
        /// <summary>
        /// 预约页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Reservate()
        {
            List<Models.ReservationPlace> places = new Business.BLLReservationPlace().GetAll(s => s.IsDel == false && s.IsActive,s=>s.PlaceName,true);
            return View(places);
        }
        /// <summary>
        /// 根据预约日期和预约地点获取可用的预约时间段
        /// </summary>
        /// <param name="dt">预约日期</param>
        /// <param name="placeId">预约地点id</param>
        /// <returns></returns>
        public ActionResult GetAvailablePeriods(DateTime dt , Guid placeId)
        {
            bool[] periodsStatus = ReservationHelper.GetAvailabelPeriodsByDateAndPlace(dt , placeId);
            return Json(periodsStatus);
        }
        /// <summary>
        /// 预约接口
        /// </summary>
        /// <param name="model">预约信息实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MakeReservation(ViewModels.ReservationViewModel model)
        {
            HelperModels.JsonResultModel result = new HelperModels.JsonResultModel() { Data = false , Status = HelperModels.JsonResultStatus.RequestError };
            try
            {
                if (ModelState.IsValid)
                {
                    string msg;
                    if (!ReservationHelper.IsReservationAvailabel(model,out msg))
                    {
                        result.Msg = msg;
                        return Json(result);
                    }

                    Models.Reservation reservation = new Models.Reservation()
                    {
                        ReservationForDate = model.ReservationForDate ,
                        ReservationForTime = model.ReservationForTime ,
                        ReservationPlaceId = model.ReservationPlaceId ,

                        ReservationUnit = model.ReservationUnit ,
                        ReservationActivityContent = model.ReservationActivityContent ,
                        ReservationPersonName = model.ReservationPersonName ,
                        ReservationPersonPhone = model.ReservationPersonPhone ,

                        ReservationFromIp = HttpContext.Request.UserHostAddress ,//记录预约人IP地址

                        UpdateBy = model.ReservationPersonName ,
                        UpdateTime = DateTime.Now ,
                        ReservationId = Guid.NewGuid()
                    };
                    foreach (string item in model.ReservationForTimeIds.Split(','))
                    {
                        switch (Convert.ToInt32(item))
                        {
                            case 1:
                                reservation.T1 = false;
                                break;
                            case 2:
                                reservation.T2 = false;
                                break;
                            case 3:
                                reservation.T3 = false;
                                break;
                            case 4:
                                reservation.T4 = false;
                                break;
                            case 5:
                                reservation.T5 = false;
                                break;
                            case 6:
                                reservation.T6 = false;
                                break;
                            case 7:
                                reservation.T7 = false;
                                break;
                            default:
                                break;
                        }
                    }
                    var bValue = new Business.BLLReservation().Add(reservation);
                    if (bValue > 0)
                    {
                        result.Data = true;
                        result.Msg = "预约成功";
                        result.Status = HelperModels.JsonResultStatus.Success;
                    }
                    else
                    {
                        result.Msg = "预约失败";
                        result.Status = HelperModels.JsonResultStatus.ProcessFail;
                    }
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                result.Status = HelperModels.JsonResultStatus.ProcessFail;
                result.Msg = ex.Message;
            }
            return Json(result);
        }
        /// <summary>
        /// Print
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Check(Guid id)
        {
            Models.Reservation r = new Business.BLLReservation().GetOne(re => re.ReservationId == id);
            return View(r);
        }
        /// <summary>
        /// 公告
        /// </summary>
        /// <param name="path">路径</param>
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
            Expression<Func<Models.Notice , bool>> whereLamdba = (n => !n.IsDeleted && n.CheckStatus);
            if (!String.IsNullOrEmpty(search.SearchItem1))
            {
                whereLamdba = whereLamdba.And(n => n.NoticeTitle.Contains(search.SearchItem1));
            }
            try
            {
                int count = 0;
                var noticeList = new Business.BLLNotice().GetPagedList(search.PageIndex , search.PageSize , out count , whereLamdba , n => n.NoticePublishTime , false);
                IPagedListModel<Models.Notice> data = noticeList.ToPagedList(search.PageIndex , search.PageSize , count);
                return View(data);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
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
            if (String.IsNullOrEmpty(path))
            {
                return RedirectToAction("Notice");
            }
            try
            {
                var notice = new Business.BLLNotice().GetOne(n => n.NoticePath == path);
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
                logger.Error(ex);
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
            GeetestHelper helper = new GeetestHelper(GeetestConsts.publicKey,GeetestConsts.privateKey);
            string userID = RequestHelper.GetRequestIP();
            byte gtServerStatus = helper.preProcess(userID);
            Session[GeetestConsts.GtServerStatusSessionKey] = gtServerStatus;
            Session["geetestUserId"] = userID;
            return Json(helper.Response,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 验证Geetest验证码
        /// </summary>
        /// <returns></returns>
        public JsonResult ValidateGeetestCode()
        {
            GeetestHelper helper = new GeetestHelper(GeetestConsts.publicKey, GeetestConsts.privateKey);
            byte gt_server_status_code = (byte)Session[GeetestConsts.GtServerStatusSessionKey];
            string userID = Session["geetestUserId"] as string;
            int result = 0;
            string challenge = Request[GeetestConsts.fnGeetestChallenge];
            string validate = Request[GeetestConsts.fnGeetestValidate];
            string seccode = Request[GeetestConsts.fnGeetestSeccode];
            if (gt_server_status_code == 1)
            {
                result = helper.enhencedValidateRequest(challenge, validate, seccode, userID);
            }
            else
            {
                result = helper.failbackValidateRequest(challenge, validate, seccode);
            }
            if(result == 1)
            {
                Session.Remove("geetestUserId");
                return Json(true);
            }
            return Json(false);
        }
    }
}