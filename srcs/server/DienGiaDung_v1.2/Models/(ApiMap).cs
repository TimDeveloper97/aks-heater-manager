using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace System
{
    public class ApiMap
    {
        Document _source;

        public Document Map => _map;
        Document _map;
        public ApiMap()
        {
            try
            {
                var content = File.ReadAllText(DB.Main.DataPath("apimap.json"));
                _source = Document.Parse(content);
            }
            catch
            {
                _source = new Document();
            }
            _map = _source.SelectContext("TuDien", null);
        }

        DocumentList _list;
        public DocumentList Items
        {
            get
            {
                if (_list == null)
                {
                    _list = new DocumentList();
                    foreach (var p in _map)
                    {
                        var a = Document.FromObject(p.Value);
                        a.Url = p.Key;

                        _list.Add(a);
                    }
                }
                return _list;
            }
        }
    }
}