using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.ApiAuthentication
{
    public class ApiSecurity
    {
        public static bool VaidateUser(string username, string password)
        {
            if ((username == "sntuser" && password == "abc123"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}