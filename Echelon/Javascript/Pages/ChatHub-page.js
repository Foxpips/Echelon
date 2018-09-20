/*jshint esversion: 6 */

var ChatHubController = function (userToken) {
    var chat = $.connection.chatHub;
    const $hub = $.connection.hub;

    const $message = $("#chat-input");
    const $sendMessage = $("#sendButton");

    var $window = $(window);
    var $loading = $("#loading");
    var $container = $("#container");
    var $chatWindow = $container.find("#messages");
    var $participants = $container.find("#users");
    var messageContainer = document.getElementById("messages");

    var previousSender = "";
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

    chat.client.ChangeName = function () {
        chat.server.notify(userToken.username, $hub.id);
    };

    chat.client.Online = function (name, avatarUrl) {
        $participants.append($(`<div class="sidebar__participant"><img class="avatar avatar--other avatar--participant" src=${avatarUrl} alt=""><div class="sidebar__participant--username">${name}</div></div>`));
    };

    chat.client.Enters = function (name) {
        $chatWindow.append(`<div class="border"><i>${name} enters chatroom</i></div>`);
        members.push(name);
    };

    chat.client.SendMessage = function (user, message) {
        console.log("here");
        console.log(user);
        console.log(message);
        if (userToken.uniqueId === user.UniqueId) {
            self.printMessage(user, message);
        } else {
            self.printReceivedMessage(user, message);
        }
    };

    chat.client.Disconnected = function (name) {
        $chatWindow.append(`<div class="border"><i>${name} leaves chatroom</i></div>`);
    };

    $.connection.hub.start().done(function () {
        chat.server.notify(userToken, $hub.id);

        $sendMessage.on("click", function () {
            chat.server.send(userToken, $message.val());
            $message.val("").focus();
        });
    });

    function scrollToLatestMessage() {
        const rsize = $window.height() - 200;
        $chatWindow.height(rsize);
        $chatWindow.scrollTop(messageContainer.scrollHeight);
    }

    self.printMessage = function (content, message) {
        console.log("here 2");
        console.log(content.UniqueId);
        console.log(content.UserName);

        const $containerHtml = $("<div class=\"message-container\">");
        const $userHtml = $("<div class=\"message-container__username message-container__username--me\">").text(content.UserName);
        const $currentmessageHtml = $("<div class=\"message-container__message message-container__message--me\">");
        $currentmessageHtml.html(message, $currentmessageHtml);
        renderMessage(`${content.UniqueId}${content.UserName}`, $currentmessageHtml, $containerHtml, $userHtml, false);
    };

    self.printReceivedMessage = function (content, message) {
        const $containerHtml = $("<div class=\"message-container message-container--other\" >");
        const $userHtml = $("<div class=\"message-container__username message-container__username--other\">").text(content.UserName);
        const $currentmessageHtml = $("<div class=\"message-container__message message-container__message--other\" >");
        $currentmessageHtml.html(message, $currentmessageHtml);
        renderMessage(`${content.UniqueId}${content.UserName}`, $currentmessageHtml, $containerHtml, $userHtml, true, content.AvatarUrl);
    };

    function renderMessage(currentSender, $currentmessage, $chatcontainer, $user, renderAvatar, avatarUrl) {
        console.log(currentSender);
        console.log(previousSender);

        if (currentSender === previousSender) {
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
        $chatcontainer.append($("<div class=\"message-container__timestamp\">").text(` ${new Date().toLocaleTimeString()}`));
        $chatWindow.append($chatcontainer);
        $chatWindow.scrollTop($chatWindow[0].scrollHeight);
        previousSender = currentSender;
    }
};