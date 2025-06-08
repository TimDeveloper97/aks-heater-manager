using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Actors
{
    partial class Customer
    {
        public Document StaffMap => SelectContext(nameof(Staff), e => { });
        protected Document StaffList()
        {
            //this.Push("Password", "1234");
            //this.SelectContext("Staff", map => {
            //    map.Push("0989xxx", new Document());
            //});

            var doc = this.GetDocument(nameof(Staff));
            var lst = new DocumentList();

            foreach (var k in doc.Keys)
            {
                lst.Add(DB.Accounts.Find(k));
            }
            return Success(lst);
        }
    }
}