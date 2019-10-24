using DoAn.Service;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Infrastructure.Core;
using Web.Models;

namespace Web.MobileApi
{
    [Authorize]
    [RoutePrefix("api/datapackage")]
    public class DataPackageController : ApiControllerBase
    {
        private IExceptionLogService _errorService;
        private IDataPackageService _dataPackageService;
        private IWarningProfileService _warningProfileService;

        public DataPackageController(IExceptionLogService errorService, IDataPackageService dataPackageService, IWarningProfileService warningProfileService) : base(errorService)
        {
            _errorService = errorService;
            _dataPackageService = dataPackageService;
            _warningProfileService = warningProfileService;
        }

        [Route("report")]
        [HttpGet]
        public HttpResponseMessage Report(HttpRequestMessage request, DateTime date, int deviceId, int paramId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var lst = BindingParam.Instance().List(deviceId, date, paramId);
                response = request.CreateResponse(HttpStatusCode.OK, lst);
                return response;
            });
        }

        //[Route("getcauhinhbyuser")]
        //[HttpGet]
        //[Authorize(Roles = "DistrictManagement")]
        //public HttpResponseMessage GetByUserId(HttpRequestMessage request, string userid)
        //{
        //    return CreateHttpResponse(request, () =>
        //    {
        //        HttpResponseMessage response = null;
        //        var lstCauhinh = _warningProfileService.GetById(userid);
        //        response = request.CreateResponse(HttpStatusCode.OK, lstCauhinh);
        //        return response;
        //    });
        //}

        [Route("getparamnewest")]
        [HttpGet]
        public HttpResponseMessage GetParamnewest(HttpRequestMessage request, int deviceId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = _dataPackageService.GetParamnewest(deviceId);
                response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }
    }
}