using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityReservation.Models
{
    [Table("tabNotice")]
    public class Notice
    {
        /// <summary>
        /// 公告id
        /// </summary>
        [Key]
        [Column]
        public Guid NoticeId { get; set; }

        /// <summary>
        /// 公告标题
        /// </summary>
        [Column]
        public string NoticeTitle { get; set; }

        /// <summary>
        /// 公告简介
        /// </summary>
        [Column]
        public string NoticeDesc { get; set; }

        /// <summary>
        /// 公告内容
        /// </summary>
        [Column]
        public string NoticeContent { get; set; }

        /// <summary>
        /// 公告静态页面路径
        /// </summary>
        [Column]
        public string NoticePath { get; set; }

        /// <summary>
        /// 公告外链
        /// </summary>
        [Column]
        public string NoticeExternalLink { get; set; }

        /// <summary>
        /// 公告图片路径
        /// </summary>
        [Column]
        public string NoticeImagePath { get; set; }

        /// <summary>
        /// 公告自定义访问路径
        /// </summary>
        [Column]
        public string NoticeCustomPath { get; set; }

        /// <summary>
        /// 公告访问量
        /// </summary>
        [Column]
        public int NoticeVisitCount { get; set; }

        /// <summary>
        /// 公告更新时间
        /// </summary>
        [Column]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 公告更新人
        /// </summary>
        [Column]
        public string UpdateBy { get; set; }

        /// <summary>
        /// 公告发布时间
        /// </summary>
        [Column]
        public DateTime NoticePublishTime { get; set; }

        /// <summary>
        /// 公告发布人
        /// </summary>
        [Column]
        public string NoticePublisher { get; set; }

        /// <summary>
        /// 公告审核状态
        /// </summary>
        [Column]
        public bool CheckStatus { get; set; } = false;

        /// <summary>
        /// 是否删除
        /// </summary>
        [Column]
        public bool IsDeleted { get; set; } = false;
    }
}
