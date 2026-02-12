using Telerik.Reporting.Services.AspNetCore;
using Telerik.Reporting.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace KendoMvcDemo.Controllers
{
    [ApiController]
    [Route("api/reports")]
    //[Authorize]
    public class ReportsApiController : ReportsControllerBase
    {
        public ReportsApiController(IReportServiceConfiguration config)
            : base(config)
        {
        }
    }
}
