$(function () {
    setSize();
    window.onresize = setSize;
    $(".menu").find("li ul").prev("a").css("padding-left", "0").prepend('<i class="icon-close"></i>');
    $(".menu").find("a").bind("click", function () { toggleActive(this); });
    $(".debug-form.form-horizontal").bind("submit", function () { return submitForm(this); });
});

var toggleActive = function (obj) {
    if ($(obj).next().length == 0) {
        $(".menu").find("a").removeClass("active");
        $(obj).addClass("active").attr("target", "mainFrame");
    } else {
        $(obj).next("ul").toggle(200);
        $(obj).find("i").toggleClass("icon-open icon-close");
    }
};
var setSize = function () {
    var width = $(window).width();
    var height = $(window).height();
    $(".menu").css("height", (height) + "px");
    $("iframe[name='mainFrame']").attr("width", width - 310).attr("height", height - 10);
};

var submitForm = function (obj) {
    var data = $(obj).serializeJson();
    if (data.action.trim().length == 0) { $.alert({ icon: 'fa fa-spinner fa-spin', title: '提示', content: '请将表单填写完整！', confirmButton: '确定' }); return false; }
    $.ajax({
        cache: false,
        type: data.method,
        url: data.action,
        data: data.parameter,
        success: function (response) { $("textarea[name='response']").val(JSON.stringify(response)); },
        error: function () { $.alert({ icon: 'fa fa-spinner fa-spin', title: '提示', content: '请求出错了', confirmButton: '确定' }); }
    });
    return false;
};

$.fn.serializeJson = function () {
    var serializeObj = {};
    var array = this.serializeArray();
    $(array).each(function () {
        if (serializeObj[this.name]) {
            if ($.isArray(serializeObj[this.name])) {
                serializeObj[this.name].push(this.value);
            } else {
                serializeObj[this.name] = [serializeObj[this.name], this.value];
            }
        } else {
            serializeObj[this.name] = this.value;
        }
    });
    return serializeObj;
};