using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    //[Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        [NonAction]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [NonAction]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [NonAction]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [NonAction]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [NonAction]
        public void Delete(int id)
        {
        }
    }
}
