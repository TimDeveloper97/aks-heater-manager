
let update = {
    init: function (selector, get_del_msg) {
        let list_view = $(selector ?? ".data-list-view")
        let api_name = list_view.attr("data-action")
        let title = list_view.attr("data-title")

        if (!title) title = api_name;

        actor.begin(api_name);

        // Insert action
        list_view.find(".btn-add").each(function () {

            if (!this.innerHTML) {
                this.innerHTML = "<i class='fa fa-plus'></i>";
                this.className += " btn btn-success";
            }

            $(this).click(function () {
                var eds = update.create_editor();
                let dlg = new EditorDialog(eds, v => actor.post('insert', v, r => {
                    if (items.length == 0) {
                        redirect("");
                        return;
                    }

                    let first = items[0];
                    let row = new View(first).clone().binding(r);
                    row.content.prop("id", r._id)
                        .addClass("new-item")
                        .insertBefore(first);

                    set_index();
                }));
                dlg.title("Thêm " + title).show();
            })
        })

        let items = list_view.find(".item");

        function get_item_data(e) {
            let p = $(e).parent();
            if (p.hasClass("item")) {
                let o = {
                    _id: p.prop("id")
                };
                p.find(".binding").each(function () {
                    let k = this.getAttribute("data-path");
                    if (k) {
                        o[k] = this.innerHTML;
                    }
                })
                return o;
            }
            return get_item_data(p);
        }

        function set_index() {
            let i = 0;
            list_view.find(".STT").each(function () {
                this.innerHTML = ++i;
            })
        }

        items.each(function () {
            let act = this.querySelector(".actions")

            let e = act.querySelector(".btn-edit")
            if (!e.innerHTML) {
                e.innerHTML = "<i class='fa fa-pencil'></i>"
                e.className += " btn btn-info"
            }
            e.addEventListener("click", () => {
                var eds = update.create_editor();
                let dlg = new EditorDialog(eds, v => actor.post('update', $.extend(o, v), r => { new View(this).binding(r) }));

                let o = get_item_data(act)
                dlg.title("Cập nhật " + title).beginEdit(o);
            })

            let d = act.querySelector(".btn-delete")
            if (!d.innerHTML) {
                d.innerHTML = "<i class='fa fa-close'></i>"
                d.className += " btn btn-danger"
            }
            d.addEventListener("click", () => {
                let e = get_item_data(act);
                let dlg = new Dialog("No");

                let t = "Xóa " + title
                dlg.append(t + " <span class='text-danger'>" + get_del_msg(e) + "</span>")
                    .title(t)
                    .accept("Yes", () => actor.post('delete', { _id: e._id }, r => { this.remove(); set_index(); }))
                    .show();
            })
        })

        set_index();
    },
    create_editor: function () { },
}
