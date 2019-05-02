using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityReservation.Models
{
    [Table("tabWechatMenuConfig")]
    public class WechatMenuConfig
    {
        [Key]
        public Guid ConfigId { get; set; }

        public Guid ParentId { get; set; }

        [Required]
        public string ButtonKey { get; set; }

        [Required]
        public string ButtonType { get; set; }

        [Required]
        public string Remark { get; set; }
    }
}
