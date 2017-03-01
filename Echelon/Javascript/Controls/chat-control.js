/*jshint esversion: 6 */
var ChatControl = function (notificationControl) {
    var self = this;
    var lastOtherAuthor = "";
    var $window = $(window);

    const $body = $("body");
    const $loading = $("#loading");
    const $loadingIcon = $(".loading__icon");
    const $skiptranslate = $(".skiptranslate");
    const container = $("#container");

    var messageContainer = document.getElementById("messages");
    var $chatWindow = container.find("#messages");
    var $conversations = container.find("#users");

    //constructor
    (function () {
        const psize = $window.height() - 150;
        $chatWindow.height(psize);

        $window.resize(() => {
            const rsize = $window.height() - 150;
            $chatWindow.height(rsize);
            $chatWindow.scrollTop(messageContainer.scrollHeight);
        });
    })();

    //public methods
    self.setOnline = function () {
        setTimeout(() => {
            for (let value of currentChannel._membersEntity.members.entries()) {
                $conversations.append($(`<div>${value[1]._identity}</div>`));
            }
        }, 1000);
    };

    self.setupChannel = function (currentChannel, identity) {
        currentChannel.join()
            .then(() => {
                self.printToLoading(`Joined channel as <span class="me">${identity.username}</span>.`, true);
                self.chatHistory(identity.username);
                self.setOnline();
            });

        currentChannel.on("messageAdded", data => {
            const content = JSON.parse(data.body);
            if (identity.email === data.author) {
                self.printMessage(data.timestamp, content);
            } else {
                self.printReceivedMessage(data.timestamp, content);
                notificationControl.sendNotification(content);
            }
        });
    };

    self.printToLoading = function (infoMessage, asHtml, initialPadding) {
        const $msg = initialPadding ? $("<div class=\"info loading__text loading__text--initial\">") : $("<div class=\"info loading__text\">");
        if (asHtml) {
            $msg.html(infoMessage);
        } else {
            $msg.text(infoMessage);
        }
        $loadingIcon.append($msg);
    };

    self.chatHistory = function (username) {
        setTimeout(() => {
            //            for (let i = 0; i < currentChannel.messages.length; i++) {
            //                const message = currentChannel.messages[i];
            //                if (username === message.author) {
            //                    self.printMessage(message.author, message);
            //                } else {
            //                    self.printReceivedMessage(message.author, message);
            //                }
            //            }
            $body.removeAttr("style");
            $skiptranslate.not(".goog-te-gadget").remove();
            $loading.hide();
        }, 1000);

        $(".goog-te-combo").on("change", () => {
            setTimeout(() => {
                $body.removeAttr("style");
                $skiptranslate.not(".goog-te-gadget").remove();
            }, 1000);
        });
    };

    self.printMessage = function (timestamp, content) {
        const $container = $("<div class=\"message-container\">");
        const $user = $("<div class=\"message-container__username message-container__username--me\">").text(content.username + ": ");
        const $time = $("<div class=\"message-container__timestamp\">").text(` ${timestamp.toLocaleTimeString()}`);
        const $message = $("<div class=\"message-container__message message-container__message--me\">").text(content.message);
        renderMessage(content.username, $message, $time, $container, $user, false);
    };

    self.printReceivedMessage = function (timestamp, content) {
        const $container = $("<div class=\"message-container message-container--other\" >");
        const $user = $("<div class=\"message-container__username message-container__username--other\">").text(content.username + ": ");
        const $time = $("<div class=\"message-container__timestamp\">").text(` ${timestamp.toLocaleTimeString()}`);
        const $message = $("<div class=\"message-container__message message-container__message--other\" >").text(content.message);
        renderMessage(content.username, $message, $time, $container, $user, true, content.avatar);
    };

    //private methods
    function renderMessage(fromUser, $message, $time, $container, $user, renderAvatar, avatarUrl) {
        if (fromUser === lastOtherAuthor) {
            $container.append($message);
            $(container.find(".message-container__timestamp").last()).hide();
        } else {
            if (renderAvatar) {
                $container.append($(`<img class="avatar avatar--other" src="${avatarUrl}" alt="avatar">`));
            }
            $container.append($user).append($message);
        }

        $container.append($time);
        $chatWindow.append($container);
        $chatWindow.scrollTop($chatWindow[0].scrollHeight);
        lastOtherAuthor = fromUser;
    }
};