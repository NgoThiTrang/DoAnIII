using DoAn.Service;
using System.Web.Http;
using Web.Infrastructure.Core;

namespace Web.Api
{
    [Authorize]
    [RoutePrefix("api/home")]
    public class HomeController : ApiControllerBase
    {
        private IExceptionLogService _errorService;

        public HomeController(IExceptionLogService errorService) : base(errorService)
        {
            this._errorService = errorService;
        }
        
        [Route("TestMethod")]
        [HttpGet]
        public string TestMethod()
        {
            return "Hello, Sanslab Member. ";
        }
    }
}