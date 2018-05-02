using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Unifiedauthoritymanagement.Controllers
{
    /// <summary>
    /// 用户中心
    /// </summary>
    public class AccountController : BaseController
    {
        /// <summary>
        /// 登陆页面
        /// </summary>
        /// <returns></returns>
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        
        public JsonResult Login(string signup_name, string signup_password)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false });
            }
            if (string.IsNullOrEmpty(signup_name) || string.IsNullOrEmpty(signup_password))
            {
                return Json(new { success = false ,msg= "请输入账号密码" });
            }
            if (signup_name != "123")
            {
                return Json(new { success = false, msg = "请输入账号不匹配" });
            }
            if (signup_password != "123")
            {
                return Json(new { success = false, msg = "请输入密码不匹配" });
            }


            var result = new
            {
                success = true,
                TargetUrl = $"http://localhost:62739/Home/MainMenue"
            };
            return Json(result);
        }

        public JsonResult Ischeck(string username)
        {
           // AESUtil aesUtil = new AESUtil(token);
           // String password = aesUtil.decryptData(password);

           // 去掉token拿到原始密码
           //String passwordOriginal = password.replaceFirst(token, "");

            var resout = new
            {
                success = true,
                tock = Guid.NewGuid().ToString()
            };
            return Json(resout);
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>

        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        public JsonResult Registe()
        {
            return Json("");
        }

        public ActionResult UserInfo()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetList()
        {
            return Json("");
        }

        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult GetUserinfo(long id)
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
            var res = new
            {
                success = true,
                resout = model
            };
            return Json(res,JsonRequestBehavior.AllowGet);
            
        }


        public JsonResult GetMessage(long id)
        {
            //var msg = new
            //{
            //    Leav = new Random().Next(1, 5),
            //    Title = "Bhaumik Patel",
            //    Head = "Sed ut perspiciatis unde",
            //    Content = "Lorem ipsum dolor sit amet, consectetur adipisicing elit",
            //    Time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
            //};
           
            var Imode =new List<object>();
            Parallel.For(1, 10, i => {
                Imode.Add(new
                {
                    Leav = new Random().Next(1, 5),
                    Title = "Bhaumik Patel"+i.ToString(),
                    Head = "Sed ut perspiciatis unde",
                    Content = "Lorem ipsum dolor sit amet, consectetur adipisicing elit",
                    Time = DateTime.Now.AddMinutes(i).ToString("yyyy-MM-dd hh:mm:ss"),
                });
            });
            var res = new
            {
                success = true,
                resout = Imode
            };
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}