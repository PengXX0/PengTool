Date.prototype.format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "H+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "f+": this.getMilliseconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3)//季度
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
};

String.prototype.replaceAll = function (oldString, newString, ignoreCase) {
    if (!RegExp.prototype.isPrototypeOf(oldString)) {
        return this.replace(new RegExp(oldString, (ignoreCase ? "gi" : "g")), newString);
    } else { return this.replace(oldString, newString); }
};

String.prototype.format = function (argsArray) {
    var matchesArray = this.match(/\{(\d+)\}/g); var result = this;
    for (var i = 0; i < matchesArray.length; i++) {
        result = result.replace(matchesArray[i], argsArray[i]);
    } return result;
};

String.prototype.render = function (arg) {
    var that = this; arg = eval(arg);
    var subRender = function (object) {
        var item = that;
        for (var name in object) { item = item.replace(eval("/\\{" + name + "\\}/g"), object[name]); }
        return item.replace(/\/\{/g, "{").replace(/\/\}/g, "}");
    };
    if (arg.constructor === Object) { return subRender(arg); }
    if (arg.constructor === Array) {
        var result = "";
        for (var i = 0; i < arg.length; i++) { result += subRender(arg[i]); }
        return result;
    }
    return "";
};

var formatJson = function (json) {
    var stringSpace = function (l) { var a = [], j; for (j = 0; j < l; j++) { a.push(' '); } return a.join(''); };
    var text = json.split("\n").join(" ");
    var t = []; var tab = 0; var inString = false;
    for (var i = 0, len = text.length; i < len; i++) {
        var c = text.charAt(i);
        if (inString && c === inString) {
            if (text.charAt(i - 1) !== '\\') { inString = false; }
        } else if (!inString && (c === '"' || c === "'")) {
            inString = c;
        } else if (!inString && (c === ' ' || c === "\t")) {
            c = '';
        } else if (!inString && c === ':') {
            c += ' ';
        } else if (!inString && c === ',') {
            c += "\n" + stringSpace(tab * 2);
        } else if (!inString && (c === '[' || c === '{')) {
            tab++; c += "\n" + stringSpace(tab * 2);
        } else if (!inString && (c === ']' || c === '}')) {
            tab--; c = "\n" + stringSpace(tab * 2) + c;
        } t.push(c);
    } return t.join('');
};

//异步js加载器
var Loader = function () { }
Loader.prototype = {
    require: function (scripts, callback) {
        this.loadCount = 0;
        this.totalRequired = scripts.length;
        this.callback = callback;

        for (var i = 0; i < scripts.length; i++) {
            this.writeScript(scripts[i]);
        }
    },
    loaded: function (evt) {
        this.loadCount++;

        if (this.loadCount == this.totalRequired && typeof this.callback == 'function') this.callback.call();
    },
    writeScript: function (src) {
        var self = this;
        var s = document.createElement('script');
        s.type = "text/javascript";
        s.async = true;
        s.src = src;
        s.addEventListener('load', function (e) { self.loaded(e); }, false);
        var head = document.getElementsByTagName('head')[0];
        head.appendChild(s);
    }
}
//用法
//var l = new Loader();
//l.require([
//    "example-script-1.js",
//    "example-script-2.js"],
//    function () {
//        // Callback
//        console.log('All Scripts Loaded');
//    });

var DOMReady = function (a, b, c) {
    b = document;
    c = 'addEventListener';
    b[c] ? b[c]('DocumentContentLoaded', a) : window.attachEvent('onload', a)
}
//用法
//DOMReady(function () {
//    alert('The DOM is Ready!');
//});








/*以下是基于jQuery的方法*/
if ($ && jQuery) {
    $.fn.serializeJson = function () {
        var serializeObj = {};
        var array = this.serializeArray();
        $(array).each(function () {
            if (serializeObj[this.name]) {
                if ($.isArray(serializeObj[this.name])) {
                    serializeObj[this.name].push(this.value);
                } else { serializeObj[this.name] = [serializeObj[this.name], this.value]; }
            } else { serializeObj[this.name] = this.value; }
        }); return serializeObj;
    };
}

