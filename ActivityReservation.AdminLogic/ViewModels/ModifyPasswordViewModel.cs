using System.ComponentModel.DataAnnotations;

namespace ActivityReservation.AdminLogic.ViewModels
{
    /// <summary>
    /// 修改密码ViewModel
    /// </summary>
    public class ModifyPasswordViewModel
    {
        /// <summary>
        /// 原密码
        /// </summary>
        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string OldPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string NewPassword { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required]
        [Compare("NewPassword", ErrorMessage = "确认密码和密码不一致")]
        public string ConfirmPassword { get; set; }
    }
}