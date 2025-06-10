using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class HomeController : ViewController
    {
        public override object Index(string id)
        {
            var doc = (Document)CheckToken();
            if (doc.Code != 0)
                return GoToLogin();

            return GoToUserMainPage();
        }
        public object Login()
        {
            if (Request.HttpMethod == "GET")
            {
                return GoToUserMainPage();
            }

            return CheckToken();
        }
        public object Logout()
        {
            if (User != null)
            {
                Actor.Logged.Remove(User);
                User = null;
            }
            RemoveCookie(AppKeys.LoginInfo, AppKeys.Token);
            Session.Abandon();
            return GoHome();
        }
    }
}
