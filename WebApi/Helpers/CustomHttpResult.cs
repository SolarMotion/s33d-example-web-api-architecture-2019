using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using WebApi.Models;
using static Library.CommomExtensions;

namespace WebApi.Helpers
{
    public static class CustomHttpResult
    {
        public static HttpResponseMessage ConstructHttpResult<T>(this T target, ApiResponseBody baseResponse) where T : HttpRequestMessage
        {
            return target.CreateResponse(baseResponse.HttpStatusCode, baseResponse);
        }
    }
}