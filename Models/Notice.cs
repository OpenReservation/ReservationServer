using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("tabNotice")]
    public class Notice
    {
        #region Private Field

        private Guid noticeId;

        private string noticeTitle;

        private string noticeDesc;

        private string noticeContent;

        private string noticePath;

        private string noticeExternalLink;

        private string noticeImagePath;

        private string noticeCustomPath;

        private int noticeVisitCount;

        private DateTime updateTime;

        private string updateBy;

        private DateTime noticePublishTime;

        private string noticePublisher;

        private bool checkStatus = false;

        private bool isDeleted = false;

        #endregion Private Field

        /// <summary>
        /// 公告id
        /// </summary>
        [Key]
        [Column]
        public Guid NoticeId
        {
            get { return noticeId; }

            set { noticeId = value; }
        }

        /// <summary>
        /// 公告标题
        /// </summary>
        [Column]
        public string NoticeTitle
        {
            get { return noticeTitle; }

            set { noticeTitle = value; }
        }

        /// <summary>
        /// 公告简介
        /// </summary>
        [Column]
        public string NoticeDesc
        {
            get { return noticeDesc; }

            set { noticeDesc = value; }
        }

        /// <summary>
        /// 公告内容
        /// </summary>
        [Column]
        public string NoticeContent
        {
            get { return noticeContent; }

            set { noticeContent = value; }
        }

        /// <summary>
        /// 公告静态页面路径
        /// </summary>
        [Column]
        public string NoticePath
        {
            get { return noticePath; }

            set { noticePath = value; }
        }

        /// <summary>
        /// 公告外链
        /// </summary>
        [Column]
        public string NoticeExternalLink
        {
            get { return noticeExternalLink; }

            set { noticeExternalLink = value; }
        }

        /// <summary>
        /// 公告图片路径
        /// </summary>
        [Column]
        public string NoticeImagePath
        {
            get { return noticeImagePath; }

            set { noticeImagePath = value; }
        }

        /// <summary>
        /// 公告自定义访问路径
        /// </summary>
        [Column]
        public string NoticeCustomPath
        {
            get { return noticeCustomPath; }

            set { noticeCustomPath = value; }
        }

        /// <summary>
        /// 公告访问量
        /// </summary>
        [Column]
        public int NoticeVisitCount
        {
            get { return noticeVisitCount; }

            set { noticeVisitCount = value; }
        }

        /// <summary>
        /// 公告更新时间
        /// </summary>
        [Column]
        public DateTime UpdateTime
        {
            get { return updateTime; }

            set { updateTime = value; }
        }

        /// <summary>
        /// 公告更新人
        /// </summary>
        [Column]
        public string UpdateBy
        {
            get { return updateBy; }

            set { updateBy = value; }
        }

        /// <summary>
        /// 公告发布时间
        /// </summary>
        [Column]
        public DateTime NoticePublishTime
        {
            get { return noticePublishTime; }

            set { noticePublishTime = value; }
        }

        /// <summary>
        /// 公告发布人
        /// </summary>
        [Column]
        public string NoticePublisher
        {
            get { return noticePublisher; }

            set { noticePublisher = value; }
        }

        /// <summary>
        /// 公告审核状态
        /// </summary>
        [Column]
        public bool CheckStatus
        {
            get { return checkStatus; }

            set { checkStatus = value; }
        }

        /// <summary>
        /// 是否删除
        /// </summary>
        [Column]
        public bool IsDeleted
        {
            get { return isDeleted; }

            set { isDeleted = value; }
        }
    }
}