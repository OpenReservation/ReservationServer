using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityReservation.Models
{
    [Table("tabSystemSettings")]
    public class SystemSettings
    {
        [Key]
        [Column]
        public Guid SettingId { get; set; }

        [Column]
        public string SettingName { get; set; }

        [Column]
        public string DisplayName { get; set; }

        [Column]
        public string SettingValue { get; set; }
    }
}
