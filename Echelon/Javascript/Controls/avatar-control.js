/*jshint esversion: 6 */
var AvatarControl = function (ajaxhelper) {
    var self = this;
    var $headerAvatar = $("#headerAvatar");

    //public methods
    self.setUserAvatar = function (email) {
        ajaxhelper.Get(`${siteurl}/api/avatar?email=${email}`,
            response => {
                for (let avatar of $(".avatar--user")) {
                    $(avatar).attr("src", response);
                }
            });
    };

    self.currentAvatar = () => {
        return $headerAvatar.prop("src");
    }

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


