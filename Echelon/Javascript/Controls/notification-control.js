/*jshint esversion: 6 */

var NotificationControl = function (popupControl) {
    var self = this;
    self.windowActive = true;
    self.showWarning = true;

    //constructor
    (function () {
        $(window).focus(() => { self.windowActive = true; });
        window.onblur = () => { self.windowActive = false; };
    })();

    //public methods
    self.sendNotification = function (user, message) {
        console.log(message);
        if (self.windowActive === false) {
            if (window.Notification.permission === "granted") {
                var notification = new window.Notification(user.UserName,
                {
                    icon: user.AvatarUrl,
                    body: message,
                    tag: user.UserName
                });

                notification.onclick = () => {
                    window.focus();
                    notification.close();
                };
            }
            else if (window.Notification.permission !== "denied") {
                Notification.requestPermission(permission => {
                    popupControl.Success("Notifications are enabled");
                });
            } else if(self.showWarning) {
                self.showWarning = false;
                popupControl.Warning("Notifications are disabled");
            }
        }
    };
};