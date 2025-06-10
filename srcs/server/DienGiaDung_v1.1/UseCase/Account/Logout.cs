using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    partial class TokenUser
    {
        protected object Logout()
        {
            Logged.Remove(this);
            return Success();
        }
    }
}