using BsonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public partial class HoatDong : Document
    {
    }
    public class HoatDongDB : Database
    {
        public DocumentList GetAccountList()
        {
            var lst = new DocumentList();
            lst.AddRange(BTC.SelectAll());
            //Action<Document, string> xe = (e, n) => {
            //    var one = e.GetDocument(n);
            //    if (one != null && one.Count > 0)
            //    {
            //        lst.Add(one);
            //        one.Role = n;
            //        one.Xe = e.ObjectId;
            //    }
            //};
            //foreach (var e in Xe.SelectAll())
            //{
            //    xe(e, nameof(Actors.TruongXe));
            //    xe(e, nameof(Actors.HDV));
            //}
            return lst;
        }
        public DocumentList GetChuongTrinh(bool now)
        {
            var lst = ChuongTrinh.Select();
            foreach (var e in lst)
            {
                if (e.BD == null)
                {
                    e.BD = DateTime.Now;
                }
                if (e.KT == null)
                    e.KT = e.BD;

                var n = e.Ngay.Value;
                var k = e.KT.Value;
                var b = e.BD.Value;

                e.BD = b = new DateTime(n.Year, n.Month, n.Day, b.Hour, b.Minute, 0);
                e.KT = k = new DateTime(n.Year, n.Month, n.Day, k.Hour, k.Minute, 0);

                var s = "warning";
                if (k < DateTime.Now)
                {
                    s = "secondary";
                }
                else if (k >= DateTime.Now && DateTime.Now >= b)
                {
                    s = "danger";
                }
                e.State = s;
            }

            if (now) lst = lst.Where(x => x.KT >= DateTime.Now);
            return new DocumentList(lst.OrderBy(x => x.KT));
        }
        public DocumentList GetBaoCao()
        {
            var lst = GetChuongTrinh(false);
            var tvs = ThanhVien.SelectAll();
            var total = tvs.Count;

            foreach (var e in lst)
            {
                int miss = 0, here = 0, pass = 0;
                var cid = e.ObjectId;

                foreach (var o in tvs)
                {
                    o.DangKy(doc => {
                        var v = doc.GetString(cid);
                        if (v != null)
                        {
                            switch (v[0])
                            {
                                case '1': ++here; return;
                                case 'x': ++pass; return;
                            }
                        }
                        ++miss;
                    });
                }
                e.DangKy(doc => {
                    doc.Push("1", here);
                    doc.Push("0", miss);
                    doc.Push("x", pass);
                });
            }
            return lst;
        }
        public DocumentList GetThanhVien(Document filter)
        {
            var lst = ThanhVien.Select(filter);
            foreach (var e in lst)
            {
                if (e.Ten == null)
                {
                    e.Ten = e.Name.VnPersonName().SortTerm().VnCharacter();
                }    
            }
            return lst;
        }
        public DocumentList GetDiemDanh(string id, Document filter)
        {
            var lst = new DocumentList();
            foreach (var e in GetThanhVien(filter))
            {
                var o = e.Clone();
                o.DangKy(doc => {
                    o.State = doc.GetString(id) ?? "0";
                });
                lst.Add(o);
            }

            return new DocumentList(lst.OrderBy(x => x.Ten));
        }
        public DocumentGroup GetPhongInfo(string phong, string nguoi)
        {
            if (phong == null)
            {
                phong = ThanhVien.Find(nguoi).Phong;
                if (phong == null)
                    return new DocumentGroup();
            }

            var one = Phong.Find<DocumentGroup>(phong);
            if (one == null)
            {
                one = new DocumentGroup { Phong = phong };
            }

            if (one.Records.Count == 0)
            {
                foreach (var e in ThanhVien.Select())
                {
                    if (e.Phong == phong)
                        one.Records.Add(e);
                }
            }
            return one;
        }
        public DocumentList GetDanhSachPhong(string xe)
        {
            var fields = new string[] { 
                nameof(Document.Name),
                nameof(Document.Ten),
                nameof(Document.SoDT),
                nameof(Document.DiaChi),
            };
            
            var remain = new DocumentList();
            var map = new Dictionary<string, DocumentGroup>();

            foreach (var p in GetThanhVien(null))
            {
                var room = p.Phong;
                if (p.Xe == xe)
                {
                    var e = new Document().Copy(p, fields);
                    if (room != null)
                    {
                        if (!map.TryGetValue(room, out var g))
                        {
                            map.Add(room, g = new DocumentGroup());
                            g.Phong = room;
                        }

                        g.Records.Add(e);
                    }
                }
                else if (room != null)
                {
                    remain.Add(p);
                }
            }
            if (map.Count > 0)
            {
                foreach (var p in remain)
                {
                    if (map.TryGetValue(p.Phong, out var g))
                        g.Records.Add(new Document().Copy(p, fields));
                }
            }

            var lst = new DocumentList();
            foreach (var p in map)
            {
                var room = p.Key.Length == 1 ? '0' + p.Key : p.Key;
                var g = p.Value;
                g.Phong = room;
                g.ObjectId = p.Key;

                var one = Phong.Find(p.Key);
                if (one != null) g.Value = one;

                lst.Add(g);
            }
            return new DocumentList(lst.OrderBy(x => x.Phong));
        }    
        public class HoatDongCollection : Collection
        {
            public HoatDongCollection(string name, HoatDongDB db) : base(name, db)
            {
            }
            public HoatDongDB HoatDongDB => (HoatDongDB)Database;
        }
        public HoatDongDB(string id) : base(id)
        {
            DB.Main.Add(this);
        }

        HoatDongCollection _chuongTrinh;
        public HoatDongCollection ChuongTrinh
        {
            get
            {
                if (_chuongTrinh == null)
                    _chuongTrinh = new HoatDongCollection(nameof(ChuongTrinh), this);
                return _chuongTrinh;
            }
        }

        public void ResetData()
        {
            _xe = null;
        }

        #region Person
        PersonCollection _btc;
        public PersonCollection BTC
        {
            get
            {
                if (_btc == null)
                    _btc = new PersonCollection(nameof(BTC), this);
                return _btc;
            }
        }

        PersonCollection _thanhVien;
        public PersonCollection ThanhVien
        {
            get
            {
                if (_thanhVien == null)
                    _thanhVien = new PersonCollection(nameof(ThanhVien), this);
                return _thanhVien;
            }
        }
        public class PersonCollection : HoatDongCollection
        {
            public PersonCollection(string name, HoatDongDB db) : base(name, db) { }
            public override object Exec(string action, string id, Document value)
            {
                switch (action)
                {
                    case "-":
                        var keys = id == null ? GetKeys() : new List<string> { id };
                        foreach (var k in keys)
                        {
                            DB.Accounts.Delete(k);
                        }
                        break;
                }

                return base.Exec(action, id, value);
            }
            public Document CreateAccount(string id, string name, string role, bool check)
            {
                if (check && DB.Accounts.Find(id) != null)
                    return null;

                var acc = new Account(id, id.Substring(id.Length - 4), role);
                acc.Value = HoatDongDB.Name;
                acc.Name = name;

                DB.Accounts.InsertOrUpdate(acc);
                return acc;
            }
            public virtual void CreateAccount(string role)
            {
                foreach (var e in this.Select())
                {
                    CreateAccount(e.SoDT, e.Name, role ?? e.Role, role != null);
                }    
            }
        }
        #endregion

        #region Xe
        public class PhucVuCollection : PersonCollection
        {
            public PhucVuCollection(HoatDongDB db) : base(nameof(db.PhucVu), db)
            {
            }
            public override object Exec(string action, string id, Document value)
            {
                switch (action)
                {
                    case "+":
                        var x = value.Xe;
                        var r = value.Role;
                        foreach (var e in this.Select())
                        {
                            if (e.Xe == x && e.Role == r)
                            {
                                Delete(e);
                                break;
                            }
                        }
                        HoatDongDB.Xe[x].Push(r, value);
                        break;
                }

                return base.Exec(action, id, value);
            }
        }
        PhucVuCollection _phucVu;
        public PhucVuCollection PhucVu
        {
            get
            {
                if (_phucVu == null)
                {
                    _phucVu = new PhucVuCollection(this);
                }
                return _phucVu;
            }
        }

        public class XeInfo : Document
        {
            public DocumentList ThanhVien { get; set; }
            public Document TruongXe
            {
                get => SelectContext(nameof(TruongXe), null);
                set => Push(nameof(TruongXe), value);
            }
            public Document HDV
            {
                get => SelectContext(nameof(HDV), null);
                set => Push(nameof(HDV), value);
            }
            public Document TaiXe
            {
                get => SelectContext(nameof(TaiXe), null);
                set => Push(nameof(TaiXe), value);
            }

        }
        public class XeCollection : Dictionary<string, XeInfo>
        {
            public XeCollection(HoatDongDB db)
            {
                var groups = db.GetThanhVien(null).GroupBy(nameof(Document.Xe));
                foreach (var g in groups)
                {
                    var k = g.Xe ?? "(chưa phân)";
                    var x = new XeInfo {
                        ObjectId = k,
                        ThanhVien = new DocumentList(g.Records.OrderBy(e => e.Ten)),
                    };
                    this.Add(k, x);
                }
                foreach (var e in db.PhucVu.Select())
                {
                    var id = e.Xe;
                    if (id != null)
                    {
                        this.TryGetValue(id, out var x);
                        x.Push(e.Role, e);
                    }    
                }    
            }
        }

        XeCollection _xe;
        public XeCollection Xe
        {
            get
            {
                if (_xe == null)
                    _xe = new XeCollection(this);
                return _xe;
            }
        }
        #endregion

        #region Phòng
        public class PhongInfo : DocumentGroup
        {
        }
        public class PhongCollection : HoatDongCollection
        {
            public PhongCollection(HoatDongDB db) : base(nameof(db.Phong), db)
            {
            }
        }

        PhongCollection _phong;
        public PhongCollection Phong
        {
            get
            {
                if (_phong == null)
                    _phong = new PhongCollection(this);
                return _phong;
            }
        }
        #endregion

        public void Delete()
        {
            DB.HoatDong.FindAndDelete(Name, doc => {
                ThanhVien.Exec("-", null, null);
                BTC.Exec("-", null, null);

                this.Clear();
            });
        }
    }
}
namespace Models
{
    public class HoatDongCollection : Collection
    {
        public HoatDongCollection() : base(nameof(HoatDong), DB.Main) { }
        public HoatDong FindOne(string id) => Find<HoatDong>(id);
        public override object Exec(string action, string id, Document value)
        {
            if (action == "-")
            {
                DB.GetHoatDongDB(id)?.Delete();
                return null;
            }
            return base.Exec(action, id, value);
        }
    }
}

namespace System
{
    using Models;
    partial class DB
    {
        public static HoatDongDB GetHoatDongDB(string id)
        {
            var db = (HoatDongDB)DB.Main.Childs[id];

            if (db == null)
                db = new HoatDongDB(id);
            return db;
        }
        static public HoatDongCollection HoatDong
        {
            get
            {
                if (_hoatDong == null)
                    _hoatDong = new HoatDongCollection();
                return _hoatDong;
            }
        }
        static HoatDongCollection _hoatDong;
    }
}


