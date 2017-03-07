/// <reference path="../Helpers/ajax-helper.js" />
/*jshint esversion: 6 */

var AvatarControl = function (ajaxhelper) {
    var self = this;
    self.avatarUrl = "";

    //public methods
    self.setUserAvatar = function (email) {
        ajaxhelper.Get(`https://localhost/Echelon/api/avatar?email=${email}`,
            response => {
                self.avatarUrl = response;
                for (let avatar of $(".avatar--user")) {
                    $(avatar).attr("src", response);
                }
            });
    };

    self.setParticipantAvatars = function () {
        let data = JSON.stringify({ Emails: ["simonpmarkey@gmail.com", "smarkey@mywebgrocer.com"] });
        ajaxhelper.Post(`https://localhost/Echelon/api/avatar/PrintPerson`,
            data,
            response => {
                for (let result of response) {
                    $("#users").append($(`<div class="sidebar__participant"><img class="avatar avatar--other avatar--participant" src=${result.url} alt="avatar"><div class="sidebar__participant--username">${result.email}</div></div>`));
                }
            });
    };
};


