
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
using System.Web.Caching;

namespace NManager.API.TokenManager
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

       
        public static string Createtoken(int staffId)
        {
            //token 产生 规则(用户ID+时间戳+)
            RemoveToken(staffId);
            // 生成新Token
            var _tokens = string.Format("{0}{1}", Guid.NewGuid().ToString(), DateTime.Now.Ticks);
            var token = DESEncrypt.Encrypt(_tokens,"kyes");
            // token过期时间
            int timeout = 8;
            if (!int.TryParse(ConfigurationManager.AppSettings["TokenTimeout"], out timeout))
                timeout = 8;
            // 创建新token
            var ut = new Tokens()
            {
                StaffId= staffId,
                SignToken = _tokens,
                ExpireTime = DateTime.Now.AddMinutes(5),
                
            };
            InitCache(token, ut);
            return token;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokens"></param>
        public static void InitCache(string tokenname, Tokens tokens)
        {
            if (tokens == null)
                return;
           
            //if (HttpRuntime.Cache[tokenname]==null)
            HttpRuntime.Cache.Insert(tokenname, tokens, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration);
            //HttpRuntime.Cache.Insert(tokens.StaffId.ToString(), tokens, 
            //    new CacheDependency(@"D:\123.txt"),DateTime.Now.AddMinutes(3),
            //    TimeSpan.FromMinutes(7),CacheItemPriority.Default,null);

            //var token = HttpRuntime.Cache.Get(tokens.StaffId.ToString());


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

        /// <summary>
        /// 验证token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsExistToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return false;
            var _token = DESEncrypt.Decrypt(token, "kyes");

            var tokes = HttpRuntime.Cache[token] as Tokens;
            if(tokes ==null|| tokes.SignToken!= _token)
            {
                return false;
            }
            else
            {
                if (tokes.ExpireTime <= DateTime.Now)
                {
                    tokes.ExpireTime = DateTime.Now.AddMinutes(5);
                    InitCache(token,tokes);
                }

                return true;
            }
           

        }
    }


}