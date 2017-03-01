/*jshint esversion: 6 */

var NotificationControl = function () {
    var self = this;
    self.windowActive = true;

    //constructor
    (function () {
        $(window).focus(() => { self.windowActive = true; });
        window.onblur = () => { self.windowActive = false; };
    })();

    //public methods
    self.sendNotification = function (content) {
        if (self.windowActive === false) {
            if (window.Notification.permission === "granted") {
                var notification = new window.Notification(content.username,
                {
                    icon: content.avatar,
                    body: content.message,
                    tag: content.username
                });

                notification.onclick = () => {
                    window.focus();
                    notification.close();
                };
            }
            else if (window.Notification.permission !== "denied") {
                Notification.requestPermission(permission => {
                });
            }
        }
    };
};