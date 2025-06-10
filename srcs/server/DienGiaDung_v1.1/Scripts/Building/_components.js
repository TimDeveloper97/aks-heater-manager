
window.init_toggle = (function (cls) {
    $("." + cls).each(function () {
        let target = $(this.getAttribute("data-target"));

        target.addClass(this.getAttribute("data-toggle"));
        this.addEventListener("click", () => {
            target.toggleClass("toggled");
        })
    })
})("nav-toggle");

window.init_svg = (function (cls) {
    $("svg." + cls).each(function () {
        this.classList.forEach(n => {
            let fn = icon[n];
            if (fn) {
                fn(this);
                return;
            }
        })
    })
})("icon");

window.dropdown_menu = (function (cls) {
    let itemCls = cls + "-item";
    let opened = null;
    $('.' + cls).each(function () {
        let m = $(this.lastElementChild).addClass(cls + "-menu");
        m.children().each(function () {
            if (this.tagName == "A") {
                $(this).addClass(itemCls).attr("role", "button");
            }
        });
    })
    $(document.body).click(function (e) {
        let t = e.target;
        function c() {
            if (!t) return null;
            let tc = t.classList;
            if (tc.contains(itemCls)) return null;
            if (tc.contains(cls)) return tc.contains("show") ? null : t;
            return c(t = t.parentElement);
        }
        function x() {
            if (opened) {
                opened.classList.remove("show");
                opened = null;
            }
        }

        if (!c()) { x(); return; }
        x();
        (opened = t).classList.add("show");
    })
})("dropdown");

Card.init();
Accordion.init()

window.init_search = (function () {
    let f = $("#form-search");
    let k = search.key;
    let t = search.text;
    if (!t && !k) {
        f.remove();
        return;
    }

    let i = f.children().first()
    function v() {return i.val().toLowerCase()}

    if (t) i.change(function(){t(v())});
    if (k) i.keyup(function () { setTimeout(() => k(val()), 100) });

    if (search.target) {
        f.appendTo($(search.target))
    }
})();
window.init_tools = (function () {
    $(".tools").appendTo("#mid-banner-content");
})();

window.init_nav_menu = (function () {
    let curnav = null;
    $(".nav-tabs").each(function () {
        let target = $(this.getAttribute("data-target"));
        if (curnav == null)
            curnav = target;
        else
            target.hide();

        $(this).click(function() {
            if (curnav)
                $(curnav).hide();
            (curnav = target).show();
        })
    })
})();
window.init_bags = (function () {
    $(".bag").each(function () {
        let b = $(this);
        let i = b.attr("icon");
        b.append("<div class='bag-line' />")
        if (i) b.append("<i class='bag-icon fa fa-" + i + "'></i>")
    });
})();