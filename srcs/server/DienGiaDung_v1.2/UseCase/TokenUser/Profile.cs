using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    partial class TokenUser
    {
        protected Document ChangePass()
        {
            var un = ObjectId;
            DB.Accounts.FindAndUpdate(un, acc => {
                var temp = new Account(un, ValueContext.Password);
                acc.Password = temp.Password ?? "*";
            });
            return Success();
        }

        protected Document UpdateProfile()
        {
            this.Push("me", ValueContext);
            DB.Accounts.Update(this);

            return Success();
        }
    }
}