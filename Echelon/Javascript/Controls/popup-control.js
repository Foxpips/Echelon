/*jshint esversion: 6 */
var PopupControl = function() {
    var self = this;

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "onclick": null,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-full-width",
        "preventDuplicates": true,
        "showDuration": "300",
        "hideDuration": "100",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    self.Information = (text, onclickCallback) => {
        toastr.clear();
        toastr.options.onclick = onclickCallback;
        toastr.info(text);
    };

    self.Error = (text, onclickCallback) => {
        toastr.clear();
        toastr.options.onclick = onclickCallback;
        toastr.error(text);
    };

    self.Success = (text) => {
        toastr.clear();
        toastr.success(text);
    };

    self.Warning = (text, onclickCallback) => {
        toastr.clear();
        toastr.options.onclick = onclickCallback;
        toastr.warning(text);
    };
};