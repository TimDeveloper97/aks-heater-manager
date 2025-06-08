using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace System.Web
{
    public class MyHelper
    {
        static string _spath { get; set; }
        public static void Start(string path)
        {
            _spath = path + '/';
        }
        static public string ServerPath(string path) => _spath + path;

        static public HtmlString Include(string folder, bool version = true)
        {
            Func<string, string, string> at = (n, v) => $" {n}=\"{v}\"";
            Func<string, string> op = (n) => $"<{n}";
            Func<string, string> cl = (n) => $"></{n}>\r\n";
            Func<FileInfo, string> src = (f) => $"{folder}/{f.Name}" 
                + (version ? string.Format("?v={0:yyyyMMddHHmmss}", f.LastWriteTime) : "");

            var s = "";
            var dir = ServerPath(folder);

            foreach (var fn in Directory.GetFiles(dir))
            {
                var fi = new FileInfo(fn);

                var ext = fi.Extension;

                if (ext == ".js")
                {
                    s += op("script")
                        + at("type", "text/javascript")
                        + at("src", src(fi))
                        + cl("script");
                }
                else if (ext == ".css")
                {
                    s += op("link")
                        + at("rel", "stylesheet")
                        + at("href", src(fi))
                        + "/>\r\n";
                }
            }

            return new HtmlString(s);
        }
        static public HtmlString Scripts(string folder = "") => Include("/Scripts/" + folder);
        static public HtmlString Css(string folder = "") => Include("/Content/" + folder);
        static public HtmlString Fonts(string folder = "") => Include("/Fonts/" + folder, false);
    }
}