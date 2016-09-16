define(["jquery", "jsonformater", "css!jsonformatercss"], function ($) {
    var Format = function () {
        var jformats = function () {
            var option = {
                dom: '.result',
                isCollapsible: $('.collapsible-view').prop('checked'),
                quoteKeys: $('.quoteKeys').prop('checked'),
                tabSize: $('input[name=tab]:checked').val()
            };
            JF.initFormat(option).format($('textarea[name="input"]').val());
        };
        $(".format").bind('click', jformats);
        $('.expandAll').bind('click', JF.expandAll);
        $('.collapseAll').bind('click', JF.collapseAll);
        $('.tab, .collapsible-view, .quoteKeys').change(jformats);
        $('.expand').bind("click", function () { JF.collapseLevel($(this).data('level')); });
    };
    return Format;
});