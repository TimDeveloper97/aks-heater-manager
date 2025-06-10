using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class AdminController : TokenViewController
    {
        protected override bool IsRoleValid(Actor user) => user is Actors.Admin;
    }
}