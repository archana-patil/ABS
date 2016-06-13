using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Mbpros.Common
{
    public class AccessDeniedAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectResult("/Account/AccessDenied");
            }
        }
    }

    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            //// check  sessions here
            if (Convert.ToString(HttpContext.Current.Session["NONLOGIN"]) == "YES")
            {
                base.OnActionExecuting(filterContext); return;
            }
            if (HttpContext.Current.Session["USERNAME"] == null)
            {
                filterContext.Result = new RedirectResult("/Account/Login", true);

                //filterContext.Result = new RedirectToRouteResult(
                //                            new RouteValueDictionary(new { controller = "Account", action = "Login" })
                //                        );
                //filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }

}