// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

jQuery.each(["put", "delete"], function (i, method) {
    jQuery[method] = function (url, data, callback, type) {
        if (jQuery.isFunction(data)) {
            type = type || callback;
            callback = data;
            data = undefined;
        }

        return jQuery.ajax({
            url: url,
            type: method,
            dataType: type,
            data: data,
            success: callback
        });
    };
});

jQuery.SendToast = (title, message) => {

    $.get(`?handler=toast&title=${title}&message=${message}`, (t) => {
        var toast = $(t);
        $('#toast-container').append(toast);
        toast.toast('show');
        toast.addEventListener('hidden.bs.toast', function () {
            toast.remove();
        })
    })

}

function ChangeMode(currentMode) {
    if (currentMode == 0) window.location = window.location.href + "?debug=0";
    if (currentMode == 1) window.location = window.location.href + "?debug=1";
}

$('#debugmode').change(() => {
    var actived = $('#debugmode').prop('checked');
    ChangeMode(actived ? 1 : 0);
})

$(document).ready(() => {
    if (window.location.href.indexOf("debug") > -1) {
        var l = window.location;

        var qs = window.location.search?.replace(/debug../, '');
        qs = qs?.replace('?', '');

        var route = l.origin + l.pathname + (qs ? `?${qs}` : '');

        window.location = route;
    }
});


/*global jQuery */
/*!
* FitText.js 1.2
*
* Copyright 2011, Dave Rupert http://daverupert.com
* Released under the WTFPL license
* http://sam.zoy.org/wtfpl/
*
* Date: Thu May 05 14:23:00 2011 -0600
*/

(function ($) {

    $.fn.fitText = function (kompressor, options) {

        // Setup options
        var compressor = kompressor || 1,
            settings = $.extend({
                'minFontSize': Number.NEGATIVE_INFINITY,
                'maxFontSize': Number.POSITIVE_INFINITY
            }, options);

        return this.each(function () {

            // Store the object
            var $this = $(this);

            // Resizer() resizes items based on the object width divided by the compressor * 10
            var resizer = function () {
                $this.css('font-size', Math.max(Math.min($this.width() / (compressor * 10), parseFloat(settings.maxFontSize)), parseFloat(settings.minFontSize)));
            };

            // Call once to set.
            resizer();

            // Call on resize. Opera debounces their resize by default.
            $(window).on('resize.fittext orientationchange.fittext', resizer);

        });

    };

})(jQuery);

var TextFitness = {
    v1: function TextFitness(selector, target = '.fitness-target') {
        var childTarget = target;
        $(selector).each((i, el) => {
            var parentWidth = $(el).width();
            var parentHeight = $(el).height();

            if (parentHeight < parentWidth) {
                while ($(el).find(childTarget).height() < parentHeight) {
                    var size = $(el).find(childTarget).attr('font-size') || '1';
                    $(el).find(childTarget).css('font-size', `${size++}px`);
                    $(el).find(childTarget).attr('font-size', size);
                }
            }
            else {
                while ($(el).find(childTarget).width() < parentWidth) {
                    var size = $(el).find(childTarget).attr('font-size') || '1';
                    $(el).find(childTarget).css('font-size', `${size++}px`);
                    $(el).find(childTarget).attr('font-size', size);
                }
            }
        })
    },
    v2: function TextFitness(selector, target = '.fitness-target') {
        this.debug();
        var childTarget = target;
        $(selector).each((i, el) => {
            var parentWidth = $(el).width();
            var parentHeight = $(el).height();

            var check = () => $(el).find(childTarget).height() < parentHeight && $(el).find(childTarget).width() < parentWidth;

            while (check()) {
                var size = $(el).find(childTarget).attr('font-size') || '1';
                $(el).find(childTarget).css('font-size', `${size++}px`);
                $(el).find(childTarget).attr('font-size', size);
            }
        })
    },
    debug: function TextFitnessDebug(selector = '.-fitness', target = '.-fitness-target') {
        //debugger;
        var childTarget = target;
        $(selector).each((i, el) => {
            var parentWidth = $(el).width();
            var parentHeight = $(el).height();

            var check = () => $(el).find(childTarget).height() < parentHeight && $(el).find(childTarget).width() < parentWidth;

            while (check()) {
                var size = $(el).find(childTarget).attr('font-size') || '1';
                $(el).find(childTarget).css('font-size', `${size++}px`);
                $(el).find(childTarget).attr('font-size', size);
            }
        })
    }
}