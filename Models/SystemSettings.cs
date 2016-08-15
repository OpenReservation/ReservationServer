using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Models
{
    [Table("tabSystemSettings")]
    public class SystemSettings
    {
        /// <summary>
        /// setting id
        /// </summary>
        private Guid settingId;

        /// <summary>
        /// settingName
        /// </summary>
        private string settingName;

        /// <summary>
        /// 显示名称
        /// </summary>
        private string displayName;

        /// <summary>
        /// settingValue
        /// </summary>
        private string settingValue;

        [Key]
        [Column]
        public Guid SettingId
        {
            get
            {
                return settingId;
            }

            set
            {
                settingId = value;
            }
        }
        [Column]
        public string SettingName
        {
            get
            {
                return settingName;
            }

            set
            {
                settingName = value;
            }
        }
        [Column]
        public string DisplayName
        {
            get
            {
                return displayName;
            }

            set
            {
                displayName = value;
            }
        }
        [Column]
        public string SettingValue
        {
            get
            {
                return settingValue;
            }
            set
            {
                settingValue = value;
            }
        }
    }
}