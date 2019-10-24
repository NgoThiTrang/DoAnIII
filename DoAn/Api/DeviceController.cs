using AutoMapper;
using DoAn.App_Start;
using DoAn.Data.Model;
using DoAn.Service;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Web.Infrastructure.Core;
using Web.Models;

namespace Web.Api
{
   // [Authorize]
    [RoutePrefix("api/device")]
    public class DeviceController : ApiControllerBase
    {
        private IExceptionLogService _errorService;
        private ApplicationUserManager _userManager;
        private IApplicationRoleService _applicationRoleService;

        private IDistrictService _districtService;

        private IDeviceService _deviceService;

        public DeviceController(IDistrictService districtService, IDeviceService deviceService, ApplicationUserManager userManager, IApplicationRoleService applicationRoleService, IExceptionLogService errorService) : base(errorService)
        {
            this._errorService = errorService;

            this._districtService = districtService;

            this._userManager = userManager;
            this._applicationRoleService = applicationRoleService;

            this._deviceService = deviceService;
        }

        [Route("getall")]
        [HttpGet]
        [Authorize(Roles = "GeneralManagement")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var lstDevice = Mapper.Map<IEnumerable<Device>, IEnumerable<DeviceModel>>(_deviceService.GetAll());
                foreach (var item in lstDevice)
                {
                    item.DistrictName = _districtService.GetById(item.DistrictId).Name;
                }
                response = request.CreateResponse(HttpStatusCode.OK, lstDevice);
                return response;
            });
        }

        [Route("update")]
        [HttpPost]
        [Authorize(Roles = "GeneralManagement")]
        public HttpResponseMessage Update(HttpRequestMessage request, DeviceModel model)
        {
            if (!ModelState.IsValid)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            else
            {
                var device = new Device();
                if (model.Id == 0)
                {
                    device.Name = model.Name;
                    device.Imei = model.Imei;
                    device.DistrictId = model.DistrictId;
                    device.isActive = model.isActive;
                    device.CreatedDate = DateTime.Now;
                    device.WarningPhoneNumber = model.WarningPhoneNumber;
                    device.WarningMail = model.WarningMail;
                    _deviceService.Insert(device);
                    _deviceService.Save();
                }
                else
                {
                    device = _deviceService.GetById(model.Id);
                    device.Name = model.Name;
                    device.Imei = model.Imei;
                    device.DistrictId = model.DistrictId;
                    device.isActive = model.isActive;
                    device.WarningPhoneNumber = model.WarningPhoneNumber;
                    device.WarningMail = model.WarningMail;
                    _deviceService.Update(device);
                    _deviceService.Save();
                }
                var responseData = Mapper.Map<Device, DeviceModel>(device);
                responseData.DistrictName = _districtService.GetById(device.DistrictId).Name;
                return request.CreateResponse(HttpStatusCode.Created, responseData);
            }
        }
        [Route("delete")]
        [HttpDelete]
      
      
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            _deviceService.Delete(id);
            _deviceService.Save();
            return request.CreateResponse(HttpStatusCode.OK, id);
        }
        [Route("getbydistrictid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetByLakeId(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var lstDevice = _deviceService.GetListByDistrictId(id);
                response = request.CreateResponse(HttpStatusCode.OK, lstDevice);
                return response;
            });
        }
    }
}