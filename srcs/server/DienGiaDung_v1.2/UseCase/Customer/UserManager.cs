using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Actors
{
    partial class Customer
    {
        public override Document FindAccount(string id)
        {
            GetStaffMap(doc => {
                if (doc.ContainsKey(id) == false)
                    id = null;
            });
            return id == null ? null : base.FindAccount(id);
        }
        protected Document UserList()
        {
            return Success(GetDataList(nameof(Staff), DB.Accounts));
        }
        protected Document AddUser()
        {
            ValueContext.Role = nameof(Staff);
            return Success(CreateUser(null, id => {
                GetStaffMap(doc => {
                    doc.Add(id, new Document());
                });
                DB.Accounts.Update(this);
            }));
        }
        protected Document DeviceToUser()
        {            
            return Success(SetDeviceToUser(null, null, null));
        }
    }
}