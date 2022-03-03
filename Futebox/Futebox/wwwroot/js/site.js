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



function Calendar() {
    var Cal = function (divId) {

        //Store div id
        this.divId = divId;

        // Days of week, starting on Sunday
        this.DaysOfWeek = [
            'Sun',
            'Mon',
            'Tue',
            'Wed',
            'Thu',
            'Fri',
            'Sat'
        ];

        // Months, stating on January
        this.Months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];

        // Set the current month, year
        var d = new Date();

        this.currMonth = d.getMonth();
        this.currYear = d.getFullYear();
        this.currDay = d.getDate();

    };

    // Goes to next month
    Cal.prototype.nextMonth = function () {
        if (this.currMonth == 11) {
            this.currMonth = 0;
            this.currYear = this.currYear + 1;
        }
        else {
            this.currMonth = this.currMonth + 1;
        }
        this.showcurr();
    };

    // Goes to previous month
    Cal.prototype.previousMonth = function () {
        if (this.currMonth == 0) {
            this.currMonth = 11;
            this.currYear = this.currYear - 1;
        }
        else {
            this.currMonth = this.currMonth - 1;
        }
        this.showcurr();
    };

    // Show current month
    Cal.prototype.showcurr = function () {
        this.showMonth(this.currYear, this.currMonth);
    };

    // Show month (year, month)
    Cal.prototype.showMonth = function (y, m) {

        var d = new Date()
            // First day of the week in the selected month
            , firstDayOfMonth = new Date(y, m, 1).getDay()
            // Last day of the selected month
            , lastDateOfMonth = new Date(y, m + 1, 0).getDate()
            // Last day of the previous month
            , lastDayOfLastMonth = m == 0 ? new Date(y - 1, 11, 0).getDate() : new Date(y, m, 0).getDate();


        var html = '<table class="table">';

        // Write selected month and year
        html += '<thead><tr>';
        html += '<td colspan="7">' + this.Months[m] + ' ' + y + '</td>';
        html += '</tr></thead>';


        // Write the header of the days of the week
        html += '<tr class="days">';
        for (var i = 0; i < this.DaysOfWeek.length; i++) {
            html += '<td>' + this.DaysOfWeek[i] + '</td>';
        }
        html += '</tr>';

        // Write the days
        var i = 1;
        do {

            var dow = new Date(y, m, i).getDay();

            // If Sunday, start new row
            if (dow == 0) {
                html += '<tr>';
            }
            // If not Sunday but first day of the month
            // it will write the last days from the previous month
            else if (i == 1) {
                html += '<tr>';
                var k = lastDayOfLastMonth - firstDayOfMonth + 1;
                for (var j = 0; j < firstDayOfMonth; j++) {
                    html += '<td class="not-current">' + k + '</td>';
                    k++;
                }
            }

            // Write the current day in the loop
            var chk = new Date();
            var chkY = chk.getFullYear();
            var chkM = chk.getMonth();
            if (chkY == this.currYear && chkM == this.currMonth && i == this.currDay) {
                html += '<td class="today">' + i + '</td>';
            } else {
                html += '<td class="normal">' + i + '</td>';
            }
            // If Saturday, closes the row
            if (dow == 6) {
                html += '</tr>';
            }
            // If not Saturday, but last day of the selected month
            // it will write the next few days from the next month
            else if (i == lastDateOfMonth) {
                var k = 1;
                for (dow; dow < 6; dow++) {
                    html += '<td class="not-current">' + k + '</td>';
                    k++;
                }
            }

            i++;
        } while (i <= lastDateOfMonth);

        // Closes table
        html += '</table>';

        // Write HTML to the div
        document.getElementById(this.divId).innerHTML = html;
    };

    // On Load of the window
    window.onload = function () {

        // Start calendar
        var c = new Cal("divCal");
        c.showcurr();

        // Bind next and previous button clicks
        //getId('btnNext').onclick = function () {
        //    c.nextMonth();
        //};
        //getId('btnPrev').onclick = function () {
        //    c.previousMonth();
        //};
    }

    // Get element by id
    function getId(id) {
        return document.getElementById(id);
    }
}

Calendar();



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

function TextFitness(selector, target = '.fitness-target') {
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
}