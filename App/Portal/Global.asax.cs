using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Portal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }


        void Application_AuthenticateRequest(object sender, EventArgs e)
        {


            //string cookieName = FormsAuthentication.FormsCookieName;
            //HttpCookie authCookie = Context.Request.Cookies[cookieName];

            //if (null == authCookie)
            //{
            //    //There is no authentication cookie.
            //    return;
            //}
            //FormsAuthenticationTicket authTicket = null;
            //try
            //{
            //    authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            //}
            //catch (Exception ex)
            //{
            //    //Write the exception to the Event Log.
            //    return;
            //}
            //if (null == authTicket)
            //{
            //    //Cookie failed to decrypt.
            //    return;
            //}
            //if (HttpContext.Current.User != null)
            //{
            //    var test = HttpContext.Current.User.Identity is FormsIdentity;

            //}
            ////When the ticket was created, the UserData property was assigned a
            ////pipe-delimited string of group names.
            ////string[] groups = authTicket.UserData.Split(new char[] { '|' });
            //string[] roles = Roles.GetRolesForUser(authTicket.UserData);

            ////Create an Identity.
            //GenericIdentity id = new GenericIdentity(authTicket.UserData, "LdapAuthentication");
            ////This principal flows throughout the request.
            //GenericPrincipal principal = new GenericPrincipal(id, roles);
            //Context.User = principal;
        }


    }
}
