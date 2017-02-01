/*jshint esversion: 6 */
var ChatControl = function (notificationManager) {
    var self = this;
    var $window = $(window);
    var $chatWindow = $("#messages"); //TODO smarkey use find on a base dom element to speed up search
    var $conversations = $("#conversations"); //TODO smarkey use find on a base dom element to speed up search
    var metypedLastMessage = false;

    (function () {
        var psize = $window.height() - 160;
        $chatWindow.height(psize);

        $window.resize(function () {
            var rsize = $window.height() - 160;
            $chatWindow.height(rsize);
            $chatWindow.scrollTop(document.getElementById("messages").scrollHeight);
        });
    })();

    // Helper function to print info messages to the chat window
    self.print = function (infoMessage, asHtml) {
        var $msg = $("<div class=\"info\">");
        if (asHtml) {
            $msg.html(infoMessage);
        } else {
            $msg.text(infoMessage);
        }
        $chatWindow.append($msg);
    };

    // Helper function to print chat message to the chat window
    self.printMessage = function (fromUser, message) {
        var $container = $("<div class=\"message-container\">");
        var $user = $("<div class=\"message-container__username message-container__username--me\">").text(fromUser + ": ");
        var $message = $("<div class=\"message-container__message message-container__message--me\">").text(message);
//        $magicDiv.append($message);

        if (metypedLastMessage === true) {
            $container.append($message);
        } else {
            $container.append($user).append($message);
        }
        
        $chatWindow.append($container);
        $chatWindow.scrollTop($chatWindow[0].scrollHeight);
        metypedLastMessage = true;
    };

    self.printReceivedMessage = function (fromUser, message) {
        var $container = $("<div class=\"message-container message-container--other\" >");
        var $user = $("<div class=\"message-container__username message-container__username--other\">").text(fromUser + ": ");
        var $message = $("<div class=\"message-container__message message-container__message--other\" >").text(message);
//        $magicDiv.append($message);

        if (metypedLastMessage === false) {
            $container.append($message);
        } else {
            $container.append($user).append($message);
        }

        $chatWindow.append($container);
        $chatWindow.scrollTop($chatWindow[0].scrollHeight);
        metypedLastMessage = false;
    };

    self.chatHistory = function (username) {
        setTimeout(function () {
            for (var i = 0; i < currentChannel.messages.length; i++) {
                var message = currentChannel.messages[i];
                if (username === message.author) {
                    self.printMessage(message.author, message.body);
                } else {
                    self.printReceivedMessage(message.author, message.body);
                }
            }
//            for (let value of currentChannel._membersEntity.members.entries()) {
//                $conversations.append($("<div>"+ value[1]._identity+"</div>"));
//            }

            $("#loading").hide();
        },
            1000);
    };

    // Set up channel after it has been found
    self.setupChannel = function (currentChannel, username) {
        // Join the general channel

        currentChannel.join()
            .then(function () {
                self.print("Joined channel as " + "<span class=\"me\">" + username + "</span>.", true);
                self.chatHistory(username);
            });

        // Listen for new messages sent to the channel
        currentChannel.on("messageAdded",
            function (message) {
                if (username === message.author) {
                    self.printMessage(message.author, message.body);
                } else {
                    self.printReceivedMessage(message.author, message.body);
                    notificationManager.sendNotification(message.author, message.body);
                }
            });
    };
};