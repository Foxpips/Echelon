﻿/// <reference path="../VsDocs/jQuery-vsdoc.2.1.0.js" />
/*jshint esversion: 6 */

var AjaxHelper = function() {
    var self = this;

    //public methods
    self.Get = function(url, callback) {
        ajaxRequest("GET", url, null, callback);
    };

    self.Post = function(url, data, callback) {
        ajaxRequest("POST", url, data, callback);
    };

    //private methods
    function ajaxRequest(requestType, url, dataToSend, callback) {

        if (dataToSend !== null && dataToSend !== undefined) {
            dataToSend.__RequestVerificationToken = $("[name=__RequestVerificationToken]").val();
        }

        $.ajax({
                async: true,
                type: requestType,
                data: dataToSend,
                url: url
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

