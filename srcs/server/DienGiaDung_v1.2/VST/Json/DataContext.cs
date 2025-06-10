using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public partial class Document : Dictionary<string, object>
    {
        #region CLONE
        public Document Clone()
        {
            var doc = new Document();
            foreach (var p in this)
            {
                if (p.Value != null && !p.Value.Equals(string.Empty))
                {
                    doc.Add(p.Key, p.Value);
                }
            }
            return doc;
        }
        public Document Copy(Document src) => Copy(src, null);
        public Document Copy(Document src, IEnumerable<string> names)
        {
            if (names == null) names = src.Keys.ToArray();
            foreach (var name in names)
            {

                if (this.ContainsKey(name) == false)
                {
                    object v = null;
                    src.TryGetValue(name, out v);
                    if (v != null)
                    {
                        base.Add(name, v);
                    }
                }
            }
            return this;
        }
        public Document Move(Document dst) => Move(dst, null);
        public Document Move(Document dst, IEnumerable<string> names)
        {
            if (names == null) names = Keys.ToArray();

            foreach (var name in names)
            {
                findField(name, (k, v) => dst.Push(k, v));
            }
            return dst;
        }
        public T ChangeType<T>() where T : Document, new()
        {
            var dst = new T();
            dst.Copy(this);

            return dst;
        }
        public T ToObject<T>()
        {
            return JObject.FromObject(this).ToObject<T>();
        }
        public static T Parse<T>(string text) where T : Document
        {
            return JObject.Parse(text).ToObject<T>();
        }
        public static Document Parse(string text)
        {
            return JObject.Parse(text).ToObject<Document>();
        }
        public static T FromObject<T>(object src) where T : Document
        {
            return JObject.FromObject(src).ToObject<T>();
        }
        public static Document FromObject(object src)
        {
            return JObject.FromObject(src).ToObject<Document>();
        }
        public static Document FromList(IEnumerable<Document> src, string name)
        {
            var doc = new Document();
            foreach (var e in src)
            {
                doc.Add(e.ObjectId, e.GetString(name));
            }
            return doc;
        }
        #endregion

        string correctName(string key)
        {
            if (char.IsUpper(key[0]))
                return char.ToLower(key[0]) + key.Substring(1);
            return key;
        }
        void findField(string name, Action<string, object> callback)
        {
            var k = correctName(name);
            if (TryGetValue(k, out object v))
                callback(k, v);
        }
        object getField(string name, Func<string, object, object> callback)
        {
            var k = correctName(name);
            TryGetValue(k, out object v);
            return callback(k, v);
        }

        public void Push(string name, object value)
        {
            var k = correctName(name);
            Remove(k);
            if (value != null && !value.Equals(string.Empty))
            {
                base.Add(k, value);
            }
        }
        public object Pop(string name) => getField(name, (k, v) => Remove(k));
        public T Pop<T>(string name) => (T)(Pop(name) ?? default(T));

        /// <summary>
        /// Select a document, if has callback then create document when not found
        /// </summary>
        /// <param name="name"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public T SelectContext<T>(string name, Action<T> callback) where T : Document, new()
        {
            return (T)getField(name, (k, v) => {
                if (v == null)
                {
                    if (callback == null) return null;
                    Push(k, v = new T());
                }
                else if (v.GetType() != typeof(T))
                {
                    var obj = v is string ? JObject.Parse((string)v) : JObject.FromObject(v);
                    v = obj.ToObject<T>();
                    Push(k, v);
                }

                var context = (T)v;
                if (callback != null)
                {
                    callback.Invoke(context);
                }
                return context;
            });
        }
        public Document SelectContext(string name, Action<Document> callback) => SelectContext<Document>(name, callback);

        public override string ToString()
        {
            return JObject.FromObject(this).ToString();
        }

        #region GET ITEMS VALUES
        public T GetObject<T>(string name)
        {
            return (T)getField(name, (k, v) => {
                if (v == null)
                    return default(T);
                
                if (typeof(T) == v.GetType())
                {
                    return v;
                }
                return (v is string ? JObject.Parse((string)v) : JObject.FromObject(v)).ToObject<T>();
            });
        }
        public T GetDocument<T>(string name) where T : Document, new()
        {
            var doc = GetObject<T>(name);
            if (doc == null)
            {
                doc = new T();
            }
            return doc;
        }
        public Document GetDocument(string name) => GetDocument<Document>(name);

        public T GetArray<T>(string name)
        {
            return (T)getField(name, (k, v) => {
                if (v == null)
                    return default(T);

                if (typeof(T) == v.GetType())
                {
                    return v;
                }
                var obj = v is string ? JArray.Parse((string)v) : JArray.FromObject(v);
                return obj.ToObject<T>();
            });
        }
        public T GetValue<T>(string name, T defaultValue)
        {
            return (T)getField(name, (k, v) => { 
                if (v != null)
                {
                    try 
                    {
                        return Convert.ChangeType(v, typeof(T));
                    } 
                    catch 
                    { 
                    };
                }
                return defaultValue;
            });
        }
        public T GetValue<T>(string name) => GetValue(name, default(T));
        public DateTime? GetDateTime(string name) => GetValue<DateTime>(name);
        public virtual string GetString(string name) => GetValue<string>(name);
        public object SelectPath(string path)
        {
            var s = path.Split('.');
            var n = s.Length - 1;
            var doc = this;
            for (int i = 0; i < n; i++)
            {
                if (doc.Count == 0)
                    return null;

                doc = doc.GetDocument(s[i]);
            }
            doc.TryGetValue(correctName(s[n]), out object v);
            return v;
        }
        #endregion

        #region SET ITEMS VALUES
        public void SetObject(string name, object value)
        {
            Push(name, JObject.FromObject(value).ToString());
        }
        public void SetDocument(Document doc)
        {
            string name = (string)doc.Pop("_id");
            Push(name, doc);
        }
        public void SetArray(string name, object value)
        {
            Push(name, JArray.FromObject(value).ToString());
        }
        public virtual void SetString(string name, string value) => Push(name, value);
        #endregion

        #region ObjectId
        public string ObjectId { get => GetString("_id"); set => Push("_id", value); }
        public virtual string GetPrimaryKey(Document context)
        {
            return new BsonData.ObjectId();
        }

        public string Join(string seperator, params string[] names)
        {
            var lst = new List<object>();
            if (names.Length == 0)
            {
                names = Keys.ToArray();
            }
            foreach (string name in names)
            {
                findField(name, (k, v) => lst.Add(v));
            }
            return string.Join(seperator, lst);
        }
        public string Unique(params string[] names) => this.Join("_", names);
        #endregion
    }
}
