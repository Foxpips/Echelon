var AjaxHelper = function () {
    var self = this;

    self.Get = function (url, callback) {
        ajaxRequest("GET", url, null, callback);
    }

    self.Post = function (url, data, callback) {
        ajaxRequest("POST", url, data, callback);
    }

    function ajaxRequest(requestType, url, dataToSend, callback) {

        if (dataToSend != null) {
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
                    if (callback != null || callback != undefined) {
                        if (response != null || response !== undefined) {
                            callback(response);
                        } else {
                            callback();
                        }
                    }
                } catch (e) {
                    console.error(`An unexpected error occured please try again! \n${e.message}`);
                }
                return response;
            }).fail((xhr, status, error) => {
                console.log(xhr.responseText);
                console.error(`An unexpected error occured please try again! \n${error}`);
            }).complete(() => {
            });
    }
}

