/// <reference path="../VsDocs/jQuery-vsdoc.2.1.0.js" />
/// <reference path="../Controls/popup-control.js" />
/*jshint esversion: 6 */

var AjaxHelper = function () {
    var self = this;
    var popupControl = new PopupControl();

    //public methods
    self.Get = function (url, callback) {
        return ajaxRequest("GET", url, null, callback);
    };

    self.Post = function (url, data, callback) {
        return ajaxRequest("POST", url, JSON.stringify(data), callback);
    };

    //private methods
    function ajaxRequest(requestType, url, dataToSend, callback) {

        return $.ajax({
            async: true,
            type: requestType,
            data: dataToSend,
            url: url,
            contentType: "application/json; charset=utf-8"
        })
             .done(response => {
                 try {
                     if (callback !== null || callback !== undefined) {
                         if (response !== null && response !== undefined) {
                             callback(response);
                         } else {
                             callback();
                         }
                     }
                 } catch (e) {
                     popupControl.Error(`A very unexpected error occured please try again! \n${e.message}`);
                 }
                 return response;
             })
             .fail((xhr, status, error) => {
                 popupControl.Error(`Error code: ${xhr.status} <br/>${xhr.responseText}`);
             })
             .complete(() => {
             });
    }
};

