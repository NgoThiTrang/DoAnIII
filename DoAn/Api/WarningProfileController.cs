using AutoMapper;
using DoAn.App_Start;
using DoAn.Data.Model;
using DoAn.Service;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using Web.Infrastructure.Core;
using Web.Models;

namespace DoAn.Api
{
    [RoutePrefix("api/warningprofile")]
    public class WarningProfileController : ApiControllerBase
    {
        private IWarningProfileService _warningProfileService;
        private ApplicationUserManager _userManager;
        private IExceptionLogService _errorService;

        public WarningProfileController(IExceptionLogService errorService, IWarningProfileService warningProfileService, ApplicationUserManager userManager) : base(errorService)
        {
            _warningProfileService = warningProfileService;
            _userManager = userManager;
            _errorService = errorService;
        }

        // GET: CauHinhCanhBao
        [HttpGet]
        [Route("getwarningprofile")]
        public HttpResponseMessage GetWarningProfile(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
              
                var lstCauhinh = _warningProfileService.GetAll();
                response = request.CreateResponse(HttpStatusCode.OK, lstCauhinh);
                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int Id)
        {
            _warningProfileService.Delete(Id);
            _warningProfileService.Save();
            return request.CreateResponse(HttpStatusCode.OK, Id);
        }

        [Route("update")]
        [HttpPost]
        public HttpResponseMessage UpdateWarningProfile(HttpRequestMessage request, WarningProfileViewModel modelVm)
        {
            if (!ModelState.IsValid)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            else
            {
                var model = new WarningProfile();
                if (modelVm.Id == 0)
                {
                    model.Name = modelVm.Name;
                    model.Low_Thres = modelVm.Low_Thres;
                    model.Up_Thres = modelVm.Up_Thres;
                    model.ProcessTimeOut = modelVm.ProcessTimeOut;
                    model.WarningContent = modelVm.WarningContent;
                    model.PropertiesName = model.PropertiesName;
                    _warningProfileService.Insert(model);
                    _warningProfileService.Save();
                }
                else
                {
                    model = _warningProfileService.GetById(modelVm.Id);
                    model.Name = modelVm.Name;
                    model.Low_Thres = modelVm.Low_Thres;
                    model.Up_Thres = modelVm.Up_Thres;
                    model.ProcessTimeOut = modelVm.ProcessTimeOut;
                    model.WarningContent = modelVm.WarningContent;
                    model.PropertiesName = model.PropertiesName;
                    _warningProfileService.Update(model);
                    _warningProfileService.Save();
                }
                var responseData = Mapper.Map<WarningProfile, WarningProfileViewModel>(model);
                responseData.Name = _warningProfileService.GetById(model.Id).Name;
                return request.CreateResponse(HttpStatusCode.Created, responseData);
            }
        }
    }
}