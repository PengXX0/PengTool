define(['jquery'], function ($) {
    var Pinyin = function () {
        $("textarea[name='input']").bind("input", function () {
            if (this.value.trim().length == 0) { return false; }
            $.ajax({
                cache: false,
                type: "POST",
                data: "content=" + this.value,
                url: "/Pinyin/Pinyin",
                success: function (data) {
                    $("textarea[name='result']").val(data);
                }
            });
        });
    };
    return Pinyin;
});