using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenReservation.Models;

[Table("tabBlockType")]
public class BlockType
{
    /// <summary>
    /// 黑名单类型id
    /// </summary>
    [Column]
    [Key]
    public Guid TypeId { get; set; }

    /// <summary>
    /// 黑名单类型名称
    /// </summary>
    [Column]
    [Required]
    [StringLength(64)]
    public string TypeName { get; set; }
}