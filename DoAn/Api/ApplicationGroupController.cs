using AutoMapper;
using DoAn.App_Start;
using DoAn.Data.Model;
using DoAn.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Web.Infrastructure.Core;
using Web.Models;

namespace Web.Api
{
    [RoutePrefix("api/applicationGroup")]
  //  [Authorize(Roles = "SystemManagement")]
    public class ApplicationGroupController : ApiControllerBase
    {
        private IApplicationGroupService _appGroupService;
        private IApplicationRoleService _appRoleService;
        private ApplicationUserManager _userManager;

        public ApplicationGroupController(IExceptionLogService errorService,
            IApplicationRoleService appRoleService,
            ApplicationUserManager userManager,
            IApplicationGroupService appGroupService) : base(errorService)
        {
            _appGroupService = appGroupService;
            _appRoleService = appRoleService;
            _userManager = userManager;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = _appGroupService.GetAll();
                IEnumerable<ApplicationGroupViewModel> modelVm = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(model);
                foreach (var item in modelVm)
                {
                    item.Roles = Mapper.Map<IEnumerable<ApplicationRole>, IEnumerable<ApplicationRoleViewModel>>(_appRoleService.GetListRoleByGroupId(item.Id)).ToList();
                }
                response = request.CreateResponse(HttpStatusCode.OK, modelVm);
                return response;
            });
        }
        [HttpDelete]
        [Route("delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            var appGroup = _appGroupService.Delete(id);
            _appGroupService.Save();
            return request.CreateResponse(HttpStatusCode.OK, appGroup);
        }
        [HttpPost]
        [Route("update")]
        public async Task<HttpResponseMessage> Update(HttpRequestMessage request, ApplicationGroupViewModel appGroupViewModel)
        {
            if (ModelState.IsValid)
            {
                var newAppGroup = new ApplicationGroup();
                if (appGroupViewModel.Id == 0)
                {
                    newAppGroup.Name = appGroupViewModel.Name;
                    newAppGroup.Description = appGroupViewModel.Description;
                    try
                    {
                        var appGroup = _appGroupService.Add(newAppGroup);
                        _appGroupService.Save();

                        //save group
                        var listRoleGroup = new List<ApplicationRoleGroup>();
                        foreach (var role in appGroupViewModel.Roles)
                        {
                            listRoleGroup.Add(new ApplicationRoleGroup()
                            {
                                GroupId = appGroup.Id,
                                RoleId = role.Id
                            });
                        }
                        _appRoleService.AddRolesToGroup(listRoleGroup, appGroup.Id);
                        _appRoleService.Save();

                        return request.CreateResponse(HttpStatusCode.OK, appGroupViewModel);
                    }
                    catch (Exception ex)
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                    }
                }
                else
                {
                    newAppGroup = _appGroupService.GetDetail(appGroupViewModel.Id);
                    newAppGroup.Name = appGroupViewModel.Name;
                    newAppGroup.Description = appGroupViewModel.Description;
                    try
                    {
                        _appGroupService.Update(newAppGroup);
                        //_appGroupService.Save();
                        //save group
                        var listRoleGroup = new List<ApplicationRoleGroup>();
                        foreach (var role in appGroupViewModel.Roles)
                        {
                            listRoleGroup.Add(new ApplicationRoleGroup()
                            {
                                GroupId = newAppGroup.Id,
                                RoleId = role.Id
                            });
                        }
                        _appRoleService.AddRolesToGroup(listRoleGroup, newAppGroup.Id);
                        _appRoleService.Save();
                        // add role to user
                        var listRole = _appRoleService.GetListRoleByGroupId(newAppGroup.Id);
                        var listUserInGroup = _appGroupService.GetListUserByGroupId(newAppGroup.Id);
                        foreach (var user in listUserInGroup)
                        {
                            var listRoleName = listRole.Select(x => x.Name).ToArray();
                            foreach (var roleName in listRoleName)
                            {
                                await _userManager.RemoveFromRoleAsync(user.Id, roleName);
                                await _userManager.AddToRoleAsync(user.Id, roleName);
                            }
                        }
                        return request.CreateResponse(HttpStatusCode.OK, appGroupViewModel);
                    }
                    catch (Exception ex)
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                    }
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
    }
}