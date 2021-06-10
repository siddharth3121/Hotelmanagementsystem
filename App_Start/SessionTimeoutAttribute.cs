using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Hotelmanagementsystem.App_Start
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class DisableSession : Attribute { }
    /// <summary>
    ///     Session expire
    /// </summary>
    public class SessionExpire : ActionFilterAttribute
    {
        /// <summary>
        ///     Check session is timeout or not
        /// </summary>
        /// <param name="objFilterContext">When sesison is timeout then redirect action to login page</param>
        public override void OnActionExecuting(ActionExecutingContext objFilterContext)
        {
            base.OnActionExecuting(objFilterContext);

            bool disabled = objFilterContext.ActionDescriptor.IsDefined(typeof(DisableSession), true) ||
                        objFilterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(DisableSession), true);
            if (disabled)
                return;

            if (HttpContext.Current.Session["nid"] == null)
            {

                FormsAuthentication.SignOut();
                objFilterContext.Result =
                    new RedirectToRouteResult(new RouteValueDictionary
                        {
                            {"action", "Login"},
                            {"controller", "Admin"},
                            {"returnUrl", objFilterContext.HttpContext.Request.RawUrl}
                        }
                    );

                //string[] test = objFilterContext.HttpContext.Request.RawUrl.Split('/');
                //var nam = test[3];
                //if (nam == "SportScrutiny")
                //{
                //    new RedirectToRouteResult(new RouteValueDictionary
                //        {
                //            {"action", "LogOut"},
                //            {"controller", "SportScrutiny"},
                //            {"returnUrl", objFilterContext.HttpContext.Request.RawUrl}
                //        }
                //   );
                //}


            }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class MyAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            OnAuthorizationHelp(filterContext);
        }

        internal void OnAuthorizationHelp(AuthorizationContext filterContext)
        {

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = 401;
                    filterContext.HttpContext.Response.End();
                }
            }
        }
    }
}