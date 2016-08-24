using System.ComponentModel.DataAnnotations;

namespace ActivityReservation.AdminLogic.ViewModels
{
    public class CreateAccountViewModel
    {
        [Required]
        [StringLength(10, MinimumLength = 2)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string UserPassword { get; set; }

        [Required]
        [Compare("UserPassword")]
        public string ConfirmPassword { get; set; }
    }
}