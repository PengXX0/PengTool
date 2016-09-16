define(["jquery", "library"], function ($) {
    return function () {
        $(".submit").bind("click", function () {
            var template = "<tr><td>{Index}</td><td>{Success}</td><td>{Length}</td><td>{Value}</td></tr>";
            $.post("/RegExp/CsharpTest", $(".csharp-Rex").serialize(), function (data) {
                $("section li > table tr:gt(0)").remove();
                $("section li > table").append(template.render(data.Data)).show();
            });
        });
    };
});