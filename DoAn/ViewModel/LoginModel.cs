using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class LoginModel
    {

        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Bạn cần nhập tài khoản")]
        public string UserName { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Bạn cần nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}