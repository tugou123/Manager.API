using NManager.API.Models;
using Manager.Model.InputeMode;
using NManager.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace NManager.API.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult Index()
        {
            return Ok("<html><body><a href='/swagger/ui/index'>WebApi 成功启动 </a><script>window.location='http://manager.api.com/swagger/ui/index';</script></body></htm>");
        }
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        [HttpPost]
        public WebApiResult Token([FromBody] LoginUser loginUser)
        {
            if (!ModelState.IsValid)
            {
                return new WebApiResult
                {
                    Code = Enuncode.Error,
                };
            }
            var token = Guid.NewGuid().ToString().Replace("-", "");

            //存入缓存
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert("Token", token);
            //存入缓存


            return new WebApiResult<dynamic>
            {
                Code = Enuncode.Success,
                Message = "Success",
                _other = new
                {
                    Token = "token",
                    Type = "系统类型",

                },
                Data = new
                {
                    Token = token,
                    Type = "LSM"
                }

            };
        }
        [HttpPost]
        /// <summary>
        /// 急啊急啊急急急啊
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IHttpActionResult Demo(long Id)
        {
            return Ok("nnnnnnn");
        }


        public WebApiResult Mennue()
        {
            return new WebApiResult
            {
                
                Code = Enuncode.Success

            };

        }

    }
}
