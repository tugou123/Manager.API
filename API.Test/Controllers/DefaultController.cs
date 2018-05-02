using Manager.Model.InputeMode;
using Mangaer.Contract.IImplement;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace API.Test.Controllers
{
    public class DefaultController : ApiController
    {
        [HttpGet]
        public async Task<string> Login()
        {
            IUserComponet _userComponet = GrainClient.GrainFactory.GetGrain<IUserComponet>("key");
            ISysLogComponet logRep = GrainClient.GrainFactory.GetGrain<ISysLogComponet>("key");
            var mx = await _userComponet.Login("123", "123");

            return "OK";
        }
    }
}
