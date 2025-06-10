using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Actors
{
    partial class Admin
    {
        protected Document AddUser()
        {
            return Success(CreateUser(ValueContext, null));
        }
    }
    partial class Technical
    {
        protected Document UserList()
        {
            return Success(DB.Accounts.Select());
        }
        protected Document FindUser()
        {
            return Success(FindAccount(ValueContext.ObjectId));
        }
        protected Document RemoveUser()
        {
            var id = ValueContext.ObjectId;
            var cus = FindAccount(id);
            if (cus != null)
            {
                DB.Accounts.Delete(id);
                foreach (var k in cus.GetDocument(nameof(Staff)).Keys)
                {
                    var s = DB.Accounts.Find(k);
                    if (s.Role == nameof(Staff))
                        DB.Accounts.Delete(k);
                }
            }
            return Success();
        }
        protected Document DeviceToUser()
        {
            var code = SetDeviceToUser(null, null, null);
            switch (code)
            {
                case 1: return Error(code, "DEVICE");
                case 2: return Error(code, "USER");
            }
            return Success();
        }
    }
}