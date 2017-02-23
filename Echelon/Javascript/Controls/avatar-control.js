/*jshint esversion: 6 */

var AvatarControl = function() {
    const self = this;
    self.avatarUrl = "";

    self.setAvatarUrl = function() {
        const settings = {
            "async": true,
            "crossDomain": true,
            "url": "https://localhost/Echelon.Api/api/avatar?email=simonpmarkey%40gmail.com",
            "method": "GET",
            "headers": {
                "cache-control": "no-cache",
                "postman-token": "07ffdd6a-0944-c788-30bf-3bdcc8d4eae1"
            }
        };

        $.ajax(settings)
            .done(function(response) {
                self.avatarUrl = response;
                console.log(response + " asd");
            });
    };
};


