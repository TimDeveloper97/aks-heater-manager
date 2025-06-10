using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    using BsonData;
    partial class DB
    {
        static Collection _devices;
        static public Collection Devices
        {
            get
            {
                if (_devices == null)
                    _devices = Main.GetCollection(nameof(Devices));
                return _devices;
            }
        }
    }
}