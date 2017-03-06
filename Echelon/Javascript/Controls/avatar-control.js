/// <reference path="../Helpers/ajax-helper.js" />
/*jshint esversion: 6 */

var AvatarControl = function (ajaxhelper) {
    var self = this;
    self.avatarUrl = "";

    //public methods
    self.setAvatarUrl = function (email) {
        ajaxhelper.Get(`https://localhost/Echelon/api/avatar?email=${email}`, response => {
            self.avatarUrl = response;
            for (let avatar of $(".avatar--user")) {
              $(avatar).attr("src", response);
            }
        });
    };
};


