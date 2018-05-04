using NManager.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Manager.API.App_Start
{
    /// <summary>
    /// 全局过滤器
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        public static void RegisterGlobalFilters(HttpFilterCollection collection)
        {
            collection.Add(new ModeVlidataGlobalFilte());
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ModeVlidataGlobalFilte:ActionFilterAttribute
    {
        /// <summary>
        /// 验证修改
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var modestate = actionContext.ModelState;
            if (!modestate.IsValid)
            {
                string error = string.Empty;
                foreach(var key in modestate.Keys)
                {
                    var state = modestate[key];
                    if (state.Errors.Any())
                    {
                        error = state.Errors.First().ErrorMessage;
                        break;
                    }
                }
                WebApiResult<string> webApiResult = new WebApiResult<string>() { Code = Enuncode.Error, Message = error };

                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content=new StringContent(JsonConvert.SerializeObject(webApiResult),System.Text.Encoding.GetEncoding("UTF-8"),"application/json")
                };
            }
            else
            {
                base.OnActionExecuting(actionContext);
            }
        }
    }
   
}