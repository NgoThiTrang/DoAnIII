using AutoMapper;
using DoAn.App_Start;
using DoAn.Data.Model;
using DoAn.Service;
using DoAn.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Web.Infrastructure.Core;

namespace DoAn.Api
{
    [RoutePrefix("api/activity")]
    public class ActivityLogController : ApiControllerBase
    {
        private readonly ApplicationUser _user;
        private readonly IActivityLogService _activityLogService;
        private IExceptionLogService _errorService;

        public ActivityLogController( IActivityLogService activityLogService,IExceptionLogService errorService) : base(errorService)
        {
            this._activityLogService = activityLogService;
            this._errorService = errorService;
        }
        [Route("getpaging")]

        [HttpGet]
        public HttpResponseMessage GetAllPaging(HttpRequestMessage request, string keyword, int page, int pageSize=10)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;

                var model = _activityLogService.GetAll(keyword);
                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);
                var responseData = Mapper.Map<IEnumerable<ActivityLog>, IEnumerable<ActivityLogViewModel>>(query);
                var paginationSet = new PaginationSet<ActivityLogViewModel>() // vif hàm không thể trả về nhièu biến nên phải gom vào pagination để trả về
                {
                    Items = responseData,

                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)

                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }
    }
}
