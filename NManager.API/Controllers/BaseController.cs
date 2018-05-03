using Manager.API.App_Start;
using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace NManager.API.Controllers
{
    [ApiAuthorize]
    public class BaseController : ApiController
    {

     
    }
}
