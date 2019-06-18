using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityReservation.Models
{
    [Table("tabUser")]
    public class User
    {
        #region Private Field

        private bool isSuper = false;

        private bool isEnabled = true;

        private DateTime addTime = DateTime.UtcNow;

        #endregion Private Field

        /// <summary>
        /// 用户id,唯一编号
        /// </summary>
        [Column]
        [Key]
        public Guid UserId { get; set; }

        /// <summary>
        ///  用户名
        /// </summary>
        [Column]
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Column]
        public string UserPassword { get; set; }

        /// <summary>
        /// 是否是超级管理员
        /// </summary>
        [Column]
        public bool IsSuper
        {
            get { return isSuper; }

            set { isSuper = value; }
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        [Column]
        public DateTime AddTime
        {
            get { return addTime; }

            set { addTime = value; }
        }

        /// <summary>
        /// 用户邮箱
        /// </summary>
        [Column]
        public string UserMail { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Column]
        public bool IsEnabled
        {
            get { return isEnabled; }

            set { isEnabled = value; }
        }
    }
}
