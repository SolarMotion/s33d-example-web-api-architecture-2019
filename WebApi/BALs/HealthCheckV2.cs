using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;
using static WebApi.Helpers.CustomApiResponse;
using static WebApi.Helpers.Log;

namespace WebApi.BALs
{
    public class HealthCheckV2BAL
    {
        private readonly GlobalVariable GLOBE = new GlobalVariable() { VERSION = ApiVersion.Two.GetEnumDescription() };

        public ApiResponseBody Status()
        {
            try
            {
                var response = new HealthCheckStatusResponse() { Status = "System is running." };

                return ConstructSuccessResponse(response, GLOBE);
            }
            catch (Exception ex)
            {
                PublicError(ex);
                return ConstructInternalServerErrorResponse(new HealthCheckStatusResponse(), GLOBE);
            }
        }
    }
}