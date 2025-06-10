let language = {
    init: function () {
        if (sessionStorage["language"]) return;

        sessionStorage.setItem("language", "ready")
        this.ready = true;
        if (!sessionStorage.getItem('return')) {
            for (let k of this.keywords) {
                sessionStorage.setItem(k, 'u')
            }
            for (let k of this.statements) {
                sessionStorage.setItem(k, 'b')
            }
        }
    },
    statements: [
        "return",
        "try",
        "catch",
        "for",
        "if",
        "else",
        "while",
        "case",
        "switch",
        "do",
        "break",
        "continue",
        "finally",
        "throw",
        "goto",
        "default",
        "foreach",
        "in",
        "of"
    ],
    keywords: [
        "auto",
        "get",
        "set",
        "string",
        "constructor",
        "super",
        "abstract",
        "arguments",
        "bool",
        "boolean",
        "byte",
        "char",
        "class",
        "const",
        "debugger",
        "default",
        "delete",
        "do",
        "double",
        "enum",
        "eval",
        "false",
        "final",
        "float",
        "friend",
        "function",
        "implements",
        "instanceof",
        "int",
        "interface",
        "let",
        "long",
        "namespace",
        "native",
        "new",
        "null",
        "operator",
        "package",
        "private",
        "protected",
        "public",
        "register",
        "sizeof",
        "short",
        "static",
        "struct",
        "synchronized",
        "template",
        "this",
        "throws",
        "transient",
        "true",
        "typedef",
        "typeof",
        "union",
        "using",
        "unsigned",
        "var",
        "virtual",
        "void",
        "volatile",
        "with",
        "yield",
        "object",
        "override"
    ],
    operators: {
        ",": { space: 2 },
        "+": {},
        "++": { space: 0 },
        "+=": {},
        "-": {},
        "--": { space: 0 },
        "-=": {},
        "->": { space: 0 },
        "*": {},
        "*=": {},
        "/": {},
        "/=": {},
        "//": {},
        "%": {},
        "%=": {},
        ">": {},
        ">=": {},
        ">>": {},
        ">>=": {},
        "<": {},
        "<=": {},
        "<<": {},
        "<<=": {},
        "=": {},
        "==": {},
        "===": {},
        "!": { space: 0 },
        "!=": {},
        "&": {},
        "&&": {},
        "&=": {},
        "|": {},
        "||": {},
        "|=": {},
        "~": { space: 1 },
        "~=": {},
        "(": { space: 0, single: 1 },
        ")": { space: 2, single: 1 },
        "[": { space: 1, single: 1 },
        "]": { space: 2, single: 1 },
        "{": { space: 3, single: 1 },
        "}": { space: 1, single: 1 },
        "?": {},
        ":": {},
        "::": { space: 0 },
        ".": { space: 0 },
        ".#": {},
        "...": {},
        ";": { space: 2, single: 1 },
        "\\": {},
        "\"": { single: 1 },
        "'": { single: 1 },
        "#": {},
        "@": {},
        "$": {}
    },
    typedef: {
        class: 1,
        struct: 1,
        union: 1,
        enum: 1,
    },
}

class Term {
    static source = "";
    static cchar = '';
    static cindex = 0;

    static inRange(l, u) { return (this.cchar >= l && u >= this.cchar) }
    static get isWord() { return this.inRange('a', 'z') || this.inRange('A', 'Z') || (this.cchar == '_') }
    static get isDigit() { return this.inRange('0', '9') }

    static get isLast() { return this.cindex + 1 >= this.source.length }
    static get nextChar() { return (this.isLast ? null : this.source.charAt(this.cindex + 1)) }

    static run(checkStop) {
        while (++this.cindex < this.source.length) {
            this.cchar = this.source.charAt(this.cindex);
            if (checkStop(this.cchar, this.cindex)) {
                break;
            }
        }
        return this.cindex;
    }

    static First = new Term();
    static get HTML() {
        let p = this.First;
        let s = "";
        while (p) {
            s += p.toHTML();

            let n = p.next;
            if (!n) break;

            if (n.start > p.end) {
                s += this.source.substring(p.end, n.start);
            }

            p = n;
        }
        return ("<pre>" + s + "</pre>");
    }
    static split(text) {
        language.init();

        this.source = text + ' ';
        this.cindex = -1;

        function create() {
            if (Term.isDigit) return new CNumber();
            if (Term.isWord) return new CWord();

            if (Term.cchar == '#') return new CDirective();

            if (Term.cchar == '"' || Term.cchar == "'") {
                return new CString();
            }

            if (Term.cchar == '/') {
                let n = Term.nextChar;
                switch (n) {
                    case '/': return new CppComment();
                    case '*': return new CComment();
                }
            }
            return new COperator();
        }
        function append() {
            if (head) {
                tail.next = current;
                current.prev = tail;
            }
            else {
                head = current;
            }
            tail = current;
        }

        let current = new Term();
        let head = null;
        let tail = null;

        while (true) {
            this.run(c => c > ' ');
            if (this.isLast) break;

            append(current = create());
            current.gotoEnd();

            this.cindex = current.end - 1;
        }

        this.First = head;
    }

