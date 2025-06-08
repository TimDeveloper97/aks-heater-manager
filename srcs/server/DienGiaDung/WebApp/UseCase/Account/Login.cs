using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    partial class Guest
    {
        static public Document TryLogin(Document context, Action<Actor> successCallback)
        {
            var un = context.UserName;
            var acc = DB.Accounts.Find<Account>(un);
            if (acc == null) return TokenError;

            var o = new Account(un, context.Password);
            if (acc.Password != o.Password)
                return Error(1, "PASSWORD");

            var u = Logged.Add(acc);
            DB.Config.User.SelectContext(u.Role, doc => u.Timeout = doc.Timeout);

            successCallback?.Invoke(u);

            return Success(u);
        }
        protected Document Login() => TryLogin(ValueContext, null);
    }
}