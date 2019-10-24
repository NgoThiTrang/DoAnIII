using DoAn.Service;
using System.Web.Mvc;
using Web.Infrastructure.Core;

namespace Web.Api
{
    [RoutePrefix("api/home")]
    [Authorize]
    public class HomeController : ApiControllerBase
    {
        private IExceptionLogService _errorService;

        public HomeController(IExceptionLogService errorService) : base(errorService)
        {
            this._errorService = errorService;
        }

        [HttpGet]
        [Route("TestMethod")]
        public string TestMethod()
        {
            return "Hello, Sanslab Member. ";
        }
    }
}