using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class GlobalVariable
    {
        public string VERSION { get; set; }
        public DateTime DATE_TIME_NOW { get; set; } = new DateTime();
    }
}