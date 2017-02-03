/*jshint esversion: 6 */
var ChatControl = function (notificationManager) {
    var self = this;
    var $window = $(window);
    const container = $("#container");
    var $chatWindow = container.find("#messages");
    var $conversations = container.find("#users");
    var lastOtherAuthor = "";

    (function () {
        const psize = $window.height() - 160;
        $chatWindow.height(psize);

        $window.resize(function () {
            const rsize = $window.height() - 160;
            $chatWindow.height(rsize);
            $chatWindow.scrollTop(document.getElementById("messages").scrollHeight);
        });
    })();

    // Helper function to print info messages to the chat window
    self.print = function (infoMessage, asHtml) {
        const $msg = $("<div class=\"info\">");
        if (asHtml) {
            $msg.html(infoMessage);
        } else {
            $msg.text(infoMessage);
        }
        $chatWindow.append($msg);
    };

    const renderMessage = function (fromUser, $message, $time, $container, $user) {
        $message.append($time);

        if (fromUser === lastOtherAuthor) {
            $container.append($message);
        } else {
            $container.append($user).append($message);
        }

        $chatWindow.append($container);
        $chatWindow.scrollTop($chatWindow[0].scrollHeight);
        lastOtherAuthor = fromUser;
    };

    self.printMessage = function (fromUser, message) {
        const $container = $("<div class=\"message-container\">");
        const $user = $("<div class=\"message-container__username message-container__username--me\">").text(fromUser + ": ");
        const $time = $("<div class=\"message-container__timestamp\">").text(` ${message.timestamp.toLocaleTimeString()}`);
        const $message = $("<div class=\"message-container__message message-container__message--me\">").text(message.body);
        renderMessage(fromUser, $message, $time, $container, $user);
    };

    self.printReceivedMessage = function (fromUser, message) {
        const $container = $("<div class=\"message-container message-container--other\" >");
        const $user = $("<div class=\"message-container__username message-container__username--other\">").text(fromUser + ": ");
        const $time = $("<div class=\"message-container__timestamp\">").text(` ${message.timestamp.toLocaleTimeString()}`);
        const $message = $("<div class=\"message-container__message message-container__message--other\" >").text(message.body);
        renderMessage(fromUser, $message, $time, $container, $user);
    };

    self.chatHistory = function (username) {
        setTimeout(function () {
            for (let i = 0; i < currentChannel.messages.length; i++) {
                const message = currentChannel.messages[i];
                if (username === message.author) {
                    self.printMessage(message.author, message);
                } else {
                    self.printReceivedMessage(message.author, message);
                }
            }

            $("#loading").hide();
        }, 1000);
    };

    self.setOnline = function () {
        setTimeout(function () {
            for (let value of currentChannel._membersEntity.members.entries()) {
                $conversations.append($(`<div>${value[1]._identity}</div>`));
            }
        }, 1000);
    };

    self.setupChannel = function (currentChannel, username) {
        currentChannel.join()
            .then(function () {
                self.print("Joined channel as " + "<span class=\"me\">" + username + "</span>.", true);
                self.chatHistory(username);
                self.setOnline();
            });

        currentChannel.on("messageAdded",
            function (message) {
                if (username === message.author) {
                    self.printMessage(message.author, message);
                } else {
                    self.printReceivedMessage(message.author, message);
                    notificationManager.sendNotification(message.author, message);
                }
            });
    };
};