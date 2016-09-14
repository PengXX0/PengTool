require.config({
    baseUrl: window.location.origin + "/content/scripts/",
    paths: {
        jquery: 'lib/jquery',
        underscore: 'lib/underscore',
        backbone: 'lib/backbone',
        router: 'router',
        masonry: 'lib/masonry',
        bridget: 'lib/bridget',
        imgloaded: 'lib/imgloaded',
        //header: 'mod/header/header',
        skrollr: 'lib/skrollr',
        parallax: 'lib/parallax',
        page: 'lib/page',
        util: 'util/util',
        qrcode: "lib/qrcode",
        jsonformater: "lib/jsonformater",
        jsonformatercss: "../css/jsonformater",
        library: "lib/library",
        //animate: 'http://static.niuduz.com/web/css/animate',
        //layer: 'lib/layer',
        //layercss: 'http://static.niuduz.com/web/css/layer',
        //swiper: 'lib/swiper',
        //swipercss: 'http://static.niuduz.com/web/css/swiper'
    },
    map: {
        '*': { 'css': 'lib/css' }
    },
    shim: {
        'underscore': {
            exports: '_'
        },
        'jquery': {
            exports: '$'
        },
        'backbone': {
            deps: ['underscore', 'jquery'],
            exports: 'Backbone'
        },
        'masonry': {
            deps: ['jquery']
        },
        'imgloaded': {
            deps: ['jquery']
        },
        'parallax': {
            deps: ['jquery']
        },
        'swiper': {
            deps: ['jquery']
        },
        'layer': {
            deps: ['jquery'],
            exports: 'layer'
        },
        'page': {
            deps: ['jquery'],
            exports: 'kkpager'
        },
        "qrcode": {
            deps: ['jquery'],
            exports: "qrcode"
        },
        "jsonformater": {
            deps: ["jquery"],
            exports: "jsonformater"
        }
    }
});
require(['router'], function (router) {
    router.app.init();
});