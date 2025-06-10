using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class Device : Document
    {
        public object GetStatus()
        {
            return "Đang phát triển";
        }

        public object GetLog(DateTime? start, DateTime? end)
        {
            return "Đang phát triển";
        }

        public object GetDayLog(long days)
        {
            return "Đang phát triển";
        }
    }
}