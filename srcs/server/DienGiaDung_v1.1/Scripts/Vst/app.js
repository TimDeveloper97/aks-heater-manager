
class SecondCounter {
    constructor() {
        let seconds = 0;

        this.tick = function (v) {}
        this.interrupt = function () {}
        let timer = 0;

        this.start = function (down_from = 0) {
            clearInterval(timer);

            seconds = down_from;
            timer = setInterval(() => {

                if (down_from) {
                    if (--seconds == 0) {
                        clearInterval(timer);
                        this.interrupt();
                    }
                }
                else {
                    ++seconds;
                }

                this.tick(seconds);
            }, 1000)
        }
    }
}

class DataViewModel {
    constructor(api) {

        this.data = {}
        this.primaryKey = null;
        this.selectedItem = {};
        this.createNewItem = function () { return {} }

        this.post = function (data, callback) {
            actor.post(api, this.data = data, r => {
                if (callback)
                    callback(r);
                else
                    redirect("");
            })
        }
        this.send = function (action, value, callback) {
            let d = this.primaryKey ? { _id: this.primaryKey } : {};
            if (action) d.action = action;
            if (value) d.value = value;

            this.post(d, callback)
        }

        // các hàm xử lý 1 bản ghi
        this.insert = function (value) { this.send("+", value) }
        this.update = function (value) { this.send("save", value) }
        this.delete = function (value) { this.send("-") }
        this.find = function (callback) {
            this.send("find", null, callback)
        }

        // các hàm xử lý tập bản ghi
        this.clear = function () { this.primaryKey = null; this.send('-') }
        this.import = function (rows) { this.send('import', { items: rows }) }
        this.select = function (callback) { this.send("list", callback) }
    }
}

class DataView {
    constructor(pageTitle, vm = new DataViewModel()) {
        this.mainTitle = function (text) { $(".main-title").html(text) }
        this.subTitle = function (text) { $(".sub-title").html(text) }
        this.createEditors = function () { return {} }
        this.beginAddNew = function () {
            let eds = this.createEditors()
            let frm = new EditorDialog(eds, v => vm.insert(v))
            frm.title("Thêm " + pageTitle).beginEdit(vm.createNewItem());
        }
        this.beginEdit = function (obj) {
            let eds = this.createEditors()
            let frm = new EditorDialog(eds, v => vm.update(v))
            frm.title("Cập nhật " + pageTitle).beginEdit(obj)
        }

    }
}

class DataListView extends DataView {
    constructor(pageTitle, vm = new DataViewModel(), cls = '.item') {
        super(pageTitle, vm)

        function wait(callback) {
            setTimeout(() => callback(vm.primaryKey), 200)
        }

        this.setItemActions = function (r) {
            let item = $(r);
            item.click(function () { vm.primaryKey = this.id })

            item.find(".delete-action").click(function () {
                let title = "Xóa " + pageTitle;
                let name = item.find(".Name").text();
                let text = "Toàn bộ thông tin của " + pageTitle + "<br/>"
                    + "<h4><b>" + name + "</b></h4><br/>"
                    + "sẽ bị xóa khỏi cơ sở dữ liệu."

                dialog.ask(text, title, () => vm.delete())
            })
            item.find(".update-action").click(function () {
                wait(i => vm.find(r => view.beginEdit(r)))
            })

            item.find(".open-action").click(function () {
                wait(i => view.openItem(i))
            })
        }

        let view = this;
        let items = $(cls).each(function () { view.setItemActions(this) })

        $(".insert-action").click(function () { view.beginAddNew() })
        $(".delete-all-action").click(function () {
            let text = "Tất cả các " + pageTitle + " sẽ bị xóa khỏi cơ sở dữ liệu";
            dialog.ask(text, null, () => vm.clear())
        })

        this.selectedItem = function () { return $(document.getElementById(vm.primaryKey)) }
        this.each = function (callback) { items.each(function () { callback(this) }) }
        this.refresh = function () { redirect("") }

        this.openItem = function (i) {
            redirect(window.location.href + "/" + i);
        }

        this.beginImport = function (cfg) {
            const pk = "_id";
            let cols = cfg.cols;
            let primaryKey = cfg.pk;

            let ed = {
                imp: new ImportBox().columns(cols),
            }
            let dlg = new EditorDialog(ed, v => {
                let rows = ed.imp.result();
                if (rows.length == 0) return;

                if (!primaryKey && cols[pk]) {
                    primaryKey = pk;
                }
                if (primaryKey) {
                    for (let e of rows) {
                        let id = e[primaryKey];
                        if (!id) {
                            console.log(e)
                            new Toast().mode("danger")
                                .text("Có bản ghi thiếu khóa chính")
                                .title("Importing").show(3);
                            return;
                        }

                        e[pk] = e[primaryKey]
                    }
                }

                vm.import(rows);
            });
            dlg.title(cfg.title ?? "Nhập " + pageTitle).show();
        }
    }
}

