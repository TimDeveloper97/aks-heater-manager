let gen_args = {
    length: 0,
    param: 0,
    input: [],
}

function rand() {
    let a = 0;
    let b = arguments[0];
    if (arguments.length > 1) {
        a = b;
        b = arguments[1]
    }

    return Math.floor(Math.random() * 16536) % (b - a) + a;
}


function split_code_template(text = "") {
    let i = 0;
    let content = "";

    gen_args = {};

    while (true) {
        let first = text.indexOf("/*{", i);
        if (first < 0) break;

        let k = first + 3;
        let last = text.indexOf("}*/", k);
        if (last < 0) break;

        let two = text.indexOf(':', k);
        if (two < 0 || two > last) break;

        let name = text.substring(k, two);
        let func = text.substring(two + 1, last);

        gen_args[name] = func.endsWith(')') ? eval(func) : func;

        content += text.substring(i, first) + gen_args[name].toString();
        i = last + 3;
    }
    return content ? content + text.substring(i) : text;
}