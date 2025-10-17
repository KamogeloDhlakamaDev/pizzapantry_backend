using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace pizzapantry_backend.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]

    public class AdjustItemController : ControllerBase
    {
        private readonly ISoftiator _softiator;
        public AdjustItemController(ISoftiator softiator)
        {
            _softiator = softiator;
        }
    }
}