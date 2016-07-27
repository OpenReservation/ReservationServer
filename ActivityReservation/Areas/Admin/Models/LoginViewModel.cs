using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ActivityReservation.Areas.Admin.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="用户名不能为空")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度有误，密码长度应在{1}到{0}之间")]
        [Display(Name ="用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="密码不能为空")]
        [StringLength(20,MinimumLength =6,ErrorMessage ="密码长度有误，密码长度应在{1}到{0}之间")]
        [Display(Name ="密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Display(Name ="记住我")]
        public bool RememberMe { get; set; }
    }
}