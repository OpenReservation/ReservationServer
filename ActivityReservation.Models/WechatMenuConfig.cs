using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityReservation.Models
{
    [Table("tabWechatMenuConfig")]
    public class WechatMenuConfig
    {
        [Column]
        [Key]
        public Guid ConfigId { get; set; }

        public Guid ParentId { get; set; }

        public string ButtonKey { get; set; }

        public string ButtonType { get; set; }

        public string Remark { get; set; }
    }
}
