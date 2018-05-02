using Orleans;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Manager.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var configpx = Orleans.Runtime.Configuration.ClientConfiguration.LocalhostSilo();
            
            GrainClient.Initialize(configpx);
          
            GlobalConfiguration.Configure(WebApiConfig.Register);
           
        }
    }
}
