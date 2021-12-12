using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenReservation.Models;

/// <summary>
/// 禁用时间段
/// </summary>
[Table("tabDisabledPeriod")]
public class DisabledPeriod
{
    /// <summary>
    /// 主键
    /// </summary>
    [Key]
    public Guid PeriodId { get; set; }

    /// <summary>
    /// 开始日期
    /// </summary>
    [Column]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    [Column]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 每年都禁用
    /// </summary>
    [Column]
    public bool RepeatYearly { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    [Column]
    public string UpdatedBy { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [Column]
    public DateTime UpdatedTime { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    [Column]
    public bool IsActive { get; set; }
}