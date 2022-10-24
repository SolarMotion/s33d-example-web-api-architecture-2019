using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Helpers
{
    public static class WebApiLogging
    {
        public static void ApiStart(string controller, string function, string datetime, object @object)
        {
            Log.PublicInfo($"<-----Start-{datetime}----->");

            Log.PublicInfo($"Entering {controller} controller > {function}() API...");

            Log.PublicInfo($"Request JSON: {@object.ToJson()}");
        }

        public static void ApiStart(string controller, string function, string datetime, string value = null)
        {
            Log.PublicInfo($"<-----Start-{datetime}----->");

            Log.PublicInfo($"Entering {controller} controller > {function}() API...");

            Log.PublicInfo($"Request Value: {value ?? "-"}");
        }

        public static void ApiEnd(string controller, string function, string datetime, object @object)
        {
            Log.PublicInfo($"Response JSON: {@object.ToJson()}");

            Log.PublicInfo($"Exiting {controller} controller > {function}() API...");

            Log.PublicInfo($"<------End-{datetime}------>");
        }
    }
}