    static format(code = document.getElementById("")) {
        let s = "";
        code.querySelectorAll("pre").forEach(node => {
            Term.split(node.innerHTML);
            s += Term.HTML;
        });

        code.innerHTML = s;
    }

    gotoEnd() {
        this.end = Term.run(c => ' ' >= c)
    }
    constructor(t) {
        this.type = t;
        this.start = this.end = Term.cindex;

        this.next = this.prev = null;
        this.toString = function () {
            return Term.source.substring(this.start, this.end)
        }
        this.tag = function () { return this.type }
        this.toHTML = function () {
            let s = ""
            for (let i = this.start; i < this.end; i++) {
                let c = Term.source.charAt(i);
                if (c == '<') c = "&lt;"
                s += c
            }

            t = this.tag();
            if (t) {
                s = "<" + t + ">" + s + "</" + t + ">";
            }
            return s;
        }
    }
}

class CWord extends Term {
    constructor() {
        super('w')

        this.gotoEnd = function () {
            this.end = Term.run(c => !(Term.isDigit || Term.isWord))
        }
        this.tag = function () {
            return sessionStorage[this.toString()];
        }
    }
}
class CString extends Term {
    constructor() {
        super('s')

        this.gotoEnd = function () {
            let s = Term.source.charAt(this.start);
            this.end = 1 + Term.run(c => {
                if (c == '\\') {
                    ++Term.cindex;
                    return false;
                }
                return c == s;
            });
        }
    }
}

class CNumber extends Term {
    constructor() {
        super('n')
        this.gotoEnd = function () {
            this.end = Term.run(c => !(c == '.' || Term.isDigit));
        }
        this.tag = function () { return '' }
    }
}

class CDirective extends Term {
    constructor() {
        super('s')
        this.gotoEnd = function () {
            this.end = Term.run(c => c < ' ');
        }
    }
}

class CComment extends Term {
    constructor() {
        super('i')

        ++Term.cindex;
        this.gotoEnd = function () {
            this.end = 2 + Term.run(c => c == '*' && Term.nextChar == '/');
        }
    }
}

class CppComment extends CComment {
    constructor() {
        super()
        this.gotoEnd = function () {
            this.end = Term.run(c => c < ' ');
        }
    }
}
class COperator extends Term {
    constructor() {
        super('o')
        this.tag = function () { return '' }
        this.gotoEnd = function () { this.end = ++Term.cindex; }
    }
}

function createCodeViewer() {

    document.body.querySelectorAll(".code").forEach(div => {

        let cur = document.createElement("code");

        function do_copy() {
            let range = new Range();

            range.setStart(cur, 0);
            range.setEnd(cur, cur.childNodes.length);

            // apply the selection, explained later below
            document.getSelection().removeAllRanges();
            document.getSelection().addRange(range);

            document.execCommand("copy");
            cur.contentEditable = false;
        }
        function show(a, b) {
            if (a.code.hidden = !b) {
                a.classList.remove("active");
            }
            else {
                a.classList.add("active");
                cur = a.code;
            }
        }


        let ul = document.createElement("ul");
        div.appendChild(ul);

        let view = document.createElement("div");
        view.className = "code-view";
        div.appendChild(view);

        div.querySelectorAll("code").forEach(code => {
            Term.format(code);
            code.remove();

            let li = document.createElement("li");
            li.innerHTML = code.getAttribute("data-lang");
            li.code = code;

            view.appendChild(code);
            code.hidden = true;

            ul.appendChild(li);
            li.addEventListener("click", function () {
                ul.childNodes.forEach(a => show(a, false))
                show(li, true);
            });
        });
        //let actions = document.createElement("li");
        //actions.style.float = "right";
        //ul.appendChild(actions);

        let btn = document.createElement("button");
        btn.className = "btn";
        btn.innerHTML = "<i class='fa fa-copy'></i> copy";
        btn.addEventListener("click", do_copy);
        view.appendChild(btn);


        show(ul.firstElementChild, true);
    });
}
