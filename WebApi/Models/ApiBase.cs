using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web;
using static Library.CommomExtensions;

namespace WebApi.Models
{
    public class ApiBaseRequest
    {
        [JsonProperty(PropertyName = "org_ref")]
        public Guid OrgRefNo { get; set; }
    }

    public class ApiBaseResponse
    {
    }

    public class ApiResponseBody
    {
        [IgnoreDataMember]
        public HttpStatusCode HttpStatusCode { get; set; }

        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        [JsonProperty(PropertyName = "response_code")]
        public int ResponseCode { get; set; }

        [JsonProperty(PropertyName = "response_message")]
        public string ResponseMessage { get; set; }

        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }
    }
}