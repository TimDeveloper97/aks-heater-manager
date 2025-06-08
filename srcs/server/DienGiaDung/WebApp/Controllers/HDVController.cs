using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class HDVController : RoleViewController
    {
        public override object Index(string id)
        {
            return Redirect($"/hoatdong/index/{User.Value}");
        }
    }
}