var config = {
    devUrl: "http://192.168.1.220:8092",
    exampleUrl: function () { return "/Api/Dev/Debug?Url=" + (document.location.host.indexOf('localhost') != -1 ? document.location.origin : config.devUrl) + "/Api" + $("#url").val(); },
    init: function () {
        $.getScript("content/js/bootstrap.js");
        $($(".panel-heading")[1]).find("a").attr("href", config.exampleUrl());
        if ($(".panel-body").length <= 1) { return false; }
        var target = $(".panel-body:eq(1)");
        target.html(target.html().replace(/RequestBase/g, "<a href='/Areas/Api/ApiDoc/RequestBase.html'><strong>RequestBase</strong></a>"));
        target.find('a').bind("click", function () { modal.showModal({ url: this.href, title: $(this).text(), cancel: "确定" }); return false; });
    }
};
$(function () { config.init(); });