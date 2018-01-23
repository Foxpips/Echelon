﻿/*jshint esversion: 6 */
var ChatControl = function (notificationControl, avatarControl) {
    var self = this;
    self.identity = "";

    var lastOtherAuthor = "";
    var $window = $(window);

    var $body = $("body");
    var $loading = $("#loading");
    var container = $("#container");

    var messageContainer = document.getElementById("messages");
    var $chatWindow = container.find("#messages");
    var $participants = container.find("#users");

    var $input = $("#chat-input");
    var $sendButton = $("#sendButton");
    var screensaverControl = new ScreenSaverControl();
    var regexHelper = new RegexHelper();

    //constructor
    (function () {
        if ($chatWindow.length !== 0) {
            scrollToLatestMessage();

            $window.resize(() => {
                scrollToLatestMessage();
            });
        }
    })();

  function scrollToLatestMessage () {
        const rsize = $window.height() - 200;
        $chatWindow.height(rsize);
        $chatWindow.scrollTop(messageContainer.scrollHeight);
    }

    //public methods
    self.setOnline = function () {
        setTimeout(() => {
            var members = [];

            for (let value of currentChannel._membersEntity.members.entries()) {
                members.push(value[1]._identity);
            }

            avatarControl.setParticipantAvatars(members, result => {
                $participants.append($(`<div class="sidebar__participant"><img class="avatar avatar--other avatar--participant" src=${encodeURI(result.url)} alt="avatar"><div class="sidebar__participant--username">${result.username}</div></div>`));
            });

        }, 1000);
    };

    self.setupChannel = function (currentChannel, identity) {
        self.identity = identity;
        currentChannel.join()
            .then(() => {
                self.printToLoading("#joinedAs",`<span class="me">${identity.username}</span>`);
                self.chatHistory(identity.uniqueuserid);
                self.setOnline();
            });

        currentChannel.on("messageAdded", data => {
            var content = JSON.parse(data.body);
            if (identity.uniqueuserid === data.author) {
                self.printMessage(data.timestamp, content);
            } else {
                self.printReceivedMessage(data.timestamp, content);
                notificationControl.sendNotification(content);
            }
        });
    };

    self.printToLoading = function (control, infoMessage) {
        let content = $(control);
        content.append(infoMessage);
        content.show();
    };

    self.chatHistory = function (username) {
        setTimeout(() => {
            for (let i = 95; i < 100; i++) {
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
        const $message = $("<div class=\"message-container__message message-container__message--me\">");
        $message.html(filterLinks(content.message, $message));
        renderMessage(`${content.uniqueuserid}${content.username}`, $message, $time, $container, $user, false);
    };

    self.printReceivedMessage = function (timestamp, content) {
        const $container = $("<div class=\"message-container message-container--other\" >");
        const $user = $("<div class=\"message-container__username message-container__username--other\">").text(content.username);
        const $time = $("<div class=\"message-container__timestamp\">").text(` ${timestamp.toLocaleTimeString()}`);
        const $message = $("<div class=\"message-container__message message-container__message--other\" >");
        $message.html(filterLinks(content.message, $message));
        renderMessage(`${content.uniqueuserid}${content.username}`, $message, $time, $container, $user, true, content.avatar);
    };

    function filterLinks(message, $container) {
        if (regexHelper.isImage(message)) {
            console.log(message);
            return $(`<img class="width-100-percent height-100-percent" src=${message} />`);
        }

        if (regexHelper.isDailyMotion(message)) {
            console.log(message);
            new AjaxHelper().Get(`${siteurl}/api/embed?videoType=DailyMotion&url=${message}`, response => {
                console.log(response.html);
                $container.append(response.html);
                scrollToLatestMessage();
            });
        }

        return message;
    }

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

    const sendMessage = function () {
        if ($input.val().length === 0) return;
        const dataToSend = JSON.stringify({
            uniqueuserid: self.identity.uniqueuserid,
            username: self.identity.username,
            message: $input.val(),
            avatar: avatarControl.currentAvatar()
        });

        currentChannel.sendMessage(dataToSend);
        $input.val("");
    };

    $sendButton.on("click", () => {
        sendMessage();
        screensaverControl.runScreenSaver();
    });

    $input.on("keydown", e => {
        if (e.keyCode === 13) {
            e.stopPropagation();
            e.preventDefault();
            sendMessage();
            screensaverControl.runScreenSaver();
        }
    });

};