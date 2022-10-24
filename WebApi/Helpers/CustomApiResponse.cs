using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;
using System.Net;

namespace WebApi.Helpers
{
    public static class CustomApiResponse
    {
        public static ApiResponseBody ConstructSuccessResponse(object response, GlobalVariable globalVariable, string message = null)
        {
            return new ApiResponseBody()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Version = globalVariable.VERSION,
                ResponseCode = (int)HttpStatusCode.OK,
                ResponseMessage = message ?? ApiResponseMessage.Success.GetEnumDescription(),
                Data = response
            };
        }

        public static ApiResponseBody ConstructBadRequestResponse(object response, GlobalVariable globalVariable, string message = null)
        {
            return new ApiResponseBody()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Version = globalVariable.VERSION,
                ResponseCode = (int)HttpStatusCode.BadRequest,
                ResponseMessage = message ?? ApiResponseMessage.BadRequest.GetEnumDescription(),
                Data = response
            };
        }

        public static ApiResponseBody ConstructNotImplementedResponse(object response, GlobalVariable globalVariable, string message = null)
        {
            return new ApiResponseBody()
            {
                HttpStatusCode = HttpStatusCode.NotImplemented,
                Version = globalVariable.VERSION,
                ResponseCode = (int)HttpStatusCode.NotImplemented,
                ResponseMessage = message ?? ApiResponseMessage.NotImplemented.GetEnumDescription(),
                Data = response
            };
        }

        public static ApiResponseBody ConstructServiceUnavailableResponse(object response, GlobalVariable globalVariable, string message = null)
        {
            return new ApiResponseBody()
            {
                HttpStatusCode = HttpStatusCode.ServiceUnavailable,
                Version = globalVariable.VERSION,
                ResponseCode = (int)HttpStatusCode.ServiceUnavailable,
                ResponseMessage = message ?? ApiResponseMessage.ServiceUnavailable.GetEnumDescription(),
                Data = response
            };
        }

        public static ApiResponseBody ConstructInternalServerErrorResponse(object response, GlobalVariable globalVariable)
        {
            return new ApiResponseBody()
            {
                HttpStatusCode = HttpStatusCode.InternalServerError,
                Version = globalVariable.VERSION,
                ResponseCode = (int)HttpStatusCode.InternalServerError,
                ResponseMessage = ApiResponseMessage.InternalServerError.GetEnumDescription(),
                Data = response
            };
        }
    }
}