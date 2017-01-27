var notifier = function () {
    var self = this;

    self.init = function () {
        self.windowActive = true;

        $(window)
            .focus(function () {
                self.windowActive = true;
            });

        window.onblur = function () {
            self.windowActive = false;
        };
    };

    self.sendNotification = function (author, message) {
        if (self.windowActive === false) {
            var notification = new window.Notification(author,
            {
                icon: "https://localhost/Echelon/assets/imgs/spinner-white.gif",
                body: message,
                tag: author
            });

            notification.onclick = function () {
                window.focus();
                notification.close();
            };
        }
    };
};