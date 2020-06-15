using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenReservation.Models
{
    [Table("tabSystemSettings")]
    public class SystemSettings
    {
        [Key]
        [Column]
        public Guid SettingId { get; set; }

        [Column]
        [Required]
        [StringLength(64)]
        public string SettingName { get; set; }

        [Column]
        [StringLength(64)]
        public string DisplayName { get; set; }

        [Column]
        public string SettingValue { get; set; }
    }
}
