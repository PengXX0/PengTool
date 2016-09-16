define(["jquery", "library"], function ($) {
    var ScriptTest = function () {
        $(".submit").bind("click", function () {
            var pattern = $("textarea[name='pattern']").val();
            var input = $("textarea[name='input']").val(); var array = [];
            try { array = input.match(eval(pattern.replace(/\\/g, "\\"))); } catch (e) { array = []; }
            $("textarea[name='result']").val(array); array = array == null ? [] : array; var data = [];
            var template = "<tr><td>{index}</td><td>{length}</td><td>{value}</td></tr>";
            for (var i = 0; i < array.length; i++) {
                var obj = {}; obj.index = i; obj.value = array[i]; obj.length = array[i].length; data.push(obj);
            } $("section li>table tr:gt(0)").remove();
            $("section li>table ").append(template.render(data)).show();
        });
    };
    return ScriptTest;
});