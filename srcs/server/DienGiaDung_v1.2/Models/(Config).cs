using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace System
{
    public class Config : Document
    {
        public Config()
        {
            Copy(Parse(File.ReadAllText(DB.Main.DataPath("config.json"))));
            TuDien = GetDocument(nameof(TuDien));
            User = GetDocument(nameof(User));
            Cookie = GetDocument(nameof(Cookie));
        }

        public Document GetDocumentContext(params string[] path)
        {
            Document doc = this;
            foreach (var s in path)
            {
                doc = doc.GetDocument(s);
            }
            var res = new Document();
            foreach (var p in doc)
            {
                res.Add(p.Key, FromObject(p.Value));
            }
            return res;
        }
        public Document TuDien { get; private set; }
        public Document User { get; private set; }
        public Document Cookie { get; private set; }
    }
}