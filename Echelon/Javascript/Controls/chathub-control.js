/*jshint esversion: 6 */

var ChatHubController = function (userToken, regexHelper, screensaverControl, notificationControl, popupControl) {
    var chat = $.connection.chatHub;
    const $hub = $.connection.hub;

    var members = [];
    var previousSender = "";

    const $message = $("#chat-input");
    const $sendMessageButton = $("#sendButton");

    const $window = $(window);
    const $joiningChannel = $("#joiningChannel");
    const $joinedAs = $("#joinedAs");
    const $loading = $("#loading");
    const $container = $("#container");
    const $chatWindow = $container.find("#messages");
    const $participants = $("#usersSideNav");
    const $usersContainer = $container.find(".users__container");
    const $selectedUser = $container.find("#participants");

    const messageContainer = document.getElementById("messages");

    //constructor
    (function () {
        if ($chatWindow.length !== 0) {
            scrollToLatestMessage();

            $window.resize(() => {
                scrollToLatestMessage();
            });
        }
    
        initialiseUi();
    })();

    chat.client.ChangeName = function () {
        chat.server.notify(userToken.UserName, $hub.id);
    };

    chat.client.Online = function (user) {
        $participants.append(`<a><div class="userMenu__participant" data-onlineUser=${user.UniqueId
        }><img class ="avatar avatar--menu avatar--participant" src=${user.AvatarUrl
        } alt=""><div class ="userMenu__participant--username">${user.UserName}</div></div></a>`);
    };

    chat.client.Enters = function (user) {
        popupControl.Information(`${user.UserName} has joined`);
        $participants
            .append(`<div class="sidebar__participant" data-onlineUser=${user.UniqueId
        }><img class="avatar avatar--other avatar--participant" src=${user.AvatarUrl
        } alt=""><div class="sidebar__participant--username">${user.UserName}</div></div>`);
        members.push(user.UserName);
    };

    chat.client.ReceiveMessage = function (user, message) {
        if (userToken.UniqueId === user.UniqueId) {
            printMyMessage(user, message);
        } else {
            printTheirMessage(user, message);
            notificationControl.sendNotification(user, message);
        }
    };

    chat.client.Disconnected = function (user) {
        popupControl.Information(`${user.UserName} has left`);
        $(`.sidebar__participant[data-onlineUser="${user.UniqueId}"]`).remove();

    };

    $.connection.hub.start()
        .done(function () {
            chat.server.notify(userToken, $hub.id);

            $sendMessageButton.on("click",
                function () {
                    sendMessage();
                });

            $message.on("keydown",
                e => {
                    if (e.keyCode === 13) {
                        e.stopPropagation();
                        e.preventDefault();
                        sendMessage();
                    }
                });
        });

    function sendMessage() {
        chat.server.send(userToken, $message.val());
        $message.val("").focus();

        screensaverControl.runScreenSaver();
    }

    function scrollToLatestMessage() {
        const rsize = $window.height() - 200;
        $usersContainer.height(rsize);
        $chatWindow.height(rsize);
        $chatWindow.scrollTop(messageContainer.scrollHeight);
    }

    function printMyMessage(content, message) {
        const $containerHtml = $("<div class=\"message-container\">");
        const $userHtml = $("<div class=\"message-container__username message-container__username--me\">").text(content.UserName);
        const $currentmessageHtml = $("<div class=\"message-container__message message-container__message--me\">");
        $currentmessageHtml.html(filterLinks(message, $currentmessageHtml));
        renderMessage(`${content.UniqueId}${content.UserName}`, $currentmessageHtml, $containerHtml, $userHtml, false);
    }

    function printTheirMessage(content, message) {
        const $containerHtml = $("<div class=\"message-container message-container--other\" >");
        const $userHtml = $("<div class=\"message-container__username message-container__username--other\">").text(content.UserName);
        const $currentmessageHtml = $("<div class=\"message-container__message message-container__message--other\">");
        $currentmessageHtml.html(filterLinks(message, $currentmessageHtml));
        renderMessage(`${content.UniqueId}${content.UserName}`, $currentmessageHtml, $containerHtml, $userHtml, true, content.AvatarUrl);
    }

    function renderMessage(currentSender, $currentmessage, $chatcontainer, $user, renderAvatar, avatarUrl) {
        if (currentSender === previousSender) {
            $chatcontainer.append($currentmessage);
            $($chatcontainer.find(".message-container__timestamp").last()).hide();
        } else {
            if (renderAvatar) {
                $chatcontainer.append((`<img class="avatar avatar--other" src="${avatarUrl}">`));
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

    function filterLinks(message, $container) {
        if (regexHelper.isImage(message)) {
            return $(`<img class="width-100-percent height-100-percent" src=${message} />`);
        }

        if (regexHelper.isDailyMotion(message)) {
            new AjaxHelper().Get(`${siteurl}/api/embed?videoType=DailyMotion&url=${message}`,
                response => {
                    $container.append(response.html);
                    scrollToLatestMessage();
                });
        }

        return message;
    }

    function initialiseUi() {
        $joiningChannel.append("General");
        $joinedAs.append(userToken.UserName);

        $(document.body).on("click", ".goog-te-combo", function () {
            $(".skiptranslate").not(".goog-te-gadget").hide();
        });

        setTimeout(() => {
            $("body").removeAttr("style");
            $(".skiptranslate").not(".goog-te-gadget").hide();
            $loading.hide();
        }, 1000);
    }
};

//chat.server.sendToSpecific(userToken, $message.val(), $selectedUser.val());