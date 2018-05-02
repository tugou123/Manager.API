using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manager.API.TokenManager
{

    public class UserToken
    {
        public string UserType { set; get; }

        public int? UId { set; get; }

        public string Permission { set; get; }

        public DateTime Timeout { set; get; }

        public string Token { set; get; }


    }
    public class UserTokenManager
    {
        public static  IUserTokenRepository _tokenRep { set; get; }
        private const string TokenName = "Passport.Token";
      
        public static void  AddToken(UserToken token)
        {

        }

        private static List<UserToken> InitCache
        {
            get
            {
                if (HttpRuntime.Cache[TokenName] == null)
                {
                    var tokens = _tokenRep.GetAll();
                    //cache 过期时间 令牌过期时间 * 2；
                    HttpRuntime.Cache.Insert(TokenName, tokens, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromDays(7 * 2));
                }
                var ts = (List<UserToken>)HttpRuntime.Cache[TokenName];
                return ts;
            }
           
        }

        public static int GetUId(string token)
        {
            var tokens = InitCache;
            var result = 0;
            if (tokens.Count > 0)
            {
                var id = tokens.Where(c => c.Token == token).Select(c => c.UId).FirstOrDefault();
                if (id != null)
                  result = id.Value;
            }
            return result;
        }

        public static string GetPermission(string token)
         {
             var tokens = InitCache;
             if (tokens.Count == 0)
                 return "NoAuthorize";
             else
                 return tokens.Where(c => c.Token == token).Select(c => c.Permission).FirstOrDefault();
         }
 
         public static string GetUserType(string token)
         {
             var tokens = InitCache;
             if (tokens.Count == 0)
                 return "";
             else
                 return tokens.Where(c => c.Token == token).Select(c => c.UserType).FirstOrDefault();
        }

          /// <summary>
         /// 判断令牌是否存在
         /// </summary>
         /// <param name="token"></param>
         /// <returns></returns>
         public static bool IsExistToken(string token)
         {
             var tokens = InitCache;
             if (tokens.Count == 0) return false;
             else
             {
                 var t = tokens.Where(c => c.Token == token).FirstOrDefault();
                 if (t == null)
                     return false;
                 else if (t.Timeout<DateTime.Now)
                 {
                     RemoveToken(t);
                     return false;
                 }
                 else
                 {
                     // 小于8小时 更新过期时间
                     if ((t.Timeout - DateTime.Now).TotalMinutes< 1 * 60 - 1)
                     {
                         t.Timeout = DateTime.Now.AddHours(8);
                         UpdateToken(t);
                     }
                     return true;
                 }
 
             }
        }
        public static bool UpdateToken(UserToken token)
         {
             var tokens = InitCache;
             if (tokens.Count == 0) return false;
             else
             {
                 var t = tokens.Where(c => c.Token == token.Token).FirstOrDefault();
                 if (t == null)
                     return false;
                 t.Timeout = token.Timeout;
                 // 更新数据库
                 var tt = _tokenRep.FindByToken(token.Token);
                 if (tt != null)
                 {
                     tt.UserType = token.UserType;
                     tt.UId = token.UId;
                     tt.Permission = token.Permission;
                     tt.Timeout = token.Timeout;
                     _tokenRep.Update(tt);
                 }
                 return true;
             }
        }

         /// <summary>
          /// 移除指定令牌
          /// </summary>
          /// <param name="token"></param>
          /// <returns></returns>
          public static void RemoveToken(UserToken token)
          {
              var tokens = InitCache;
              if (tokens.Count == 0) return;
              tokens.Remove(token);
              _tokenRep.Remove(token);
          }


        public static void RemoveToken(string token)
          {
              var tokens = InitCache;
              if (tokens.Count == 0) return;
  
              var ts = tokens.Where(c => c.Token == token).ToList();
              foreach (var t in ts)
              {
                  tokens.Remove(t);
                  var tt = _tokenRep.FindByToken(t.Token);
                  if (tt != null)
                      _tokenRep.Remove(tt);
              }
          }

        public static void RemoveToken(int uid)
        {
            var tokens = InitCache;
            if (tokens.Count == 0) return;

            var ts = tokens.Where(c => c.UId == uid).ToList();
            foreach (var t in ts)
            {
                tokens.Remove(t);
                var tt = _tokenRep.FindByToken(t.Token);
                if (tt != null)
                    _tokenRep.Remove(tt);
            }
        }

    }




    public interface IUserTokenRepository : IDisposable
    {
        List<UserToken> GetAll();
        void Update(UserToken token);
        void Remove(UserToken token);

        UserToken FindByToken(string token);

    }

    public class UserTokenRepository : IUserTokenRepository
    {
     
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public UserToken FindByToken(string token)
        {
            return new UserToken();
        }

        public List<UserToken> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(UserToken token)
        {
            throw new NotImplementedException();
        }

        public void Update(UserToken token)
        {
            throw new NotImplementedException();
        }
    }
}