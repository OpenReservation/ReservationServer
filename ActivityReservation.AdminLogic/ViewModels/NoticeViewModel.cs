using System.ComponentModel.DataAnnotations;

namespace ActivityReservation.AdminLogic.ViewModels
{
    /// <summary>
    /// 公告view model
    /// </summary>
    public class NoticeViewModel
    {
        /// <summary>
        /// 公告标题
        /// </summary>
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; }

        /// <summary>
        /// 公告内容
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 自定义路径
        /// </summary>
        public string CustomPath { get; set; }
    }
}
