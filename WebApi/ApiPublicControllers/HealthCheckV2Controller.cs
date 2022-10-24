using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.ApiAuthentication;
using static WebApi.Helpers.CustomHttpResult;
using WebApi.Models;
using Library;
using WebApi.BALs;
using WebApi.Helpers;
using static Library.CommomExtensions;
using static WebApi.Helpers.WebApiLogging;

namespace WebApi.ApiPublicControllers
{
    [BasicAuthentication]
    public class HealthCheckV2Controller : ApiController
    {
        private const string BASE_URL = "api/public/v2/health_check/";
        private readonly string CURRENT_TIME = GetTime();
        private ApiResponseBody API_RESPONSE_BODY = new ApiResponseBody();
        private HealthCheckV2BAL HEALTH_CHECK_V2_BAL = new HealthCheckV2BAL();

        [Route(BASE_URL + "status")]
        [HttpGet]
        public HttpResponseMessage Status()
        {
            ApiStart(nameof(HealthCheckV2Controller), nameof(Status), CURRENT_TIME);

            API_RESPONSE_BODY = HEALTH_CHECK_V2_BAL.Status();

            ApiEnd(nameof(HealthCheckV2Controller), nameof(Status), CURRENT_TIME, API_RESPONSE_BODY);

            return Request.ConstructHttpResult(API_RESPONSE_BODY);
        }
    }
}
