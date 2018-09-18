/*jshint esversion: 6 */

var ChatHubController = function (identity) {
    var chat = $.connection.chatHub;
    const $hub = $.connection.hub;

    const $message = $("#chat-input");
    const $sendMessage = $("#sendButton");

    var $window = $(window);
    var $container = $("#container");
    var messageContainer = document.getElementById("messages");

    var lastOtherAuthor = "";
    var $chatWindow = $container.find("#messages");
    var $participants = $container.find("#users");
    var $loading = $("#loading");

    var members = [];

    //constructor
    (function () {
        if ($chatWindow.length !== 0) {
            scrollToLatestMessage();

            $window.resize(() => {
                scrollToLatestMessage();
            });
        }

        $loading.hide();
    })();

    function scrollToLatestMessage() {
        const rsize = $window.height() - 200;
        $chatWindow.height(rsize);
        $chatWindow.scrollTop(messageContainer.scrollHeight);
    }

    self.printMessage = function (timestamp, content) {
        const $container = $("<div class=\"message-container\">");
        const $user = $("<div class=\"message-container__username message-container__username--me\">").text(content.username);
        const $time = $("<div class=\"message-container__timestamp\">").text(` ${timestamp.toLocaleTimeString()}`);
        const $currentmessage = $("<div class=\"message-container__message message-container__message--me\">");
        $currentmessage.html(content.message, $currentmessage);
        renderMessage(`${content.uniqueuserid}${content.username}`, $currentmessage, $time, $container, $user, false);
    };

    self.printReceivedMessage = function (timestamp, content) {
        const $container = $("<div class=\"message-container message-container--other\" >");
        const $user = $("<div class=\"message-container__username message-container__username--other\">").text(content.username);
        const $time = $("<div class=\"message-container__timestamp\">").text(` ${timestamp.toLocaleTimeString()}`);
        const $currentmessage = $("<div class=\"message-container__message message-container__message--other\" >");
        $currentmessage.html(content.message, $currentmessage);
        renderMessage(`${content.uniqueuserid}${content.username}`, $currentmessage, $time, $container, $user, true, content.avatar);
    };

    function renderMessage(fromUser, $currentmessage, $time, $chatcontainer, $user, renderAvatar, avatarUrl) {
        if (fromUser === lastOtherAuthor) {
            $chatcontainer.append($currentmessage);
            $($chatcontainer.find(".message-container__timestamp").last()).hide();
        }
        else {
            if (renderAvatar) {
                $chatcontainer.append($(`<img class="avatar avatar--other" src="${avatarUrl}" alt="avatar">`));
            }

            if ($currentmessage.hasClass("message-container__message--me")) {
                $currentmessage.addClass("message-container__message--initial-me");
            } else {
                $currentmessage.addClass("message-container__message--initial-other");
            }

            $chatcontainer.append($user).append($currentmessage);
        }

        $chatcontainer.append($time);
        $chatWindow.append($chatcontainer);
        $chatWindow.scrollTop($chatWindow[0].scrollHeight);
        lastOtherAuthor = fromUser;
    }

    chat.client.ChangeName = function () {
        chat.server.notify(identity.username, $hub.id);
    };

    chat.client.Online = function (name) {
        $participants.append($(`<div class="sidebar__participant"><img class="avatar avatar--other avatar--participant" alt="avatar"><div class="sidebar__participant--username">${name}</div></div>`));
    };

    chat.client.Enters = function (name) {
        $chatWindow.append(`<div class="border"><i>${name} enters chatroom</i></div>`);
        members.push(name);
    };

    chat.client.SendMessage = function (name, message, uniqueId) {
        const timestamp = new Date();

        let content = {
            name: name,
            message: message,
            uniqueId: uniqueId,
            avatar: ""
        };

        console.log(content);

        if (identity.uniqueuserid === uniqueId) {
            self.printMessage(timestamp, content);
        } else {
            self.printReceivedMessage(timestamp, content);
        }
    };

    chat.client.Disconnected = function (name) {
        $chatWindow.append(`<div class="border"><i>${name} leaves chatroom</i></div>`);
    };

    $.connection.hub.start()
        .done(function () {
            chat.server.notify(identity.username, $hub.id);
            $sendMessage.click(function () {
                console.log("asd");
                chat.server.send(identity.username, $message.val(), identity.uniqueuserid);
                $message.val("").focus();
            });
        });
};