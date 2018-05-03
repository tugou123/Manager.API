using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NManager.API.Models
{
    public class WebApiResult
    {
        /// <summary>
        /// 返回代码
        /// </summary>
        public Enuncode Code { set; get; }

        public string Message { set; get; }

        public object _other { set; get; }

        public bool Success { set; get; } = false;
    }


    public class WebApiResult<T>: WebApiResult
    {
        public T Data { set; get; }
    }



    public enum Enuncode
    {
        [Display(Name ="访问错误")]
        Error=10000,
        [Display(Name = "成功")]
        Success =0,
        [Display(Name = "无数据")]
        Failed =1
    }
}