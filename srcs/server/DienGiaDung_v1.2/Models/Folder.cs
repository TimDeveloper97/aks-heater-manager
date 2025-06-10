using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;

namespace System
{
    public class Folder
    {
        protected DirectoryInfo _di;
        public Folder(string path)
        {
            _di = new DirectoryInfo(path);
            if (_di.Exists == false)
            {
                _di.Create();
            }
        }
        public void ReadAll(Action<string, string> action)
        {
            GetFiles(fi => { 
                using (var sr = fi.OpenText())
                {
                    action(fi.Name, sr.ReadToEnd());
                }
            });
        }
        public Document FileMap
        {
            get
            {
                var doc = new Document();
                ReadAll((k, v) => doc.Add(k, v));

                return doc;
            }
        }
        public void GetFiles(Action<FileInfo> action)
        {
            foreach (var fi in _di.GetFiles()) action(fi);
        }

        public Folder Sub(string name) => new Folder(Path + '/' + name);

        public string Path => _di.FullName;
    }
}