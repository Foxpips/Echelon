/// <reference path="../VsDocs/jQuery-vsdoc.2.1.0.js" />
/*jshint esversion: 6 */

var AjaxHelper = function() {
    var self = this;

    //public methods
    self.Get = function(url, callback) {
      return  ajaxRequest("GET", url, null, callback);
    };

    self.Post = function(url, data, callback) {
        ajaxRequest("POST", url, JSON.stringify(data), callback);
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
                    console.error(`An unexpected error occured please try again! \n${e.message}`);
                }
                return response;
            })
            .fail((xhr, status, error) => {
                console.log(xhr.responseText);
                console.error(`An unexpected error occured please try again! \n${error}`);
            })
            .complete(() => {
            });
    }
};

