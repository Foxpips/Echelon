/// <reference path="../Helpers/ajax-helper.js" />
/*jshint esversion: 6 */

var AvatarControl = function (ajaxhelper) {
    var self = this;
    self.avatarUrl = "";

    //public methods
    self.setAvatarUrl = function (email) {
        ajaxhelper.Get(`https://localhost/Echelon/api/avatar?email=${email}`, response => self.avatarUrl = response);

        //        const settings = {
        //            "async": true,
        //            "crossDomain": true,
        //            "url": `https://localhost/Echelon/api/avatar?email=${email}`,
        //            "method": "GET",
        //            "headers": {
        //                "cache-control": "no-cache",
        //                "postman-token": "07ffdd6a-0944-c788-30bf-3bdcc8d4eae1"
        //            }
        //        };



        //        $.ajax(settings).done(response => {
        //            self.avatarUrl = response;
        //        });
    };
};


