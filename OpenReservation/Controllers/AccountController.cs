using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenReservation.Business;
using OpenReservation.Models;
using OpenReservation.ViewModels;
using OpenReservation.WorkContexts;
using WeihanLi.AspNetMvc.MvcSimplePager;
using WeihanLi.Web.Extensions;

namespace OpenReservation.Controllers
{
    public class AccountController : FrontBaseController
    {
        public AccountController(ILogger<AccountController> logger) : base(logger)
        {
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        /// <summary>
        /// 用户预约列表
        /// </summary>
        [Authorize]
        public ActionResult UserReservation()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult> UserReservationList(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            [FromServices] IBLLReservation reservationBLL)
        {
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }
            if (pageSize <= 0)
            {
                pageSize = 10;
            }
            var userId = User.GetUserId<Guid>();
            Expression<Func<Reservation, bool>> predict = n => n.ReservedBy == userId;
            var result = await reservationBLL.GetPagedListResultAsync(
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
                    .WithPredict(predict)
                    .WithOrderBy(q => q.OrderByDescending(_ => _.ReservationForDate).ThenByDescending(_ => _.ReservationTime))
                    .WithInclude(q => q.Include(x => x.Place))
                , pageNumber, pageSize, HttpContext.RequestAborted);

            return View(result.ToPagedList());
        }

        [AllowAnonymous]
        public IActionResult Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                Logger.Info($"{HttpContext.User.Identity.Name} logout at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");

                return new SignOutResult(new[] { "Cookies", "OpenIdConnect" });
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
