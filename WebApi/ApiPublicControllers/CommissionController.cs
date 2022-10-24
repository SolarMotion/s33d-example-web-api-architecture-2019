using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.ApiAuthentication;
using WebApi.BALs;
using WebApi.Models;
using static Library.CommomExtensions;
using static WebApi.Helpers.WebApiLogging;
using static WebApi.Helpers.CustomHttpResult;

namespace WebApi.ApiPublicControllers
{
    [BasicAuthentication]
    public class CommissionController : ApiController
    {
        private const string BASE_URL = "api/public/v1/commission/";
        private readonly string CURRENT_TIME = GetTime();
        private ApiResponseBody API_RESPONSE_BODY = new ApiResponseBody();
        private CommissionBAL COMMISSION_BAL = new CommissionBAL();

        [Route(BASE_URL + "list")]
        [HttpPost]
        public HttpResponseMessage List(CommissionListRequest request)
        {
            ApiStart(nameof(CommissionController), nameof(List), CURRENT_TIME);

            API_RESPONSE_BODY = COMMISSION_BAL.List(request);

            ApiEnd(nameof(CommissionController), nameof(List), CURRENT_TIME, API_RESPONSE_BODY);

            return Request.ConstructHttpResult(API_RESPONSE_BODY);
        }

        [Route(BASE_URL + "fixed_rate_commission_by_amount")]
        [HttpPost]
        public HttpResponseMessage FixedRateCommissionByAmount(FixedRateCommissionByAmountRequest request)
        {
            ApiStart(nameof(CommissionController), nameof(FixedRateCommissionByAmount), CURRENT_TIME);

            API_RESPONSE_BODY = COMMISSION_BAL.FixedRateCommissionByAmount(request);

            ApiEnd(nameof(CommissionController), nameof(FixedRateCommissionByAmount), CURRENT_TIME, API_RESPONSE_BODY);

            return Request.ConstructHttpResult(API_RESPONSE_BODY);
        }

        [Route(BASE_URL + "fixed_rate_commission_by_percentage")]
        [HttpPost]
        public HttpResponseMessage FixedRateCommissionByPercentage(FixedRateCommissionByPercentageRequest request)
        {
            ApiStart(nameof(CommissionController), nameof(FixedRateCommissionByPercentage), CURRENT_TIME);

            API_RESPONSE_BODY = COMMISSION_BAL.FixedRateCommissionByPercentage(request);

            ApiEnd(nameof(CommissionController), nameof(FixedRateCommissionByPercentage), CURRENT_TIME, API_RESPONSE_BODY);

            return Request.ConstructHttpResult(API_RESPONSE_BODY);
        }

        [Route(BASE_URL + "tiered_rate_commission_by_amount")]
        [HttpPost]
        public HttpResponseMessage TieredRateCommissionByAmount(TieredRateCommissionByAmountRequest request)
        {
            ApiStart(nameof(CommissionController), nameof(TieredRateCommissionByAmount), CURRENT_TIME);

            API_RESPONSE_BODY = COMMISSION_BAL.TieredRateCommissionByAmount(request);

            ApiEnd(nameof(CommissionController), nameof(TieredRateCommissionByAmount), CURRENT_TIME, API_RESPONSE_BODY);

            return Request.ConstructHttpResult(API_RESPONSE_BODY);
        }

        [Route(BASE_URL + "tiered_rate_commission_by_percentage")]
        [HttpPost]
        public HttpResponseMessage TieredRateCommissionByPercentage(TieredRateCommissionByPercentageRequest request)
        {
            ApiStart(nameof(CommissionController), nameof(TieredRateCommissionByPercentage), CURRENT_TIME);

            API_RESPONSE_BODY = COMMISSION_BAL.TieredRateCommissionByPercentage(request);

            ApiEnd(nameof(CommissionController), nameof(TieredRateCommissionByPercentage), CURRENT_TIME, API_RESPONSE_BODY);

            return Request.ConstructHttpResult(API_RESPONSE_BODY);
        }

        [Route(BASE_URL + "tiered_rate_commission_by_distance_amount")]
        [HttpPost]
        public HttpResponseMessage TieredRateCommissionByDistanceAmount(TieredRateCommissionByDistanceAmountRequest request)
        {
            ApiStart(nameof(CommissionController), nameof(TieredRateCommissionByDistanceAmount), CURRENT_TIME);

            API_RESPONSE_BODY = COMMISSION_BAL.TieredRateCommissionByDistanceAmount(request);

            ApiEnd(nameof(CommissionController), nameof(TieredRateCommissionByDistanceAmount), CURRENT_TIME, API_RESPONSE_BODY);

            return Request.ConstructHttpResult(API_RESPONSE_BODY);
        }

        [Route(BASE_URL + "tiered_rate_commission_by_percentage")]
        [HttpPost]
        public HttpResponseMessage TieredRateCommissionByDistancePercentage(TieredRateCommissionByDistancePercentageRequest request)
        {
            ApiStart(nameof(CommissionController), nameof(TieredRateCommissionByDistancePercentage), CURRENT_TIME);

            API_RESPONSE_BODY = COMMISSION_BAL.TieredRateCommissionByDistancePercentage(request);

            ApiEnd(nameof(CommissionController), nameof(TieredRateCommissionByDistancePercentage), CURRENT_TIME, API_RESPONSE_BODY);

            return Request.ConstructHttpResult(API_RESPONSE_BODY);
        }

        [Route(BASE_URL + "payout")]
        [HttpPost]
        public HttpResponseMessage Payout(CommissionPayoutRequest request)
        {
            ApiStart(nameof(CommissionController), nameof(Payout), CURRENT_TIME);

            API_RESPONSE_BODY = COMMISSION_BAL.Payout(request);

            ApiEnd(nameof(CommissionController), nameof(Payout), CURRENT_TIME, API_RESPONSE_BODY);

            return Request.ConstructHttpResult(API_RESPONSE_BODY);
        }
    }
}
