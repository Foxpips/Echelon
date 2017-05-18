/*jshint esversion: 6 */

var AvatarControl = function (ajaxhelper) {
    var self = this;
    self.avatarUrl = "";

    //public methods
    self.setUserAvatar = function (email) {
        ajaxhelper.Get(`${siteurl}/api/avatar?email=${email}`,
            response => {
                self.avatarUrl = response;
                avatarUrlUser = response;
                console.log(response);
                for (let avatar of $(".avatar--user")) {
                    $(avatar).attr("src", response);
                }
            });
    };

    //public methods
    self.getAvatar = function (email) {
        ajaxhelper.Get(`${siteurl}/api/avatar?email=${email}`,
            response => {
                self.avatarUrl = response;
                for (let avatar of $(".avatar--user")) {
                    $(avatar).attr("src", response);
                }
            });
    };

    self.setParticipantAvatars = function (emails, callback) {
        let data = JSON.stringify({ Emails: emails });
        ajaxhelper.Post(`${siteurl}/api/avatar/PrintPerson`,
            data,
            response => {
                for (let result of response) {
                    callback(result);
                }
            });
    };
};


