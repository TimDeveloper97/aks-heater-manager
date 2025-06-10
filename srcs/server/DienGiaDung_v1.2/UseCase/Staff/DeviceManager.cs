using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Actors
{
    partial class Staff
    {
        public override Document UpdateDevice() => ActionNotFound;
        
        protected Document DeviceList()
        {
            return Success(GetDataList(nameof(Models.Device), DB.Devices));
        }
        public override Models.Device FindDevice(string id)
        {
            GetDeviceMap(doc => {
                if (doc.ContainsKey(id) == false)
                    id = null;
            });
            return id == null ? null : base.FindDevice(id);
        }
    }
    partial class Customer
    {
    }
}