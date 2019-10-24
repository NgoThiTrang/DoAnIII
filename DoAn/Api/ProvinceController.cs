using DoAn.Data.Model;
using DoAn.Service;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Infrastructure.Core;
using Web.Models;

namespace Web.Api
{
  //  [Authorize(Roles = "GeneralManagement")]
    [RoutePrefix("api/province")]
    public class ProvinceController : ApiControllerBase
    {
        private IExceptionLogService _errorService;
        private IProvinceService _provinceService;
        private IDistrictService _districtService;

        public ProvinceController(IProvinceService provinceService, IDistrictService districtService, IExceptionLogService errorService) : base(errorService)
        {
            this._errorService = errorService;
            this._provinceService = provinceService;
            this._districtService = districtService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var lstProvince = _provinceService.GetAll();
                response = request.CreateResponse(HttpStatusCode.OK, lstProvince);
                return response;
            });
        }
        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Detele(HttpRequestMessage request, int id)
        {
            _provinceService.Delete(id);
            _provinceService.Save();
            return request.CreateResponse(HttpStatusCode.OK, id);
        }
        [Route("getprovincebydistrictid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetProvinceByDistrictId(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var provinceId = _provinceService.GetById(_districtService.GetById(id).ProvinceId).Id;

                var response = request.CreateResponse(HttpStatusCode.OK, provinceId);

                return response;
            });
        }

        [Route("update")]
        [HttpPost]
        public HttpResponseMessage Update(HttpRequestMessage request, ProvinceModel model)
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
                    var province = new Province();
                    if (model.Id == 0)
                    {
                        province.Name = model.Name;
                        province.PhoneNumber = model.PhoneNumber;
                        province.isActive = model.isActive;
                        province.CreatedDate = DateTime.Now;
                        _provinceService.Insert(province);
                        _provinceService.Save();
                    }
                    else
                    {
                        province = _provinceService.GetById(model.Id);
                        province.Name = model.Name;
                        province.PhoneNumber = model.PhoneNumber;
                        province.isActive = model.isActive;
                        _provinceService.Update(province);
                        _provinceService.Save();
                    }
                    var responseData = province;
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }
    }
}