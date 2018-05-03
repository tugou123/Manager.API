using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Unifiedauthoritymanagement
{
    public class BaseController: Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
          
          //  Response.Redirect("/Login/Index");

        }
    }
}