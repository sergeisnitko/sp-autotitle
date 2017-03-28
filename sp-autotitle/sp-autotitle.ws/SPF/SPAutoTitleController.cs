using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace sp_autotitle.ws.Controllers
{
    public class SPAutoTitleController : ApiController
    {
        [Route("api/TestPost")]
        [HttpPost]
        public string TestPost()
        {
            return "gggg";
        }

        [Route("api/TestGet")]
        [HttpGet]
        public string TestGet()
        {
            return "gggg1";
        }
    }
}
