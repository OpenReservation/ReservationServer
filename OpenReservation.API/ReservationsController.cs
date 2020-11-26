using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenReservation.Database;
using OpenReservation.Helpers;
using OpenReservation.Models;
using OpenReservation.ViewModels;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Models;
using WeihanLi.EntityFramework;
using WeihanLi.Extensions;
using WeihanLi.Web.Extensions;

namespace OpenReservation.API
{
    public class ReservationsController : ApiControllerBase
    {
        private readonly IEFRepository<ReservationDbContext, Reservation> _repository;

        public ReservationsController(ILogger<ReservationsController> logger, IEFRepository<ReservationDbContext, Reservation> repository) : base(logger)
        {
            _repository = repository;
        }

        /// <summary>
        /// 活动室预约列表
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="pageNumber">pageNumber</param>
        /// <param name="pageSize">pageSize</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync(string phone, int pageNumber = 1, int pageSize = 10)
        {
            Expression<Func<Reservation, bool>> predict = n => n.ReservationStatus != ReservationStatus.Canceled;
            if (!string.IsNullOrWhiteSpace(phone))
            {
                phone = phone.Trim();
                predict = predict.And(n => n.ReservationPersonPhone == phone);
            }

            var result = await _repository.GetPagedListResultAsync(
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

            foreach (var model in result.Data)
            {
                model.ReservationPersonPhone = StringHelper.HideTelDetails(model.ReservationPersonPhone);
                model.ReservationPersonName = StringHelper.HideSensitiveInfo(model.ReservationPersonName, 1, 0, 2);
            }

            return Ok(result);
        }

        /// <summary>
        /// 获取预约详情
        /// </summary>
        /// <param name="id">预约id</param>
        /// <param name="phone">预约人手机号</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(Guid id, string phone, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var phoneNumValid = ValidateHelper.IsMobile(phone);
            var userId = User.Identity.IsAuthenticated
                ? User.GetUserId<Guid>()
                : Guid.Empty
                ;
            if (userId == Guid.Empty && phoneNumValid == false)
            {
                return BadRequest();
            }

            Reservation detail;
            if (phoneNumValid)
            {
                detail = await _repository.FirstOrDefaultAsync(builder => builder.WithPredict(x => x.ReservationId == id && x.ReservationPersonPhone == phone), cancellationToken);
                if (detail == null)
                {
                    return NotFound();
                }
            }
            else
            {
                detail = await _repository.FirstOrDefaultAsync(builder => builder.WithPredict(x => x.ReservationId == id && x.ReservedBy == userId), cancellationToken);
                if (detail == null)
                {
                    return NotFound();
                }
            }

            return Ok(detail);
        }

        /// <summary>
        /// 取消预约
        /// </summary>
        /// <param name="id">id</param>
        [HttpDelete("{id}")]
        [Authorize(Policy = "ReservationApi")]
        public async Task<IActionResult> CancelReservation(Guid id)
        {
            var userId = User.GetUserId<Guid>();
            if (userId == Guid.Empty)
                return new StatusCodeResult(401);

            var exists = await _repository.ExistAsync(x => x.ReservationId == id
            && x.ReservedBy == userId
            && x.ReservationForDate > DateTime.UtcNow.AddHours(8).Date);
            if (!exists)
            {
                return new StatusCodeResult(403);
            }

            var result = await _repository.UpdateAsync(
                new Reservation()
                {
                    ReservationId = id,
                    ReservationStatus = ReservationStatus.Canceled,
                },
                r => r.ReservationStatus);

            return new ResultModel()
            {
                Status = result > 0
                    ? ResultStatus.Success
                    : ResultStatus.ProcessFail,
            }.GetOkObjectResult();
        }

        /// <summary>
        /// 获取用户预约列表
        /// </summary>
        [Authorize(Policy = "ReservationApi")]
        [HttpGet("user")]
        [ResponseCache(CacheProfileName = "noCache")]
        public async Task<IActionResult> GetUserReservations(int pageNumber = 1, int pageSize = 10)
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
            var result = await _repository.GetPagedListResultAsync(
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

            return Ok(result);
        }

        /// <summary>
        /// 提交预约信息
        /// </summary>
        /// <param name="model">预约信息</param>
        /// <param name="captcha">captcha info</param>
        /// <param name="captchaType">captchaType</param>
        /// <param name="captchaVerifyHelper">captchaVerifyHelper</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "ReservationApi")]
        public async Task<IActionResult> MakeReservation(
            [FromBody] ReservationViewModel model,
            [FromHeader] string captcha,
            [FromHeader] string captchaType,
            [FromServices] CaptchaVerifyHelper captchaVerifyHelper
            )
        {
            var result = new ResultModel<bool> { Status = ResultStatus.RequestError };
            var isCodeValid = await captchaVerifyHelper
                .ValidateVerifyCodeAsync(captchaType, captcha);
            if (!isCodeValid)
            {
                result.ErrorMsg = "验证码有误, 请重新验证";
                return Ok(result);
            }
            try
            {
                if (!HttpContext.RequestServices.GetRequiredService<ReservationHelper>()
                    .MakeReservation(model, out var msg))
                {
                    result.ErrorMsg = msg;
                }
                else
                {
                    result.Result = true;
                    result.Status = ResultStatus.Success;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Make reservation exception: {ex.Message}");
                result.Status = ResultStatus.ProcessFail;
                result.ErrorMsg = ex.Message;
            }
            return Ok(result);
        }
    }
}
