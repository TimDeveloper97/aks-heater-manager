using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    partial class TokenUser
    {
        Document RunDeviceApi(Func<Models.Device, object> callback)
        {
            var devi = FindDevice(ValueContext.ObjectId);
            if (devi == null)
                return Error(-1);

            return Success(callback(devi));
        }
        protected Document DeviceStatus()
        {
            return RunDeviceApi(d => d.GetStatus());
        }

        protected Document DeviceLog()
        {
            return RunDeviceApi(d => d.GetLog(ValueContext.GetDateTime("start"), ValueContext.GetDateTime("end")));
        }

        protected Document DeviceDayLog()
        {
            return RunDeviceApi(d => d.GetDayLog(ValueContext.GetValue<long>("days")));
        }
    }
}