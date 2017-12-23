using System.ComponentModel.DataAnnotations;

namespace ActivityReservation.AdminLogic.ViewModels
{
    /// <summary>
    /// 用户登录，viewmodel
    /// </summary>
    public class LoginViewModel
    {
        [Required(ErrorMessage = "用户名不能为空")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "用户名长度有误，密码长度应在{1}到{0}之间")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度有误，密码长度应在{1}到{0}之间")]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "记住我")]
        public bool RememberMe { get; set; }

        [Display(Name = "验证码")]
        [Required]
        public string Recaptcha { get; set; }

        [Required]
        [Display(Name = "验证码类型")]
        public string RecaptchaType { get; set; } = "Google";// Google, Geetest, None
    }
}