using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApp
{
    public abstract class ViewController : BaseController
    {
        protected virtual object GoHome() => Redirect("/home");
        public virtual object Index(string id)
        {
            var res = (Document)CheckToken();
            if (res.Code != 0)
                return View("~/views/home/login.cshtml", new Guest());

            User.RequestContext.Tag = id;
            return View(User);
        }

        protected virtual string GetViewName(ExecuteContext context)
        {
            return $"~/views/{context.Controller}/{context.Action}/{context.ViewName ?? "index"}.cshtml";
        }
        protected virtual Document GetViewModel(ExecuteContext context)
        {
            if (context.Id != null)
            {
                return new Document { ObjectId = context.Id };
            }
            return null;
        }
        protected virtual ExecuteContext CreateUserContext()
        {
            var context = new ExecuteContext { Url = Request.RawUrl };
            context.Tag = GetViewModel(context);
            User.RequestContext = context;
            return context;
        }
        protected virtual object ContextView()
        {
            var context = CreateUserContext();
            return View(GetViewName(context), User);
        }

        public virtual object L() => ContextView();
        public virtual object C() => ContextView();
        public virtual object O() => ContextView();
        public virtual object D() => ContextView();
    }
    public abstract class TokenViewController : ViewController
    {
        protected virtual bool IsRoleValid(Actor user) => user is TokenUser;
        protected override object ContextView()
        {
            var res = (Document)CheckToken();
            if (res.Code != 0)
            {
                return GoToLogin();
            }
            return base.ContextView();
        }
    }

    public abstract class RoleViewController : TokenViewController
    {
        protected override string GetViewName(ExecuteContext context)
        {
            return $"~/views/{context.Controller}/{User.Role}/{context.Action}/{context.ViewName ?? "index"}.cshtml";
        }
    }
}