using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp
{
    public abstract class BaseController : Controller
    {
        protected void RemoveCookie(params string[] names)
        {
            var t = DateTime.Now.AddDays(-1);
            foreach (var n in names)
            {
                Response.Cookies.Add(new HttpCookie(n) { 
                    Expires = t 
                });
            }
        }
        protected void SetCookie(string name, object value, int days) 
        {
            Response.Cookies.Add(new HttpCookie(name, value.ToString()) { 
                Expires = DateTime.Now.AddDays(days)
            });
        }
        protected bool GetCookie(string name, Action<string> callback)
        {
            var ck = Request.Cookies.Get(name);
            if (ck != null)
            {
                callback(ck.Value);
                return true;
            }
            return false;
        }

        Actor _actor;
        protected virtual Actor GetActor() => new Guest();
        new public Actor User
        {
            get
            {
                if (_actor == null)
                {
                    _actor = (Actor)Session[AppKeys.User];
                    if (_actor == null) _actor = GetActor();
                }
                return _actor;
            }
            set
            {
                if (_actor != value)
                {
                    Session[AppKeys.User] = _actor = value;
                }
            }
        }

        public Document CheckToken()
        {
            if (Session[AppKeys.User] == null)
            {
                var actor = GetCookieActor();
                if (actor == null)
                {
                    return Actor.Error(-3, "TIMEOUT");
                }

                User = actor;
                return Actor.Success(actor.Token);
            }
            return Actor.Success();
        }
        protected object GoToUserMainPage()
        {
            return Redirect($"/{User.Role}");
        }
        protected object GoToLogin()
        {
            return View("~/views/home/login.cshtml");
        }
        protected object GoFirst() => RedirectToAction("index");
        public object Error() => View("~/views/shared/error.cshtml");
        protected Actor GetCookieActor()
        {
            Actor actor = null;
            GetCookie(AppKeys.Token, s => actor = Actor.Logged.Find(s));
            if (actor == null)
            {
                GetCookie(AppKeys.LoginInfo, s => {
                    Guest.TryLogin(Document.Parse(s), a => {
                        actor = a;
                        SetCookie(AppKeys.Token, a.Token, DB.Config.Cookie.GetValue<int>(AppKeys.Token));
                    });
                });
            }
            return actor;
        }
    }
}