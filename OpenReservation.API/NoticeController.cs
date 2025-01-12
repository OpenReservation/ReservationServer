using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenReservation.Business;
using OpenReservation.Database;
using OpenReservation.Events;
using OpenReservation.Models;
using WeihanLi.Common.Event;
using WeihanLi.Common.Helpers;
using WeihanLi.EntityFramework;
using WeihanLi.Extensions;
using WeihanLi.Redis;

namespace OpenReservation.API;

/// <summary>
/// 公告 API
/// </summary>
public class NoticeController(ILogger<NoticeController> logger, IEFRepository<ReservationDbContext, Notice> repository)
    : ApiControllerBase(logger)
{
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
        var predict = ExpressionHelper.True<Notice>();
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.Trim();
            predict = predict.And(n => n.NoticeTitle.Contains(keyword));
        }
        var result = await repository.GetPagedListResultAsync(x => new
            {
                x.NoticeTitle,
                x.NoticeCustomPath,
                x.NoticePublishTime,
                x.NoticeExternalLink
            }, queryBuilder => queryBuilder
                .WithPredict(predict)
                .WithOrderBy(q => q.OrderByDescending(n => n.NoticePublishTime))
            , pageNumber, pageSize, HttpContext.RequestAborted);

        return Ok(result);
    }

    /// <summary>
    /// 获取公告详情
    /// </summary>
    /// <param name="path">path</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <param name="eventBus">eventBus</param>
    /// <param name="cacheClient">cacheClient</param>
    /// <returns></returns>
    [HttpGet("{path}")]
    [ResponseCache(CacheProfileName = "noCache")]
    public async Task<IActionResult> GetByPath(string path, CancellationToken cancellationToken, 
        [FromServices] IEventBus eventBus, [FromServices] IMemoryCache cacheClient)
    {
        path = path?.Trim();
        if (string.IsNullOrEmpty(path)) 
            return BadRequest("Notice");

        var cacheKey = "notice_" + path;
        var notice = await cacheClient.GetOrCreateAsync(
            cacheKey,
            _ => HttpContext.RequestServices.GetRequiredService<IBLLNotice>()
                .FetchAsync(n => n.NoticeCustomPath == path.Trim(), cancellationToken), new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
        if (notice is null)
        {
            return NotFound();
        }

        await eventBus.PublishAsync(new NoticeViewEvent { NoticeId = notice.NoticeId });
        return Ok(notice);
    }
}
