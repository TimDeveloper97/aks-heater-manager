using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace System
{
    public static class AppKeys
    {
        public const string User = "user";
        public const string UserMenu = "user-menu";
        public const string LoginInfo = "login-info";
        public const string Token = "token";
    }

}

namespace WebApp
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_BeginRequest()
        {
            if (Request.RawUrl.IndexOf("/api/") == 0)
            {
                Response.Clear();

                var context = new ApiContext {
                    Url = Request.RawUrl,
                    Request = Request,
                    Response = Response,
                };

                var controller = new Api();
                var result = controller.Exec(context);
                
                Response.Write(result ?? "{}");
                Response.End();
            }
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            MyHelper.Start(Server.MapPath(""));
            MyRazor.TemplatePath = Server.MapPath("/views/_template/");
            DB.Start(Server.MapPath("/app_data"));
        }
    }
}
