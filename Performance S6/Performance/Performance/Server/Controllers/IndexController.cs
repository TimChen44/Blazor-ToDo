using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Performance.Entity;
using Performance.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Performance.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class IndexController : ControllerBase
    {

        public IndexController()
        {

        }

        [HttpGet]
        public string GetHost()
        {
            return HttpContext.Request.Host.ToString();
        }
    }

}
