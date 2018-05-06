using NManager.API.TokenManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Manager.API.App_Start
{
    /// <summary>
    /// 登陆验证
    /// </summary>
    public class ApiAuthorizeAttribute : AuthorizeAttribute
     {
        /// <summary>
        /// 返会 404
        /// </summary>
        /// <param name="actionContext"></param>
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (!IsAuthorized(actionContext))
            {
                var challengeMsg = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new HttpError("Token过期"));
                challengeMsg.Headers.Add("WWW-Authenticate", "Basic");
                throw new System.Web.Http.HttpResponseException(challengeMsg);
            }
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                return;
            }
            HandleUnauthorizedRequest(actionContext);
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
         {
             var ts = actionContext.Request.Headers.Where(c => c.Key.ToLower() == "token").FirstOrDefault().Value;
             if (ts != null && ts.Count() > 0)
             {
                 var token = ts.First<string>();
                 if (!TokenResultMsg.IsExistToken(token))
                 {
                     return false;
                 }
                 return true;
             }
             if (actionContext.Request.Method == HttpMethod.Options)
                 return true;
             return false;
         }
     }
}



//HttpResponseMessage responseMessage =
//    Request.CreateResponse(HttpStatusCode.OK, "success");
//CookieHeaderValue cookie = new CookieHeaderValue("userToken", authorization)
//{ Path = "/",
//    Domain = Request.RequestUri.Host,
//    Expires = DateTimeOffset.Now.AddDays(7) };
//responseMessage.Headers.AddCookies(new[] {cookie});


//function ajaxOp(url, type, data, contentType)
//{         $.ajax({
//        url: url,     
//        type: type,     
//        data: data,       
//        //crossDomain: true,     
//        beforeSend: function(xhr) {
//            xhr.setRequestHeader('Authorization', 'Basic ' + $.cookie("userToken"));
//        },       
//      contentType: contentType,    
//        success: function(result) {
//            alert(result);
//        }         });
//}