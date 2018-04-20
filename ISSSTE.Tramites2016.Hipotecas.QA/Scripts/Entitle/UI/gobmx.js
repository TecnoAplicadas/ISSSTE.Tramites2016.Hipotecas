"use strict";

function _addEvent(e, evt, handler) {

    if (typeof handler !== "function")
        return;

    if (e.addEventListener)
        e.addEventListener(evt, handler, false);
    else if (e.attachEvent)
        e.attachEvent("on" + evt, handler);
    else {
        var oldHandler = e["on" + evt];
    }
}

var _events = ["ready"];
var _myLib = function(item) {
    function eventWorker(item, event) {
        this.add = function(handler) {
            _addEvent(item, event, handler);
        };
    }

    for (var i = 0; i < _events.length; i++)
        this[_events[i]] = (new eventWorker(item, _events[i])).add;
};
var $gmx = function(item) {
    return new _myLib(item);
};
// Custom event for ready gobmx-framework

(function() {
    //var root = 'http://gobmx.dosdev.com/framework/';
    var root = "http://framework-gb.cdn.gob.mx/";
    var path = root + "assets/";
    var imagesPath = path + "images/";
    var scriptsPath = path + "scripts/";
    var stylesPath = path + "styles/";
    var myVar;

    function addScript(src, isHead) {
        var s = document.createElement("script");
        s.setAttribute("src", src);

        if (isHead === true) {
            document.getElementsByTagName("head")[0].appendChild(s);
        } else {
            document.body.appendChild(s);
        };
    };

    function addStyleSheet(src) {
        var head = document.head;
        var link = document.createElement("link");

        link.type = "text/css";
        link.rel = "stylesheet";
        link.href = src;
        head.appendChild(link);
    };


    if (!window.Modernizr) {
        var script = document.createElement("script");
        script.type = "text/javascript";
        script.src = scriptsPath + "vendor/modernizr.js";
        document.getElementsByTagName("head")[0].appendChild(script);

        window.onload = function() {
            loadScripts();
        };

    } else {
        window.onload = function() {
            loadScripts();
        };
    };

    function setTime(func) {
        myVar = setTimeout(function() { func }, 100);
    };

    //addStyleSheet( stylesPath + 'main.css' );

    function loadScripts() {
        if (!window.jQuery) {

            Modernizr.load([
                {
                    load: "//ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js",
                    complete: function() {
                        if (!window.jQuery) {
                            Modernizr.load(root + "jquery.min.js");
                        }
                    }
                }, {
                    load: [scriptsPath + "plugins.js", scriptsPath + "main.js"]
                }
            ]);
        } else {
            Modernizr.load([
                {
                    load: [scriptsPath + "plugins.js", scriptsPath + "main.js"]
                }
            ]);
        }


    }

})();