using DoAn.App_Start;
using DoAn.Data.Model;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace DoAn.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        //private IDeviceService _deviceService;
        //private ILakeService _lakeService;

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager/*, IDeviceService deviceService, ILakeService lakeService*/)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            //_deviceService = deviceService;
            //_lakeService = lakeService;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public AccountController()
        {
        }

        // GET: Account
        [AllowAnonymous]
        public ActionResult Login(string returnUrl) // e để authorize vao controller action nào nếu chưa đăng nhập thì thằng identity n tự redirect về trang đăng nhâp kèm returnUrl là cai action đấy
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);  // ????? thây chưa vâng đăng nhập thành công thì chuyển hướng đến cái retunUrl kia bỏ cái này đi đăng nhập thanh công lại quay lại trang login làm gì nữa
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác");
                    return View(model);
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult LogOut()

        //{
        //    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        //    return RedirectToAction("Index", "Home");
        //}

        //[Authorize]
        //[HttpGet]
        //public async Task<ActionResult> AccountInfo()
        //{
        //    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        //    var model = Mapper.Map<ApplicationUser, ApplicationUserViewModel>(user);
        //    return View(model);
        //}

        //[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> AccountInfo(ApplicationUserViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        HttpPostedFileBase file = HttpContext.Request.Files[0];
        //        var user = await UserManager.FindByIdAsync(model.Id);
        //        var tempEmail = await UserManager.FindByEmailAsync(model.Email);
        //        var tempName = await UserManager.FindByNameAsync(model.UserName);
        //        if (tempEmail != null && user != tempEmail)
        //        {
        //            ModelState.AddModelError("", "Email đã tồn tại");
        //            return View(model);
        //        }
        //        if (tempName != null && user != tempName)
        //        {
        //            ModelState.AddModelError("", "Tên tài khoản đã tồn tại");
        //            return View(model);
        //        }
        //        if (file.ContentLength > 0)
        //        {
        //            using (MemoryStream ms = new MemoryStream())
        //            {
        //                file.InputStream.CopyTo(ms);
        //                byte[] imageBytes = ms.GetBuffer();
        //                //Save the Byte Array as Image File.
        //                string file_Name = StringHelper.ToUnsignString(model.UserName) + "-" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".png";
        //                string filePath = Path.Combine(Server.MapPath("~/Content/admin/img/avatar"), file_Name);
        //                System.IO.File.WriteAllBytes(filePath, imageBytes);
        //                if (!String.IsNullOrEmpty(model.Avatar))
        //                {
        //                    if (Directory.Exists(Path.GetDirectoryName(Path.Combine(Server.MapPath(model.Avatar)))))
        //                    {
        //                        System.IO.File.Delete(model.Avatar);
        //                    }
        //                }
        //                model.Avatar = "~/Content/admin/img/avatar/" + file_Name;
        //            }
        //        }
        //        user.Email = model.Email;
        //        user.UserName = model.UserName;
        //        user.PhoneNumber = model.PhoneNumber;
        //        user.FullName = model.FullName;
        //        user.Address = model.Address;
        //        user.Job = model.Job;
        //        user.Avatar = model.Avatar;
        //        user.Gender = model.Gender;
        //        await _userManager.UpdateAsync(user);
        //        ViewBag.Message = "Cập nhật tài khoản thành công";
        //        return View(model);
        //    }
        //    return View(model);
        //}

        //[Authorize]
        //public ActionResult ChangePass()
        //{
        //    var model = new ChangePasswordModel();
        //    return View(model);
        //}

        //[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ChangePass(ChangePasswordModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        //        var testuser = await UserManager.FindAsync(user.UserName, model.OldPassword);
        //        if (testuser != null)
        //        {
        //            try
        //            {
        //                user.PasswordHash = UserManager.PasswordHasher.HashPassword(model.NewPassword);
        //                var result = await UserManager.UpdateAsync(user);
        //                if (result.Succeeded)
        //                {
        //                    ViewBag.Success = "Đổi mật khẩu thành công";
        //                    return View(model);
        //                }
        //                else
        //                    ModelState.AddModelError("", "Đổi mật khẩu thất bại");
        //            }
        //            catch (Exception ex)
        //            {
        //                ModelState.AddModelError("", ex.Message);
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Mật khẩu cũ không đúng");
        //            return View(model);
        //        }
        //    }
        //    return View(model);
        //}

        //[AllowAnonymous]
        //public ActionResult ForgotPassword()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ForgotPassword(ForgotPasswordModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByEmailAsync(model.Email);
        //        if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
        //        {
        //            ViewBag.errorMessage = "Hãy đảm bảo rằng đây chính xác là email của bạn.";
        //            return View(model);
        //        }
        //        var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("BKRES-LORA");
        //        UserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(provider.Create("ASP.NET Identity"));
        //        UserManager.EmailService = new EmailService();
        //        var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
        //        var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //        await UserManager.SendEmailAsync(user.Id, "Đặt lại mật khẩu", "Vui lòng xác nhận đặt lại mật khẩu bằng việc click vào <a href=\"" + callbackUrl + "\">liên kết</a> này ");
        //        return View("ForgotPasswordConfirmation");
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}
        //[AllowAnonymous]
        //public ActionResult ForgotPasswordConfirmation()
        //{
        //    return View();
        //}

        //[AllowAnonymous]
        //public ActionResult ResetPassword(string userId, string code)
        //{
        //    return code == null ? View("Error") : View();
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ResetPassword(ResetPasswordModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    var user = await UserManager.FindByIdAsync(model.UserId);
        //    if (user == null)
        //    {
        //        // Don't reveal that the user does not exist
        //        return RedirectToAction("ResetPasswordConfirmation", "Account");
        //    }
        //    var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("BKRES-LORA");
        //    UserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(provider.Create("ASP.NET Identity"));
        //    var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction("ResetPasswordConfirmation", "Account");
        //    }
        //    AddErrors(result);
        //    return View();
        //}
        //[AllowAnonymous]
        //public ActionResult ResetPasswordConfirmation()
        //{
        //    return View();
        //}
        //private void AddErrors(IdentityResult result)
        //{
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError("", error);
        //    }
        //}

        //[Authorize(Roles = "HoDanManagement")]
        //public async Task<ActionResult> GetDevices()
        //{
        //    ApplicationUser user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
        //    var lstDevice = _deviceService.GetAllByHoDan(user.HoDanId);
        //    var model = Mapper.Map<IEnumerable<Device>, IEnumerable<DeviceModel>>(lstDevice);
        //    foreach (var item in model)
        //    {
        //        item.LakeName = _lakeService.GetById(item.LakeId).Name;
        //    }
        //    return View(model);
        //}
        //[Authorize(Roles = "HoDanManagement")]
        //public async Task<ActionResult> ChangeDevice(int Id)
        //{
        //    ApplicationUser user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
        //    var device = _deviceService.GetById(Id);
        //    var model = Mapper.Map<Device, DeviceModel>(device);
        //    var lstLake = _lakeService.GetListByHoDanId(user.HoDanId);
        //    ViewBag.Lakes = new List<SelectListItem>(lstLake.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }));
        //    return View(model);
        //}

        //[Authorize(Roles = "HoDanManagement")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ChangeDevice(DeviceModel model)
        //{
        //    ApplicationUser user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
        //    var lstLake = _lakeService.GetListByHoDanId(user.HoDanId);
        //    ViewBag.Lakes = new List<SelectListItem>(lstLake.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }));
        //    if (ModelState.IsValid)
        //    {
        //        var device = _deviceService.GetById(model.Id);
        //        device.LakeId = model.LakeId;
        //        device.Name = model.Name;
        //        device.WarningMail = model.WarningMail;
        //        device.WarningPhoneNumber = model.WarningPhoneNumber;
        //        _deviceService.Update(device);
        //        _deviceService.Save();
        //        return RedirectToAction("GetDevices");
        //    }
        //    return View(model);
        //}
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion Helpers
    }
}