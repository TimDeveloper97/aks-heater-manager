using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    partial class Document
    {
        public string UserName { get => GetString(nameof(UserName)); set => Push(nameof(UserName), value); }
        public string Password { get => GetString(nameof(Password)); set => Push(nameof(Password), value); }
        public string Role { get => GetString(nameof(Role)); set => Push(nameof(Role), value); }
        public string Token { get { return GetString("token"); } set => Push("token", value); }
        public long Timeout { get { return GetValue<long>(nameof(Timeout)); } set => Push(nameof(Timeout), value); }
        public string LastLogin
        {
            get => GetString(nameof(LastLogin));
            set => SetString(nameof(LastLogin), value);
        }
    }
}


namespace System
{
    partial class Document
    {
        public string Avatar { get => GetString(nameof(Avatar)); set => Push(nameof(Avatar), value); }
        public string Email { get => GetString(nameof(Email)); set => Push(nameof(Email), value); }
        public string SoDT { get => GetString(nameof(SoDT)); set => Push(nameof(SoDT), value); }
    }
}

namespace System
{
    partial class Document
    {
        public string Name { get => GetString(nameof(Name)); set => Push(nameof(Name), value); }
        public DocumentList Items { get => GetArray<DocumentList>("items"); set => Push("items", value); }
        public string[] Fields
        {
            get
            {
                var s = GetString("fields");
                if (s == null)
                    return null;
                var lst = new List<string>();
                foreach (var i in s.Split(';')) lst.Add(i.Trim());
                return lst.ToArray();
            }
        }
        public string Link { get => GetString(nameof(Link)); set => Push(nameof(Link), value); }
        public int Vote { get => GetValue<int>(nameof(Vote)); set => Push(nameof(Vote), value); }
    }
}

namespace System
{
    using BsonData;
    partial class Document
    {
        public long Mins { get => GetValue<long>("mins"); set => Push("mins", value); }
        public string State { get => GetString("s"); set => Push("s", value); }
        public DateTime? BD { get => GetDateTime(nameof(BD)); set => Push(nameof(BD), value); }
        public DateTime? KT { get => GetDateTime(nameof(KT)); set => Push(nameof(KT), value); }
        public DateTime? Ngay { get => GetDateTime(nameof(Ngay)); set => Push(nameof(Ngay), value); }
        public DateTime? Last { get => GetDateTime(nameof(Last)); set => Push(nameof(Last), value); }
        public string DiaChi { get => GetString(nameof(DiaChi)); set => Push(nameof(DiaChi), value); }
        public string Ten { get => GetString(nameof(Ten)); set => Push(nameof(Ten), value); }
        public long SoLuong { get => GetValue<long>(nameof(SoLuong)); set => Push(nameof(SoLuong), value); }
        public string NoiDung { get => GetString(nameof(NoiDung)); set => Push(nameof(NoiDung), value); }
        public string Command { get => GetString("cmd"); set => Push("cmd", value); }
        public string Xe { get => GetString(nameof(Xe)); set => Push(nameof(Xe), value); }
        public string Phong { get => GetString(nameof(Phong)); set => Push(nameof(Phong), value); }

        public void DangKy(Action<Document> callback) => SelectContext("dang-ky", callback);
        public DocumentList ToList()
        {
            var lst = new DocumentList();
            foreach (var p in this)
            {
                var e = FromObject(p.Value);
                e.ObjectId = p.Key;
                lst.Add(e);
            }
            return lst;
        }
        public Document InnerJoin(params Collection[] tables)
        {
            var doc = Clone();
            var id = ObjectId;
            foreach (var t in tables)
            {
                var e = t.Find(id);
                if (e != null)
                {
                    doc.Copy(e);
                }
            }
            return doc;
        }
        public Document Load(params Collection[] tables)
        {
            var doc = Clone();
            var id = ObjectId;
            foreach (var t in tables)
            {
                var e = t.Find(id) ?? new Document();
                doc.Push(t.Name, e);
            }
            return doc;
        }
    }
}

namespace System
{
    partial class Document
    {
        public DocumentList GetDataList(string name, BsonData.Collection collection)
        {
            var lst = new DocumentList();
            this.SelectContext(name, doc => {
                var err = new List<string>();
                foreach (var k in doc.Keys)
                {
                    var one = collection.Find(k);
                    if (one == null)
                    {
                        err.Add(k);
                    }
                    else
                    {
                        lst.Add(one);
                    }
                }

                if (err.Count != 0)
                {
                    foreach (var k in err)
                    {
                        doc.Remove(k);
                    }
                    DB.Accounts.Update(this);
                }
            });
            return lst;
        }
        public Document GetStaffMap(Action<Document> callback) =>
            SelectContext(nameof(Actors.Staff), callback);
        public Document GetDeviceMap(Action<Document> callback) =>
           SelectContext(nameof(Models.Device), callback);
    }
}