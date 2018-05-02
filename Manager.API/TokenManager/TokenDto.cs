using Base.Expand;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Manager.API.TokenManager
{
    /// <summary>
    /// 登陆Token
    /// </summary>
    public class Tokens
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public int StaffId { get; set; }

        /// <summary>
        /// 用户名对应签名Token
        /// </summary>
        public string SignToken { get; set; }

        /// <summary>
        /// Token过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class TokenResultMsg
    {

       
        public static Tokens Createtoken(int staffId)
        {
            UserTokenManager.RemoveToken(staffId);
            // 生成新Token
            var token = DESEncrypt.Encrypt(string.Format("{0}{1}", Guid.NewGuid().ToString("D"), DateTime.Now.Ticks));
            // token过期时间
            int timeout = 8;
            if (!int.TryParse(ConfigurationManager.AppSettings["TokenTimeout"], out timeout))
                timeout = 8;
            // 创建新token
            var ut = new Tokens()
            {
                StaffId= staffId,
                SignToken = token,
                ExpireTime = DateTime.Now.AddHours(timeout),
                
            };
            InitCache(ut);
            return ut;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokens"></param>
        public static void InitCache(Tokens tokens)
        {
            if (tokens == null)
                return;
            var tokenname = tokens.ToString();
            //if (HttpRuntime.Cache[tokenname]==null)
            HttpRuntime.Cache.Insert(tokens.StaffId.ToString(), tokens, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromDays(7 * 2));
          

        }
        /// <summary>
        /// 移除Token
        /// </summary>
        /// <param name="staffId"></param>
        public static void RemoveToken(int staffId)
        {
            var tokenmame = staffId.ToString();
            if (HttpRuntime.Cache[tokenmame] == null)
                return;
            HttpRuntime.Cache.Remove(tokenmame);
            
        }


    }


}