using AutoMapper;
using DoAn.Data.Model;
using DoAn.Service;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Infrastructure.Core;
using Web.Models;

namespace Web.Api
{
   // [Authorize(Roles = "GeneralManagement")]
    [RoutePrefix("api/district")]
    public class DistrictController : ApiControllerBase
    {
        private IExceptionLogService _errorService;
        private IDistrictService _districtService;
        private IProvinceService _provinceService;
        private IDeviceService _deviceService;

        public DistrictController(IProvinceService provinceService, IDistrictService districtService, IDeviceService deviceService, IExceptionLogService errorService) : base(errorService)
        {
            this._errorService = errorService;
            this._districtService = districtService;
            this._provinceService = provinceService;
            this._deviceService = deviceService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var lstDistrict = Mapper.Map<IEnumerable<District>, IEnumerable<DistrictModel>>(_districtService.GetAll());
                foreach (var item in lstDistrict)
                {
                    item.ProvinceName = _provinceService.GetById(item.ProvinceId).Name;
                }
                response = request.CreateResponse(HttpStatusCode.OK, lstDistrict);
                return response;
            });
        }
        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Detele( HttpRequestMessage request , int id)
        {
            _districtService.Delete(id);
            _districtService.Save();
            return request.CreateResponse(HttpStatusCode.OK, id);
        }
        [Route("getbyprovinceid")]
        [HttpGet]
        public HttpResponseMessage GetListByProvinceId(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var lstDistrict = Mapper.Map<IEnumerable<District>, IEnumerable<DistrictModel>>(_districtService.GetListByProvinceId(id));
                response = request.CreateResponse(HttpStatusCode.OK, lstDistrict);
                return response;
            });
        }

        [Route("getdistrictbydeviceid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetProvinceByDistrictId(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var district = _districtService.GetById(_deviceService.GetById(id).DistrictId);

                var response = request.CreateResponse(HttpStatusCode.OK, district);

                return response;
            });
        }

        [Route("update")]
        [HttpPost]
        public HttpResponseMessage Update(HttpRequestMessage request, DistrictModel model)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var district = new District();
                    if (model.Id == 0)
                    {
                        district.Name = model.Name;
                        district.PhoneNumber = model.PhoneNumber;
                        district.ProvinceId = model.ProvinceId;
                        district.isActive = model.isActive;
                        district.CreatedDate = DateTime.Now;
                        _districtService.Insert(district);
                        _districtService.Save();
                    }
                    else
                    {
                        district = _districtService.GetById(model.Id);
                        district.Name = model.Name;
                        district.PhoneNumber = model.PhoneNumber;
                        district.ProvinceId = model.ProvinceId;
                        district.isActive = model.isActive;
                        _districtService.Update(district);
                        _districtService.Save();
                    }
                    var responseData = Mapper.Map<District, DistrictModel>(district);
                    responseData.ProvinceName = _provinceService.GetById(district.ProvinceId).Name;
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }
    }
}