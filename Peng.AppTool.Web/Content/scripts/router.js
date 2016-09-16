define(['underscore', 'backbone', 'jquery', 'util'], function (_, Backbone, $, util) {
    window.app = {
        Views: {},
        Extensions: {},
        Collections: {},
        Models: {},
        Host: window.location.origin,
        Router: null,
        SingleId: null,
        //Tab: function () { require(["header"], function (header) { header(); }); },
        //Statis: function () { require(["mod/statis/statis"], function (statis) { statis(); }); },
        init: function () {
            var router = new app.Router();
            Backbone.history.start({ pushState: true, hashChange: false });
            //this.Tab(); this.Statis();
            return router;
        }
    };

    app.Router = Backbone.Router.extend({
        routes: {
            "": "index",
            "Home/Index": "index",
            "Home/HtmlToJs": "HtmlToJs",
            "Json/Format": "Format",
            "RegExp/ScriptTest": "ScriptTest",
            "RegExp/CsharpTest": "CsharpTest",
            "Pinyin/Pinyin": "Pinyin",
            //'cf/product': 'cfproductlist',
            //'contact': 'contact',
            //'funclist': 'funclist',
            //'product': 'product',
            //'price': 'price',
            //'platforms': 'platform',
            //'news': 'news',
            //'news/type/:id': 'news',
            //'news/:id': 'newsdetail',
            //'platforms/:id': 'platforms',
            //'supermarket/entry': 'entry'
        },
        index: function () {
            this.loadModule("Home", "Index");
        },
        HtmlToJs: function () {
            this.loadModule("Home", "HtmlToJs");
        },
        Format: function () {
            this.loadModule("Json", "Format");
        },
        ScriptTest: function () {
            this.loadModule("RegExp", "ScriptTest");
        },
        CsharpTest: function () {
            this.loadModule("RegExp", "CsharpTest");
        },
        Pinyin: function () {
            this.loadModule("Pinyin", "Pinyin");
        },
        loadModule: function (module, action, controller) {
            var moduleRoute = ['mod', module, action].join('/');
            require([moduleRoute], function (callback) {
                if (!callback) {
                    if (confirm("模块加载失败，是否重新加载？")) {
                        window.location.reload();
                    }return false;}
                callback(controller);
                return true;
            });
        }
    });
    util.scrollTop();
    return { app: app };
});