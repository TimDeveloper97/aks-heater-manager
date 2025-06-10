using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    partial class TokenUser
    {
        protected Document FindAccount()
        {
            var acc = FindAccount(ValueContext.ObjectId);
            return acc == null ? Error(-1) : Success(acc);
        }
    }
}