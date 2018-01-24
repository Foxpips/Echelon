/*jshint esversion: 6 */
var PopupControl = function() {
    var self = this;

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "onclick": null,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-bottom-right",
        "preventDuplicates": true,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "8000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    self.Information = (text, onclickCallback) => {
        toastr.options.onclick = onclickCallback;
        toastr.info(text);
    };

    self.Error = (text, onclickCallback) => {
        toastr.options.onclick = onclickCallback;
        toastr.error(text);
    };

    self.Warning = (text, onclickCallback) => {
        toastr.options.onclick = onclickCallback;
        toastr.warning(text);
    };
};