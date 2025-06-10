using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    using BsonData;
    partial class TokenUser
    {
        public virtual object RunDB(Collection data, string action, Document context = null)
        {
            if (context == null)
                context = ValueContext;

            var id = context.ObjectId;
            return data.Exec(action, id, context);
        }
        public virtual object RunAccount(string action, Document context = null)
        {
            return RunDB(DB.Accounts, action, context); 
        }
        public virtual object RunDevice(string action, Document context = null)
        {
            return RunDB(DB.Devices, action, context);
        }
        public virtual object CreateUser(Document context, Action<string> callback)
        {
            if (context == null)
                context = ValueContext;

            var un = context.ObjectId;
            var ps = un;
            if (un.Length > 4)
            {
                ps = un.Substring(un.Length - 4);
            }    

            DB.Accounts.Insert(un, new Account(un, ps, context.Role).Copy(context));
            callback?.Invoke(un);

            context.Password = ps;
            return context;
        }
        public virtual Document FindUser(string id)
        {
            return DB.Accounts.Find(id);
        }
        public virtual Models.Device FindDevice(string id)
        {
            return DB.Devices.Find<Models.Device>(id);
        }
        public virtual int SetDeviceToUser(string deviceId, string userId, Action<Document, Document> callback)
        {
            if (deviceId == null)
                deviceId = ValueContext.GetString("deviceId");

            if (userId == null)
                userId = ValueContext.GetString("userId");


            var devi = FindDevice(deviceId);
            if (devi == null) return 1;

            var user = FindAccount(userId);
            if (user == null) return 2;

            user.SelectContext(nameof(Models.Device), doc => {
                doc.Push(deviceId, new Document());
                DB.Accounts.Update(user);
            });
            callback?.Invoke(devi, user);
            return 0;
        }
    }
}