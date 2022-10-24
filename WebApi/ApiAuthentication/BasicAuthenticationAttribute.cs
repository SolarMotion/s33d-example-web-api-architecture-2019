using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Filters;

namespace WebApi.ApiAuthentication
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var routeTemplete = actionContext.ControllerContext.RouteData.Route.RouteTemplate;
            var allowPublicRoutes = new List<string>()
            {
                //"api/snt/v1/auth/Login",
                //"api/snt/v1/auth/Logout",
                //"api/snt/v1/auth/ForgotPassword",
                //"api/snt/v1/auth/ChangePassword",
            };

            var isAllowedPublicRoute = allowPublicRoutes.Any(a => a == routeTemplete);
            if (!isAllowedPublicRoute)
            {
                if (actionContext.Request.Headers.Authorization == null)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");
                }
                else
                {
                    // Gets header parameters  
                    string authenticationString = actionContext.Request.Headers.Authorization.Parameter;
                    string originalString = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationString));

                    // Gets username and password  
                    string username = originalString.Split(':')[0];
                    string password = originalString.Split(':')[1];

                    // Validate token 
                    if (!ApiSecurity.VaidateUser(username, password))
                    {
                        // returns unauthorized error  
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");
                    }
                }
            }

            base.OnAuthorization(actionContext);
        }
    }
}