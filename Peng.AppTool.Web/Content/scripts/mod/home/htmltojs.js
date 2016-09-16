define(["jquery"], function ($) {
    function double() {
        var isArraySel = $("#arrays-sel").prop("checked");
        var htmlArr = $("textarea[name='input']").val().replace(/\\/g, "\\\\").replace(/\\/g, "\\/").replace(/\'/g, "\\\'").replace(/\"/g, "\\\"").split('\n');
        var len = htmlArr.length;
        var outArr = [];
        if (isArraySel) {
            outArr.push("[");
            jQuery.each(htmlArr, function (index, value) {
                if (value !== "") {
                    if (index === len - 1) {
                        outArr.push("\"" + value + "\"");
                    } else {
                        outArr.push("\"" + value + "\",\n");
                    }
                }
            });
            outArr.push("].join(\"\");");
        } else {
            jQuery.each(htmlArr, function (index, value) {
                if (value !== "") {
                    if (index === len - 1) {
                        outArr.push("\"" + value + "\";");
                    } else {
                        outArr.push("\"" + value + "\"+\n");
                    }
                }
            });
        }
        $("textarea.result").text(outArr.join(""));
    }
    function single() {
        var isArraySel = $("#arrays-sel").prop("checked");
        var htmlArr = $("textarea[name='input']").val().replace(/\\/g, "\\\\").replace(/\\/g, "\\/").replace(/\'/g, "\\\'").split('\n');
        var len = htmlArr.length;
        var outArr = [];
        if (isArraySel) {
            outArr.push("[");
            jQuery.each(htmlArr, function (index, value) {
                if (value !== "") {
                    if (index === len - 1) {
                        outArr.push("\'" + value + "\'");
                    } else {
                        outArr.push("\'" + value + "\',\n");
                    }
                }

            });
            outArr.push("].join(\"\");");
        } else {
            jQuery.each(htmlArr, function (index, value) {
                if (value !== "") {
                    if (index === len - 1) {
                        outArr.push("\'" + value + "\';");
                    } else {
                        outArr.push("\'" + value + "\'+\n");
                    }
                }
            });
        }
        $("textarea.result").text(outArr.join(""));
    }

    var HtmlToJs = function () {
        $("textarea[name='input'],textarea.result").bind("mouseover", function () {
            this.focus();
            this.select();
        });
        $(".single").bind("click", single);
        $(".double").bind("click", double);
    };
    return HtmlToJs;
});