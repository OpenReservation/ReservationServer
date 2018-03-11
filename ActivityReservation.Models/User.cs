using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("tabUser")]
    public class User
    {
        #region Private Field

        private Guid userId;

        private string userName;

        private string userMail;

        private string userPassword;

        private bool isSuper = false;

        private bool isEnabled = true;

        private DateTime addTime = DateTime.Now;

        #endregion Private Field

        /// <summary>
        /// 用户id,唯一编号
        /// </summary>
        [Column]
        [Key]
        public Guid UserId
        {
            get { return userId; }

            set { userId = value; }
        }

        /// <summary>
        ///  用户名
        /// </summary>
        [Column]
        public string UserName
        {
            get { return userName; }

            set { userName = value; }
        }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Column]
        public string UserPassword
        {
            get { return userPassword; }

            set { userPassword = value; }
        }

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
        public string UserMail
        {
            get { return userMail; }

            set { userMail = value; }
        }

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