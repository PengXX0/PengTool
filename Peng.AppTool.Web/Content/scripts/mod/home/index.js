define(['jquery', 'qrcode'], function ($) {
    var index = function () {
        $(".one>button").click(function () {
            var src = "/home/qrcode?data=" + $(".one .input").val() + "&_=" + new Date();
            $(".one>.imgpannl").append("<img src='" + src + "'/>");
        });
        $(".two>button").click(function () {
            qrcode = new QRCode($(".two .imgpannl")[0], { width: 145, height: 145 });
            qrcode.makeCode($(".two .input").val());
            $(".two>.imgpannl>img").css("margin", "10px").css("display", "initial");
        });
    };
    return index;
});