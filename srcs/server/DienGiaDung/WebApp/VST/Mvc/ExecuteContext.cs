using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    public class ExecuteContext
    {
        static public string GetObjectName(string name)
        {
            var r = new char[name.Length];
            int count = 0;
            foreach (var c in name)
            {
                if (c != '-') r[count++] = char.ToLower(c);
            }
            return new string(r, 0, count);
        }
        public class ContextParams
        {
            string[] _vals;
            public ContextParams(IEnumerable<string> vals)
            {
                _vals = vals.ToArray();
            }
            public string this[int index]
            {
                get
                {
                    return index >= _vals.Length ? null : _vals[index];
                }
            }
        }

        string _cname, _aname, _url;

        public string Url
        {
            get => _url;
            set
            {
                var url = value;
                var qry = "";

                var i = url.IndexOf('?');
                if (i > 0)
                {
                    qry = url.Substring(i + 1);
                    url = url.Substring(0, i);
                }

                _cname = _aname = null;
                _url = url;

                var lst = new List<string>();
                foreach (var s in value.Split('/'))
                {
                    if (s == string.Empty) continue;
                    if (_cname == null)
                    {
                        _cname = GetObjectName(s);
                        continue;
                    }
                    if (_aname == null)
                    {
                        _aname = GetObjectName(s);
                        continue;
                    }
                    lst.Add(s);
                }

                Params = new ContextParams(lst);
                
                Query = new Document();
                if (qry != string.Empty)
                {
                    foreach (var s in qry.Split('&'))
                    {
                        i = s.IndexOf('=');
                        if (i > 0)
                        {
                            Query.Push(s.Substring(0, i), s.Substring(i + 1));
                        }
                    }
                }
            }
        }

        public string Controller { get => _cname ?? "home"; set => _cname = value; }
        public string Action { get => _aname ?? "index"; set => _aname = value; }
        public ContextParams Params { get; private set; }
        public Document Query { get; private set; } = new Document();
        public string ViewName => Params[0];
        public string Id => Params[1];
        public object Tag { get; set; }
    }
}