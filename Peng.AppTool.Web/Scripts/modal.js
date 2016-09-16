var modal = {
    bootStrapModalHtml: ['<div class="modal fade">',
                         '    <div class="modal-dialog">',
                         '        <div class="modal-content">',
                         '            <div class="modal-header">',
                         '                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>',
                         '                <h4 class="modal-title"></h4>',
                         '            </div>',
                         '            <div class="modal-body"></div>',
                         '            <div class="modal-footer">',
                         '                <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>',
                         '                <button type="button" class="btn btn-primary">确定</button>',
                         '            </div>',
                         '        </div>',
                         '    </div>',
                         '</div>'].join(""),
    initModal: function (options) {
        this.options = $.extend({
            title: "提示",
            content: false,
            type: "info",//info、warning 、danger、success
            large: null,
            confirm: false,
            cancel: false,
            backdrop: true,
            keyboard: true,
            show: true,
            url: false
        }, options || {});
        $(".modal.fade").remove();
        $('body').append(this.bootStrapModalHtml);
        this.modal = $(".modal.fade");
        if (this.options.large == true) { this.modal.find(".modal-dialog").addClass("modal-lg"); }
        if (this.options.large == false) { modal.find(".modal-dialog").addClass("modal-sm"); }
        if (!this.options.cancel) { this.modal.find(".modal-footer button:first").remove(); } else { this.modal.find(".modal-footer button:first").text(this.options.cancel); }
        if (!this.options.confirm) { this.modal.find(".modal-footer button:last").remove(); } else { this.modal.find(".modal-footer button:last").text(this.options.confirm); }
        if (!this.options.confirm && !this.options.cancel) { this.modal.find(".modal-footer").remove(); } this.modal.find('.modal-title').text(this.options.title);
        return this.modal;
    },
    showModal: function (options) {
        this.modal = this.initModal(options); var that = this;
        if (!options.url) { this.modal.find(".modal-body").html(options.content); this.modal.modal(); }
        else { this.modal.find(".modal-body").load(options.url, function () { that.modal.modal(); }); }
    },
    alert: function (options) {
        options.large = !(options.large == undefined); options.type = (options.type == undefined) ? "info" : options.type;
        this.modal = this.initModal(options); var html = "<strong>" + options.content + "<strong>";
        this.modal.find(".modal-body").html(html).addClass("bg-" + options.type + " text-center").find("strong").addClass("text-" + options.type);
        this.modal.modal();
    }
};