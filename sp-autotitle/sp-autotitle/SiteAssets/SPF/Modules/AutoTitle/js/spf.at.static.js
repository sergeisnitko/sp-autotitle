
window.console = window.console || {
    log: function(val) {

    }
};

var spf = spf || {};
spf.Core = spf.Core || {};
spf.Const = spf.Const || {};

spf.ensureMQuery = function (fn) {
    EnsureScriptFunc('mQuery.js', 'm$', function() {
        if (fn)
            fn();
    });
};


spf.urlParam = function (name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (results == null) {
        return null;
    } else {
        return results[1] || 0;
    }
};


spf.UnMask = function (notification) {
    var table = m$("#FormTable");
    if (table) {
        table.find("input, textarea, select").forEach(function (el, i, a) {
            a[i].removeAttribute("disabled");
        });
        var link = m$("#SPFAddLink");
        if (link.length > 0) {
            link[0].style.display = "inline";
        }
    }
    if (notification) {
        SP.UI.Notify.removeNotification(notification);
    }
};

spf.parseFieldAttr = function (attr) {
    var returnValue = undefined;
    if (attr) {
        returnValue = attr.value;
    }
    return returnValue;
};

spf.parseFieldXml = function (xml) {
    var xmlDoc = "";
    if (window.DOMParser) {
        var parser = new DOMParser();
        xmlDoc = parser.parseFromString(xml, "text/xml");
    } else {
        xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
        xmlDoc.async = false;
        xmlDoc.loadXML(xml);
    }

    return xmlDoc.childNodes[0].attributes;
};

spf.checkBoolFromString = function (value) {
    var returnValue = false;
    if (value) {
        if (value.toLowerCase() === "true") {
            returnValue = true;
        }
    }
    return returnValue;
};

spf.GetLocalized = function(key) {
    var value = key;
    if (spf.Resources) {
        if (spf.Resources[key]) {
            value = spf.Resources[key];
        }
    }
    return value;
};

