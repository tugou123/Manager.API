
using Manager.API.Models;
using Manager.API.TokenManager;
using Manager.Model.InputeMode;
using System;
using System.Configuration;
using System.Web.Http;

using Orleans;
using System.Threading.Tasks;
using Mangaer.Contract.IImplement;
using Base.Info.Enums;

namespace Manager.API.Controllers
{
    public class AccountController : BaseController
    {

        private int Token = 0;
        IUserComponet _userComponet;
        ISysLogComponet logRep;
     

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">登录人员信息： 账号，密码 ，是否记住密码</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task <WebApiResult> Login([FromBody]LoginUser user)
        {
            _userComponet = GrainClient.GrainFactory.GetGrain<IUserComponet>("key");
            logRep = GrainClient.GrainFactory.GetGrain<ISysLogComponet>("key");
            string username = user.UserName;
            string password = user.Password;
            bool IsRememberMe = user.RemenberMe;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return new WebApiResult
                {
                    Code = Enuncode.Error
                };

            LoginUser u = null;

            LoginResultEnum loginResult =await _userComponet.Login(username, password, (o)=> { u = o; });
            if (loginResult == LoginResultEnum.Success)
            {
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

               await logRep.Add(log);
                Token = u.Id;
                var data = new
                {
                    id = u.Id,
                    issaler = u.IsSaler.HasValue ? u.IsSaler.Value : false,
                    username = u.UserName,
                    token = _tokens
                };
                return new WebApiResult<dynamic>
                {
                    Code = Enuncode.Success,
                    Message = "Success",
                    Data = data
                };
            }

            if (loginResult == LoginResultEnum.UserNameUnExists)
            {
                return new WebApiResult
                {
                    Code = Enuncode.Failed,
                    Message = "账号不存在",
                };
            }
            if (loginResult == LoginResultEnum.VerifyCodeError)
            {
                return new WebApiResult
                {
                    Code = Enuncode.Failed,
                    Message = "验证码错误",
                };
            }
            if (loginResult == LoginResultEnum.UserNameOrPasswordError)
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

            logRep.Update(log);
            //System.Web.Security.FormsAuthentication.SignOut();
            UserTokenManager.RemoveToken(this.Token);
            return new WebApiResult()
            {
                Code = Enuncode.Success,
                Message = "退出成功"
            };
        }

    }
}

  

