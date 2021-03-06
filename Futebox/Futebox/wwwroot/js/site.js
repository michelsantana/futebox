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