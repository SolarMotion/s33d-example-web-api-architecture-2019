using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;
using static Library.CommomExtensions;

namespace WebApi.Helpers
{
    public static class CustomApiRequest
    {
        public static T FormatRequest<T>(this T target) where T : ApiBaseRequest
        {
            //target.OrgRefNo = target.StringOrgGuid.ToGuid();

            return target;
        }

        public static void FormatRequest(ApiBaseRequest request)
        {
            //request.OrgRefNo = request.StringOrgGuid.ToGuid();
        }
    }
}