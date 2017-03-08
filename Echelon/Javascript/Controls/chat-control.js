/*jshint esversion: 6 */
var ChatControl = function (notificationControl, avatarControl) {
    var self = this;
    var lastOtherAuthor = "";
    var $window = $(window);

    var $body = $("body");
    var $loading = $("#loading");
    var $loadingIcon = $(".loading__icon");
    var container = $("#container");

    var messageContainer = document.getElementById("messages");
    var $chatWindow = container.find("#messages");
    var $participants = container.find("#users");

    //constructor
    (function () {
        if ($chatWindow.length !== 0) {
            const psize = $window.height() - 150;
            $chatWindow.height(psize);

            $window.resize(() => {
                const rsize = $window.height() - 150;
                $chatWindow.height(rsize);
                $chatWindow.scrollTop(messageContainer.scrollHeight);
            });
        }
    })();

    //public methods
    self.setOnline = function () {
        setTimeout(() => {
            var members = [];

            for (let value of currentChannel._membersEntity.members.entries()) {
                members.push(value[1]._identity);
            }

            avatarControl.setParticipantAvatars(members, result => {
                $participants.append($(`<div class="sidebar__participant"><img class="avatar avatar--other avatar--participant" src=${result.url} alt="avatar"><div class="sidebar__participant--username">${result.email}</div></div>`));
            });
        }, 1000);
    };

    self.setupChannel = function (currentChannel, identity) {
        currentChannel.join()
            .then(() => {
                self.printToLoading(`Joined channel as <span class="me">${identity.username}</span>.`, true);
                self.chatHistory(identity.email);
                self.setOnline();
            });

        currentChannel.on("messageAdded", data => {
            var content = JSON.parse(data.body);
            if (identity.email === data.author) {
                self.printMessage(data.timestamp, content);
            } else {
                self.printReceivedMessage(data.timestamp, content);
                notificationControl.sendNotification(content);
            }
        });
    };

    self.printToLoading = function (infoMessage, asHtml, initialPadding) {
        var $msg = initialPadding ? $("<div class=\"info loading__text loading__text--initial\">") : $("<div class=\"info loading__text\">");
        if (asHtml) {
            $msg.html(infoMessage);
        } else {
            $msg.text(infoMessage);
        }
        $loadingIcon.append($msg);
    };

    self.chatHistory = function (username) {
        setTimeout(() => {
            for (let i = 90; i < 100; i++) {
                var message = currentChannel.messages[i];
                var content = JSON.parse(message.body);
                if (username === message.author) {
                    self.printMessage(message.timestamp, content);
                } else {
                    self.printReceivedMessage(message.timestamp, content);
                }
            }
            $body.removeAttr("style");
            $(".skiptranslate").not(".goog-te-gadget").remove();
            $loading.hide();
        }, 1000);

        $(".goog-te-combo").on("change", () => {
            setTimeout(() => {
                $body.removeAttr("style");
                $(".skiptranslate").not(".goog-te-gadget").remove();
            }, 1000);
        });
    };

    self.printMessage = function (timestamp, content) {
        const $container = $("<div class=\"message-container\">");
        const $user = $("<div class=\"message-container__username message-container__username--me\">").text(content.username);
        const $time = $("<div class=\"message-container__timestamp\">").text(` ${timestamp.toLocaleTimeString()}`);
        const $message = $("<div class=\"message-container__message message-container__message--me\">").text(content.message);
        renderMessage(content.username, $message, $time, $container, $user, false);
    };

    self.printReceivedMessage = function (timestamp, content) {
        const $container = $("<div class=\"message-container message-container--other\" >");
        const $user = $("<div class=\"message-container__username message-container__username--other\">").text(content.username);
        const $time = $("<div class=\"message-container__timestamp\">").text(` ${timestamp.toLocaleTimeString()}`);
        const $message = $("<div class=\"message-container__message message-container__message--other\" >").text(content.message);
        renderMessage(content.username, $message, $time, $container, $user, true, content.avatar);
    };

    //private methods
    function renderMessage(fromUser, $message, $time, $container, $user, renderAvatar, avatarUrl) {
        if (fromUser === lastOtherAuthor) {
            $container.append($message);
            $(container.find(".message-container__timestamp").last()).hide();
        }
        else {
            if (renderAvatar) {
                $container.append($(`<img class="avatar avatar--other" src="${avatarUrl}" alt="avatar">`));
            }

            if ($message.hasClass("message-container__message--me")) {
                $message.addClass("message-container__message--initial-me");
            } else {
                $message.addClass("message-container__message--initial-other");
            }


            $container.append($user).append($message);
        }

        $container.append($time);
        $chatWindow.append($container);
        $chatWindow.scrollTop($chatWindow[0].scrollHeight);
        lastOtherAuthor = fromUser;
    }
};