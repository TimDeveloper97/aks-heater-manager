using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Actors
{
    partial class Technical
    {
        protected Document DeviceList() => Success(RunDevice("l"));
        protected Document AddDevice()
        {
            return Success(RunDevice("+"));
        }
        protected Document RemoveDevice()
        {
            return Success(RunDevice("-"));
        }
        protected Document FindDevice()
        {
            return Success(RunDevice("f"));
        }
    }
}
