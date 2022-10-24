using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class HealthCheckStatusResponse
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
    }
}