
using NManager.API.TokenManager;
using Manager.Model.InputeMode;
using System;
using System.Configuration;
using System.Web.Http;

using Orleans;
using System.Threading.Tasks;
using Mangaer.Contract.IImplement;
using Base.Info.Enums;
using NManager.API.Models;
using System.Collections.Generic;

namespace NManager.API.Controllers
{
    public class AccountController : BaseController
    {

        private int Token = 0;
      

     

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">登录人员信息： 账号，密码 ，是否记住密码</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task <WebApiResult> Login([FromBody]LoginUser user)
        {
            IUserComponet  _userComponet = GrainClient.GrainFactory.GetGrain<IUserComponet>("key");
            ISysLogComponet logRep = GrainClient.GrainFactory.GetGrain<ISysLogComponet>("key");
            string username = user.UserName;
            string password = user.Password;
            bool IsRememberMe = user.RemenberMe;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return new WebApiResult
                {
                    Code = Enuncode.Error
                };

            LoginUser u = null;

            var  loginResult =await _userComponet.Login(username, password);
            if (loginResult.LoginResult == LoginResultEnum.Success)
            {
                u = loginResult.loginUser;
                var _tokens= TokenResultMsg.Createtoken(u.Id);

               
                //UserTokenManager.AddToken(ut);
            

                // 登录log

                var log = new Log()
                {
                    Action = "Login",
                    Detail = "会员登录:" + u.UserType + "|" + u.UserName,
                    CreateDate = DateTime.Now,
                    CreatorLoginName = u.UserName,
                    IpAddress = "127.0.0.1",
                    UserId=1
                    
                };

              // await logRep.Add(log);
                Token = u.Id;
                var data = new
                {
                    id = u.Id,
                    issaler = u.IsSaler.HasValue ? u.IsSaler.Value : false,
                    username = u.UserName,
                    token = _tokens,
                    TargetUrl = $"Home/MainMenue"
                };
                return new WebApiResult<dynamic>
                {
                    Code = Enuncode.Success,
                    Message = "Success",
                    Data = data
                };
            }

            if (loginResult.LoginResult == LoginResultEnum.UserNameUnExists)
            {
                return new WebApiResult
                {
                    Code = Enuncode.Failed,
                    Message = "账号不存在",
                };
            }
            if (loginResult.LoginResult == LoginResultEnum.VerifyCodeError)
            {
                return new WebApiResult
                {
                    Code = Enuncode.Failed,
                    Message = "验证码错误",
                };
            }
            if (loginResult.LoginResult == LoginResultEnum.UserNameOrPasswordError)
            {
                return new WebApiResult
                {
                    Code = Enuncode.Failed,
                    Message = "账号密码错误",
                };
            }
            return new WebApiResult
            {
                Code = Enuncode.Failed,
                Message = "登录失败，原因未知",
            };
        }
        /// <summary>
        /// 退出当前账号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public WebApiResult SignOut()
        {
            // 登录log

            var log = new Log()
            {
                Action = "SignOut",
                Detail = "会员退出:" + "adminstor",//RISContext.Current.CurrentUserInfo.UserName,             
                CreatorLoginName = "adminstor", //RISContext.Current.CurrentUserInfo.UserName,
                IpAddress = "127.0.0.1", //GetClientIp(this.Request)
                EndTime = DateTime.Now,
                HostName="hahha",
                UserId=2,
                Id=1,              
            };

          //  logRep.Update(log);
            //System.Web.Security.FormsAuthentication.SignOut();
            UserTokenManager.RemoveToken(this.Token);
            return new WebApiResult()
            {
                Code = Enuncode.Success,
                Message = "退出成功"
            };
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<WebApiResult> GetUserinfo(long id)
        {
            var model = new
            {
                Username = "John Doe",
                Email = "name@example.com",
                Address = "Street 123, Avenue 45, Country",
                Telphone = "078-57841285",
                Status = "Active",
                Userrating = new Random().Next(1, 8),
                Membersince = "Jun 03, 2014",
                Urlimgae = "http=//lorempixel.com/640/480/business/1/"
            };
            return new WebApiResult<dynamic>
            {
                Code = Enuncode.Success,
                Message = "Success",
                Data = model
            };

        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<WebApiResult> GetMessage(long id)
        {
            
            var Imode = new List<object>();
            Parallel.For(1, 10, i => {
                Imode.Add(new
                {
                    Leav = new Random().Next(1, 5),
                    Title = "Bhaumik Patel" + i.ToString(),
                    Head = "Sed ut perspiciatis unde",
                    Content = "Lorem ipsum dolor sit amet, consectetur adipisicing elit",
                    Time = DateTime.Now.AddMinutes(i).ToString("yyyy-MM-dd hh:mm:ss"),
                });
            });
            return new WebApiResult<dynamic>
            {
                Code = Enuncode.Success,
                Message = "Success",
                Data = Imode
            };
        }
    }
}

  

