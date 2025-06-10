using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    partial class TokenUser
    {
        public virtual Document UpdateDevice()
        {
            return RunDeviceApi(d => {
                DB.Devices.Update(ValueContext.Move(d));
                return null;
            });
        }
    }
}