using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("tabUser")]
    public class User
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        private Guid userId;

        /// <summary>
        ///  用户名
        /// </summary>
        private string userName;

        /// <summary>
        /// 用户邮箱，用来激活账号和找回密码
        /// </summary>
        private string userMail;

        /// <summary>
        /// 用户密码
        /// </summary>
        private string userPassword;

        /// <summary>
        /// 是否是超级管理员
        /// </summary>
        private bool isSuper = false;

        /// <summary>
        /// 添加时间
        /// </summary>
        private DateTime addTime = DateTime.Now;

        [Column]
        [Key]
        public Guid UserId
        {
            get
            {
                return userId;
            }

            set
            {
                userId = value;
            }
        }

        [Column]
        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                userName = value;
            }
        }

        [Column]
        public string UserPassword
        {
            get
            {
                return userPassword;
            }

            set
            {
                userPassword = value;
            }
        }

        [Column]
        public bool IsSuper
        {
            get
            {
                return isSuper;
            }

            set
            {
                isSuper = value;
            }
        }

        [Column]
        public DateTime AddTime
        {
            get
            {
                return addTime;
            }

            set
            {
                addTime = value;
            }
        }

        public string UserMail
        {
            get
            {
                return userMail;
            }

            set
            {
                userMail = value;
            }
        }
    }
}