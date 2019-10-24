using AutoMapper;
using DoAn.App_Start;
using DoAn.Data.Model;
using DoAn.Service;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Infrastructure.Core;
using Web.Models;

namespace Sanslab.Web.Api
{
    [RoutePrefix("api/applicationRole")]
   // [Authorize(Roles = "SystemManagement")]
    public class ApplicationRoleController : ApiControllerBase
    {
        private IApplicationRoleService _appRoleService;
        private ApplicationUserManager _userManager;

        public ApplicationRoleController(IExceptionLogService errorService,
            IApplicationRoleService appRoleService, ApplicationUserManager userManager) : base(errorService)
        {
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
                var model = _appRoleService.GetAll();
                IEnumerable<ApplicationRoleViewModel> modelVm = Mapper.Map<IEnumerable<ApplicationRole>, IEnumerable<ApplicationRoleViewModel>>(model);
                response = request.CreateResponse(HttpStatusCode.OK, modelVm);

                return response;
            });
        }
        [HttpDelete]
        [Route("delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, string id)
        {
            _appRoleService.Delete(id);
            _appRoleService.Save();
            return request.CreateResponse(HttpStatusCode.OK, id);
        }
        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, ApplicationRoleViewModel applicationRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var newAppRole = new ApplicationRole();
                if (String.IsNullOrEmpty(applicationRoleViewModel.Id))
                {
                    newAppRole.Id = Guid.NewGuid().ToString();
                    newAppRole.Name = applicationRoleViewModel.Name;
                    newAppRole.Description = applicationRoleViewModel.Description;
                    try
                    {
                        _appRoleService.Add(newAppRole);
                        _appRoleService.Save();
                        return request.CreateResponse(HttpStatusCode.OK, newAppRole);
                    }
                    catch (Exception dex)
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                    }
                }
                else
                {
                    newAppRole = _appRoleService.GetDetail(applicationRoleViewModel.Id);
                    newAppRole.Name = applicationRoleViewModel.Name;
                    newAppRole.Description = applicationRoleViewModel.Description;
                    try
                    {
                        _appRoleService.Add(newAppRole);
                        _appRoleService.Save();
                        return request.CreateResponse(HttpStatusCode.OK, newAppRole);
                    }
                    catch (Exception dex)
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
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