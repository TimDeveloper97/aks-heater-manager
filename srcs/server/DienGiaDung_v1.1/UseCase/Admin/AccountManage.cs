using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    partial class TokenUser
    {
        public virtual Account CreateAccount(string un, string ps, string role)
        {
            return new Account(un, ps, role);
        }
        public virtual Document FindAccount(string id)
        {
            return DB.Accounts.Find(id);
        }
    }
}

namespace Actors
{
}

namespace Actors
{
    partial class Admin
    {
        protected Document CreateAccount()
        {
            var un = ValueContext.ObjectId;
            if (DB.Accounts.Find(un) != null)
            {
                return Error(1);
            }

            var pw = ValueContext.Password ?? un.Substring(un.Length - 4);
            var acc = new Account(un, pw, ValueContext.Role);

            DB.Accounts.Insert(acc.Copy(ValueContext));

            return Success(pw);
        }
    }
}