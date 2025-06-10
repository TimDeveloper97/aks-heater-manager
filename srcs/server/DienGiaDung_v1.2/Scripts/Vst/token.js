function redirect(url, target) {
    let a = document.createElement("a");
    a.href = url ?? window.location.href;

    if (target) a.setAttribute("target", target);
    document.body.appendChild(a);

    a.click();
}

class HttpRequest {
    constructor(config = {}) {
        this.error = function (m) {
            new Toast().text(m).mode("danger").show(2);
        }
        this.success = null;

        this.send = function (url, method, data, successCallback) {

            let req = this;

            var http = new XMLHttpRequest();
            http.onload = function () {
                var e = JSON.parse(http.response);
                
                if (e.code) {
                    req.error(e.message, e.code);
                } else {
                    if (req.success) req.success(e.value, e.message);
                    if (successCallback) {
                        successCallback(e.value, e.message);
                    }
                }
            }
            if (!data) {
                data = {};
            }

            http.open(method, url)
            http.setRequestHeader('Content-type', 'application/json')
            http.send(JSON.stringify(data))
        }

        $.extend(this, config);
    }
}

let request = new HttpRequest();


/*************** STORAGE ***************/
let cookie = {
    set: function (name, value, exdays) {
        const d = new Date();
        d.setTime(d.getTime() + ((exdays ?? 1) * 24 * 60 * 60 * 1000));

        let expires = "expires=" + d.toUTCString();
        let cvalue = (typeof value === "string" ? value : JSON.stringify(value));
        document.cookie = name + "=" + cvalue + ";" + expires + ";path=/";
    },
    get: function (name) {
        let cname = name + "=";
        let decodedCookie = decodeURIComponent(document.cookie);
        let ca = decodedCookie.split(';');
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(cname) == 0) {
                let s = c.substring(cname.length, c.length);
                return s;
            }
        }
        return "";
    },
    getObject: function (name, callback) {
        let o = null;
        let s = this.get(name);

        if (s) {
            try {
                o = JSON.parse(s);
            } catch {
            }
        }

        if (callback) { callback(o) }
        return o;
    },
    remove: function (name) {
        document.cookie = name + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    },
}

let session = {
    message: "Session time out",
    timeout: 20,
    remain: 0,
    set: function (name, value) {
        let data = (typeof value === "string" ? value : JSON.stringify(value));
        sessionStorage.setItem(name, data);
    },
    get: function (name) {
        return sessionStorage.getItem(name);
    },
    getObject: function (name, callback) {
        let s = sessionStorage.getItem(name);
        let o = !s ? {} : JSON.parse(s);

        if (callback) { callback(o) }
        return o;
    },
    getArray: function (name, callback) {
        let s = sessionStorage.getItem(name);
        let a = !s ? [] : JSON.parse(s);

        if (callback) { callback(a) }
        return a;

    },
    loggedIn: function (token) {
        this.set("token", token)
        setTimeout(() => request.send("/home/login", "POST", { id: token },
            r => redirect("/" + r.Role)), 250)
        
        return this;
    },
    updateProfile: function (v) {
        this.call("myprofile", v, () => {
            user.profile = v;
            this.set("user", user);
            redirect("/home")
        });
    },
    restart: function () {
        this.remain = this.timeout;
    },
    start: function (mins) {
        session.ready();
        return this;
    },
    ready: function () { },
    end: function (url) {
        sessionStorage.clear("user");
        request.post("account/logout", { "token": actor.token },
            function () { if (url) redirect(url) },
            function () { })

        if (!url) {
            let dlg = $("<div><h5>System</h5><section><p>" + this.message + "</p></section></div>")
                .card("warning")
                .dialog();

            dlg.closing = function () {
                redirect("/login");
            }
        }
    },
    call: function (action, value, successCallback, errorCallback) {
        request.post(user.Role + "/" + action, value ?? {}, successCallback, errorCallback);
    },
    async: function (waitTarget, prog, action, value, successCallback, errorCallback) {
        let content = $(waitTarget);
        let childs = content.children().each(function () { this.remove() });

        if (!prog) {
            prog = $("<span class='fa fa-spinner fa-spin'></span>")
        }
        prog.appendTo(content);

        function end() {
            prog.remove();
            content.append(childs);
        }
        this.call(action, value, v => {
            end();
            if (successCallback) successCallback(v);
        }, m => {
            end();
            if (errorCallback) {
                errorCallback;
            } else {
                new Toast().title("Error").mode("danger").text(m).show();
            }
        });
    },

    loading: function (target, action, value, successCallback) {
        let div = $("<div class='data-loading'><span class='fa fa-spinner fa-spin'></span> loading...</div>")
        this.async(target, div, action, value, successCallback);
    },
    upload: function (action, path, file, progContent, completedCallback) {
        let reader = new FileReader();
        let prog = $progressBar();
        //let childs = $(progContent).children().each(function () { this.remove() })
        //prog.appendTo(progContent);

        let old = request.onprogress;
        reader.onload = function () {
            let s = reader.result;
            prog.total(s.length);

            s = s.substr(s.indexOf(',') + 1);

            request.onprogress = function (t, v) {
                prog.val(v);
            }
            session.async(progContent, prog, action, {
                code: actionCode.Insert,
                value: s,
                path: path + '/' + file.name,
            }, () => {
                prog.val(s.length);
                request.onprogress = old;

                if (completedCallback) {
                    completedCallback();
                }

            }, () => request.onprogress = old)
        }
        prog.wait(100, () => {
            reader.readAsDataURL(file);
        })
    },
    download: function (data, filename, type) {
        let a = document.createElement("a");
        a.download = filename;

        if (type) {
            let blob = new Blob([data], {
                type: "application/" + type,
            });
            a.href = URL.createObjectURL(blob);
        }
        else {
            a.href = data;
        }

        document.body.appendChild(a);
        a.click();
        a.remove();
    },
}

class Actor {
    constructor(role) {

        function u(a) { return "/api/" + role + '/' + a }
        this.post = function(action, value, successCallback) {
            let data = {
                token: cookie.get("token"),
            }
            if (value) data.value = value;
            request.send(u(action), 'POST', data, successCallback);
        }
        this.get = function(action, param, successCallback) {
            request.send(u(action) + "?" + param, 'GET', null, successCallback)
        }

        this.login = function (successCallback, errorCallback) {
            let req = new HttpRequest({
                error: errorCallback,
                success: function (r) {
                    if (r) {
                        cookie.set("token", r, 1);
                    }
                    if (successCallback) {
                        setTimeout(successCallback, 250)
                    }
                }
            });
            req.send("/home/login", "POST")
        }
    }
}
