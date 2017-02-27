/*jshint esversion: 6 */

var AvatarControl = function() {
    var self = this;
    self.avatarUrl = "";

    self.setAvatarUrl = function(email) {
        const settings = {
            "async": true,
            "crossDomain": true,
            "url": `https://localhost/Echelon/api/avatar?email=${email}`,
            "method": "GET",
            "headers": {
                "cache-control": "no-cache",
                "postman-token": "07ffdd6a-0944-c788-30bf-3bdcc8d4eae1"
            }
        };

        $.ajax(settings)
            .done(function(response) {
                self.avatarUrl = response;
            });
    };
};


