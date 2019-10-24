using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class ChangePasswordModel
    {
        [Display(Name = "Mật khẩu cũ")]
        public string OldPassword { get; set; }

        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [Display(Name = "Xác nhận mật khẩu ")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }
    }
}