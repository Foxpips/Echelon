var NotificationControl = function () {
    var self = this;

    (function () {
        self.windowActive = true;

        $(window)
            .focus(function () {
                self.windowActive = true;
            });

        window.onblur = function () {
            self.windowActive = false;
        };
    })();

    self.sendNotification = function (author, content) {
        if (self.windowActive === false) {
            if (window.Notification.permission === "granted") {
                var notification = new window.Notification(author,
                {
                    icon: content.avatar,
                    body: content.message,
                    tag: author
                });

                notification.onclick = function() {
                    window.focus();
                    notification.close();
                };
            }
            else if (window.Notification.permission !== "denied") {
                Notification.requestPermission(function (permission) {
                });
            }
        }
    };
};