class HoatDongViewModel extends DataViewModel {
    constructor(id, name = "ThanhVien") {

        function create_api(a, n) {
            return [a ?? "manage-hoatdong", id, n].join('/');
        }

        super(create_api(null, name))

        this.dbName = id;
        this.clone = function (other) {
            return new HoatDongViewModel(id, other);
        }
        this.getThanhVien = function (filter, callback) {
            new HoatDongViewModel(id).send("where", filter, callback);
        }

        this.createViewModel = function (action, name) {
            return new DataViewModel(create_api(action, name))
        }
    }
}
class HoatDongDataView extends DataListView {
    constructor(pageTitle, vm = new HoatDongViewModel(), cls = ".item") {
        super(pageTitle, vm, cls)

        $(".side-menu a").each(function () {
            this.href = this.href + "/" + vm.dbName;
        })

        this.selectThanhVien = function (filter, itemp, target) {
            target.html("")
            vm.send("where", filter, lst => {
                lst.sort((a, b) => a.Ten.localeCompare(b.Ten))
                for (let o of lst) {
                    let iv = itemp.clone();
                    iv.appendTo(target)

                    this.setItemActions(iv.render(o).content);
                }
            })
        }
        
        this.beginCheck = function (chuongTrinh, xe, itemp, target) {
            target.html("")
            let m = vm.createViewModel("diem-danh", chuongTrinh);
            m.send("where", { Xe: xe }, lst => {
                lst = lst.sort((a, b) => a.Ten.localeCompare(b.Ten))
                for (let o of lst) {
                    let iv = itemp.clone();
                    iv.content.prop("id", o._id);
                    iv.content.attr("data-alphabet", o.Ten[0])

                    let s = o.s ?? '0';
                    iv.content.attr("data-state", s)

                    iv.appendTo(target)

                    this.setItemActions(iv.render(o).content);
                }
            })

            return m;
        }
    }
}

class DiemDanh {
    constructor(hId, cId) {

        class TacVu {
            constructor(text, state, cls) {
                let items = [];

                this.state = state;
                this.active = function () {
                    $(".main-title").html(text)
                    return this;
                }
                this.updateRow = function (r) {
                    $(r).attr("data-state", this.state)
                    actions.calc();
                }

                this.show = function () {
                    rows.hide();
                    items.forEach(r => $(r).show())
                }
                this.hide = function () { items.forEach(r => $(r).hide()) }
                this.calc = function () {
                    items = [];
                    rows.each(function () {
                        if (this.getAttribute("data-state") == state) {
                            this.className = "bag bag-" + cls;
                            items.push(this)
                        }
                    })
                    tools.find(".btn-" + cls).html(items.length);

                    return items.length;
                }
            }
        }


        var vm = new HoatDongViewModel(hId)
        let checkList = $("#check-list")
        let model = vm.createViewModel("diem-danh", cId);

        let rows = $(".bag")

        $alphabet(rows);
        rows.click(function () {
            let t = current;
            let s = this.getAttribute('data-state')
            if (s == t.state) t = actions.miss

            model.primaryKey = this.id;

            if (cId) {
                model.send(null, t.state,
                    () => t.updateRow(this));
            } else {
                t.updateRow(this);
            }
        })

        let tools = $(".tools")

        let current = new TacVu();
        let actions = {
            here: new TacVu("Danh sách điểm danh", '1', 'success'),
            pass: new TacVu("Xác nhận không tham gia", 'x', 'danger'),
            miss: new TacVu("Danh sách chưa có mặt", '0', 'secondary'),

            start: function (k) {
                if (k) current = this[k];
                current.active();

                rows.show();
                this.calc();
                this.pass.hide();
            },
            calc: function () {
                this.here.calc();
                this.pass.calc();
                this.miss.calc();

                tools.show();
            },
        }
        current = actions.here;

        this.show = function (k) { actions[k].show() }
        this.start = function (k) { actions.start(k) }
        session.ready = function () {

            setTimeout(() => {

                actions.start();
                checkList.show();
                if (window.innerWidth < 768) {
                    checkList.height(window.innerHeight - checkList.parent()[0].offsetTop - 20)
                }

            }, 200)

        }

    }
}