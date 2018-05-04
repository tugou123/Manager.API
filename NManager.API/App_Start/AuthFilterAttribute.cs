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
#if DEBUGM
    public class AuthFilterAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //如果用户方位的Action带有AllowAnonymousAttribute，则不进行授权验证
            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                return;
            }
            var verifyResult = actionContext.Request.Headers.Authorization != null &&  //要求请求中需要带有Authorization头
                               actionContext.Request.Headers.Authorization.Parameter == "123456"; //并且Authorization参数为123456则验证通过

            if (!verifyResult)
            {
                //如果验证不通过，则返回401错误，并且Body中写入错误原因
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new HttpError("Token 不正确"));
            }
        }

    }

#endif 

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
            #region 弃用
            //var contect = actionContext.Request.Properties["MS_HttpContext"] as HttpContextBase;
            // var token = contect.Request.Headers["Token"];
            // if (!string.IsNullOrEmpty(token))
            // {
            //获取token的 Key 携带ID
            // var tokenstr = HttpRuntime.Cache.Get(token) as Tokens;
            //    if (tokenstr== null || string.IsNullOrEmpty(tokenstr.SignToken)|| !IsAuthorized(actionContext))
            //    {
            //            var challengeMsg = actionContext.Request.CreateErrorResponse(HttpStatusCode.Ambiguous, new HttpError("Token 过期"));
            //            challengeMsg.Headers.Add("WWW-Authenticate", "Basic");
            //            throw new System.Web.Http.HttpResponseException(challengeMsg);
            //    }
            //}
            //else
            //{
            //    var challengeMsg = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new HttpError("Token 不正确"));
            //    challengeMsg.Headers.Add("WWW-Authenticate", "Basic");
            //    throw new System.Web.Http.HttpResponseException(challengeMsg);
            //}  

            #endregion
            
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
             // 验证token
             var ts = actionContext.Request.Headers.Where(c => c.Key.ToLower() == "token").FirstOrDefault().Value;
             if (ts != null && ts.Count() > 0)
             {
                 var token = ts.First<string>();
                 // 验证token
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