define(['jquery'], function ($, layer) {
    var getCookie = function (name) {
        var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
        if (arr = document.cookie.match(reg))
            return unescape(arr[2]);
        else
            return null;
    };
    var setCookie = function (name, value, time) {
        var Days
        if (time) {
            Days = time;
        } else {
            Days = 1;
        }
        var exp = new Date();
        //exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
        exp.setTime(exp.getTime() + Days * 60 * 60 * 1000);//1小时
        document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
    };
    var delCookie = function (name) {
        var exp = new Date();
        exp.setTime(exp.getTime() - 1);
        var cval = getCookie(name);
        if (cval != null)
            document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
    };
    var html_encode = function (str) {
        var s = "";
        str = str + "";
        if (str.length == 0) return "";
        s = str.replace(/&/g, "");
        s = s.replace(/</g, "");
        s = s.replace(/>/g, "");
        s = s.replace(/ /g, "");
        s = s.replace(/\'/g, "");
        s = s.replace(/\"/g, "");
        s = s.replace(/\n/g, "");
        return s;
    }
    var scrollTop = function () {
        $(".return-top").click(function () {
            $("html,body").animate({ "scrollTop": 0 }, 1000);
        });
        $(window).scroll(function () {
            var Sctop = $(window).scrollTop();
            Sctop > 100 ? $(".return-top").show() : $(".return-top").hide();
        });
    }
    
    return { getCookie: getCookie, setCookie: setCookie, delCookie: delCookie, html_encode: html_encode, scrollTop: scrollTop };
})