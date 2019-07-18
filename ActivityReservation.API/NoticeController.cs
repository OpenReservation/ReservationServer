using ActivityReservation.Events;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ActivityReservation.Database;
using ActivityReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeihanLi.Common.Event;
using WeihanLi.EntityFramework;
using WeihanLi.Extensions;

namespace ActivityReservation.API
{
    public class NoticeController : ApiControllerBase
    {
        private readonly IEFRepository<ReservationDbContext, Notice> _repository;

        public NoticeController(ILogger<NoticeController> logger, IEFRepository<ReservationDbContext, Notice> repository) : base(logger)
        {
            _repository = repository;
        }

        /// <summary>
        /// 获取公告列表
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <param name="pageNumber">pageNumber</param>
        /// <param name="pageSize">pageSize</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync(string keyword, int pageNumber = 1, int pageSize = 10)
        {
            Expression<Func<Notice, bool>> predict = n => true;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                predict = predict.And(n => n.NoticeTitle.Contains(keyword));
            }
            var result = await _repository.GetPagedListResultAsync(x => new
            {
                x.NoticeTitle,
                x.NoticeVisitCount,
                x.NoticeCustomPath,
                x.NoticePublisher,
                x.NoticePublishTime,
                x.NoticeImagePath
            }, queryBuilder => queryBuilder
                   .WithPredict(predict)
                   .WithOrderBy(q => q.OrderByDescending(_ => _.NoticePublishTime))
                , pageNumber, pageSize, HttpContext.RequestAborted);

            return Ok(result);
        }

        /// <summary>
        /// 获取公告详情
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <param name="eventBus">eventBus</param>
        /// <returns></returns>
        [HttpGet("{path}")]
        public async Task<IActionResult> GetByPath(string path, CancellationToken cancellationToken, [FromServices]IEventBus eventBus)
        {
            var notice = await _repository.FetchAsync(n => n.NoticeCustomPath == path, cancellationToken);
            if (notice == null)
            {
                return NotFound();
            }
            eventBus.Publish(new NoticeViewEvent { NoticeId = notice.NoticeId });
            return Ok(notice);
        }
    }
}
