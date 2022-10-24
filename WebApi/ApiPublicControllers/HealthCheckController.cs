using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.ApiAuthentication;
using WebApi.Models;
using static WebApi.Helpers.CustomHttpResult;
using WebApi.BALs;
using static WebApi.Helpers.WebApiLogging;
using static Library.CommomExtensions;

namespace WebApi.ApiPublicControllers
{
    [BasicAuthentication]
    public class HealthCheckController : ApiController
    {
        private const string BASE_URL = "api/public/v1/health_check/";
        private readonly string CURRENT_TIME = GetTime();
        private ApiResponseBody API_RESPONSE_BODY = new ApiResponseBody();
        private HealthCheckBAL HEALTH_CHECK_BAL = new HealthCheckBAL();

        [Route(BASE_URL + "status")]
        [HttpGet]
        public HttpResponseMessage Status()
        {
            ApiStart(nameof(HealthCheckController), nameof(Status), CURRENT_TIME);

            API_RESPONSE_BODY = HEALTH_CHECK_BAL.Status();

            ApiEnd(nameof(HealthCheckController), nameof(Status), CURRENT_TIME, API_RESPONSE_BODY);

            return Request.ConstructHttpResult(API_RESPONSE_BODY);
        }
    }
}
