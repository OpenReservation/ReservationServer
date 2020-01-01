using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ActivityReservation.Database;
using ActivityReservation.Helpers;
using ActivityReservation.Models;
using ActivityReservation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Models;
using WeihanLi.EntityFramework;

namespace ActivityReservation.API
{
    public class ReservationController : ApiControllerBase
    {
        private readonly IEFRepository<ReservationDbContext, Reservation> _repository;

        public ReservationController(ILogger<ReservationController> logger, IEFRepository<ReservationDbContext, Reservation> repository) : base(logger)
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
            Expression<Func<Reservation, bool>> predict = n => true;
            if (!string.IsNullOrWhiteSpace(phone))
            {
                predict = n => n.ReservationPersonPhone == phone.Trim();
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
            var detail = await _repository.FirstOrDefaultAsync(builder => builder.WithPredict(x => x.ReservationId == id && x.ReservationPersonPhone == phone), cancellationToken);
            if (detail == null)
            {
                return NotFound();
            }
            return Ok(detail);
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
        public async Task<IActionResult> MakeReservation([FromBody]ReservationViewModel model, [FromHeader]string captcha, [FromHeader]string captchaType, [FromServices]CaptchaVerifyHelper captchaVerifyHelper)
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
                if (!HttpContext.RequestServices.GetService<ReservationHelper>()
                    .MakeReservation(model, out var msg))
                {
                    result.ErrorMsg = msg;
                    return Ok(result);
                }

                result.Result = true;
                result.Status = ResultStatus.Success;
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"活动室预约失败：{ex.Message}");
                result.Status = ResultStatus.ProcessFail;
                result.ErrorMsg = ex.Message;
                return Ok(result);
            }
        }
    }
}
