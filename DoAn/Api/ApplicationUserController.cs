using AutoMapper;
using DoAn.App_Start;
using DoAn.Common.Common;
using DoAn.Common.Constant;
using DoAn.Data.Model;
using DoAn.Service;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TeduShop.Common.Exceptions;
using Web.Infrastructure.Core;
using Web.Models;

namespace Web.Api
{
    //[Authorize(Roles = "SystemManagement")]
    [RoutePrefix("api/user")]
    public class ApplicationUserController : ApiControllerBase
    {
        private ApplicationUserManager _userManager;
        private IApplicationGroupService _appGroupService;
        private IApplicationRoleService _appRoleService;

        public ApplicationUserController(
            IApplicationGroupService appGroupService,
            IApplicationRoleService appRoleService,
            ApplicationUserManager userManager,
            IExceptionLogService errorService)
            : base(errorService)
        {
            _appRoleService = appRoleService;
            _appGroupService = appGroupService;
            _userManager = userManager;
        }

     //   [Authorize(Roles = "SystemManagement")]
        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var lstUser = _userManager.Users;
                IEnumerable<ApplicationUserViewModel> model = Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<ApplicationUserViewModel>>(lstUser);
                foreach (var item in model)
                {
                    item.Groups = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(_appGroupService.GetListGroupByUserId(item.Id)).ToList();
                }
          
                response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }

       
        //  [Authorize(Roles = "SystemManagement")]
        [HttpPost]
        [Route("update")]
        public async Task<HttpResponseMessage> Update(HttpRequestMessage request, ApplicationUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();
                if (String.IsNullOrEmpty(model.Id))
                {
                    var tempName = await _userManager.FindByNameAsync(model.UserName);
                    var tempEmail = await _userManager.FindByEmailAsync(model.Email);
                    if (tempName != null)
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, "Tài khoản này đã tồn tại");
                    if (tempEmail != null)
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, "Email này đã tồn tại");
                    if (String.IsNullOrEmpty(model.Avatar) || model.Avatar.Contains("images_none.png"))
                    {
                        model.Avatar = "~/Content/admin/img/images_none.png";
                    }
                    else
                    {
                        byte[] imageBytes = Convert.FromBase64String(model.Avatar.Split(',')[1]);
                        //Save the Byte Array as Image File.
                        string fileName = StringHelper.ToUnsignString(model.UserName) + "-" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".png";
                        string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/admin/img/avatar"), fileName);
                        System.IO.File.WriteAllBytes(filePath, imageBytes);
                        model.Avatar = "~/Content/admin/img/avatar/" + fileName;
                    }
                    try
                    {
                        user.Id = Guid.NewGuid().ToString();
                        user.Avatar = model.Avatar;
                        user.Email = model.Email;
                        user.Address = model.Address;
                        user.FullName = model.FullName;
                    
                        user.PhoneNumber = model.PhoneNumber;
                        user.UserName = model.UserName;
                       // user.ApplicationGroupId = CommonConstants.Administrator;
                       
                        user.EmailConfirmed = true;
                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {

                            var listAppUserGroup = new List<ApplicationUserGroup>();

                            foreach (var group in model.Groups)
                            {

                                listAppUserGroup.Add(new ApplicationUserGroup()
                                {
                                    GroupId = group.Id,
                                    UserId = user.Id
                                }); 
                                //add role to user
                                var listRole = _appRoleService.GetListRoleByGroupId(group.Id);
                                foreach (var role in listRole)
                                {
                                    await _userManager.RemoveFromRoleAsync(user.Id, role.Name);
                                    await _userManager.AddToRoleAsync(user.Id, role.Name);
                                }
                            }
                            _appGroupService.AddUserToGroups(listAppUserGroup, user.Id);
                            _appGroupService.Save();
                            return request.CreateResponse(HttpStatusCode.Created, model);
                        }
                        else
                            return request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(",", result.Errors));
                    }
                    catch (Exception ex)
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                    }
                }
                else
                {
                    user = await _userManager.FindByIdAsync(model.Id);
                    ApplicationUser tempName = await _userManager.FindByNameAsync(model.UserName);
                    ApplicationUser tempEmail = await _userManager.FindByEmailAsync(model.Email);
                    if (tempName != null && tempName.UserName != model.UserName)
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, "Tài khoản này đã tồn tại");
                    if (tempEmail != null && tempEmail.Email != model.Email)
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, "Email này đã tồn tại");
                    if (String.IsNullOrEmpty(model.Avatar) || model.Avatar.Contains("images_none.png"))
                    {
                        user.Avatar = "~/Content/admin/img/avatar";
                    }
                    else if (!model.Avatar.StartsWith("~/Content") && user.Avatar != model.Avatar)
                    {
                        byte[] imageBytes = Convert.FromBase64String(model.Avatar.Split(',')[1]);
                        //Save the Byte Array as Image File.
                        string fileName = StringHelper.ToUnsignString(user.UserName) + "-" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".png";
                        string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/admin/img/avatar"), fileName);
                        System.IO.File.WriteAllBytes(filePath, imageBytes);
                        model.Avatar = "~/Content/admin/img/avatar/" + fileName;
                    }
                    user.Avatar = model.Avatar;
                    user.Email = model.Email;
                    user.FullName = model.FullName;
                    user.Address = model.Address;
               
                    user.PhoneNumber = model.PhoneNumber;
                    user.UserName = model.UserName;
                //    user.ApplicationGroupId = CommonConstants.Administrator;
                  
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var listAppUserGroup = new List<ApplicationUserGroup>();

                        foreach (var group in model.Groups)
                        {

                            listAppUserGroup.Add(new ApplicationUserGroup()
                            {
                                GroupId = group.Id,
                                UserId = user.Id // model có giá trị đâu mà gán?
                            });
                            //add role to user
                            var listRole = _appRoleService.GetListRoleByGroupId(group.Id);
                            foreach (var role in listRole)
                            {
                                await _userManager.RemoveFromRoleAsync(user.Id, role.Name);
                                await _userManager.AddToRoleAsync(user.Id, role.Name);
                            }
                        }
                        _appGroupService.AddUserToGroups(listAppUserGroup, user.Id);
                        _appGroupService.Save();
                        return request.CreateResponse(HttpStatusCode.Created, model);
                    }
                    else
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(",", result.Errors));
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
      
        [Route("delete")]
        [HttpDelete]
        //[Authorize(Roles ="DeleteUser")]
        public async Task<HttpResponseMessage> Delete(HttpRequestMessage request, string id)
        {
            var appUser = await _userManager.FindByIdAsync(id);
            var result = await _userManager.DeleteAsync(appUser);
            if (result.Succeeded)
                return request.CreateResponse(HttpStatusCode.OK, id);
            else
                return request.CreateErrorResponse(HttpStatusCode.OK, string.Join(",", result.Errors));
        }

        [Authorize]
        [Route("getcurrentuser")]
        [HttpGet]
        public HttpResponseMessage GetCurrentUser(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                ApplicationUser user = _userManager.FindById(User.Identity.GetUserId());
                var model = Mapper.Map<ApplicationUser, ApplicationUserViewModel>(user);
                response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }
    }
}