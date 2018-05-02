using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;
using Manager.API.App_Start;
using Newtonsoft.Json.Serialization;
using Orleans;

namespace Manager.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "v1/{controller}/{action}/{id}",
            //   defaults: new { controller = "Home", action = "Index", id = RouteParameter.Optional }
            //);
            config.Routes.MapHttpRoute(
                name:"SwaggerApi",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new {controller="Home",action="Index",id=RouteParameter.Optional}            
                );
             //去除string 字符串的 trim
            config.BindParameter(typeof(string), new TirmModelBinder());
            //默认返回Json
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedEncodings.Clear();
            //解决IE访问 下载问题
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));
            //解决Json 数据使用混合大小写，驼峰式 但是JavaScript 首字母小写
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //跨域访问
            var allowOrigins = ConfigurationManager.AppSettings["cors_allowOrigins"];
            var allowHeaders = ConfigurationManager.AppSettings["cors_allowHeaders"];
            var allowMethods = ConfigurationManager.AppSettings["cors_allowMethods"];
            var globalCors = new EnableCorsAttribute(allowOrigins, allowHeaders, allowMethods)
            {
                SupportsCredentials = true
            };           
            config.EnableCors(globalCors);
            config.Filters.Add(new ModeVlidataGlobalFilte());
            var configpx = Orleans.Runtime.Configuration.ClientConfiguration.LocalhostSilo();
            GrainClient.Initialize(configpx);
            // config.MessageHandlers.Add(new MessageHanlder());
            //消息机制
            //错误机制
        }
    }

    public class TirmModelBinder : IModelBinder
    {
      
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            throw new NotImplementedException();
        }
    }

    public class MessageHanlder : MessageProcessingHandler
    {
        protected override HttpRequestMessage ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override HttpResponseMessage ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
