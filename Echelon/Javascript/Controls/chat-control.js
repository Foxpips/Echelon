var ChatControl = function (notificationManager) {
    var self = this;
    var $window = $(window);
    var $chatWindow = $("#messages"); //TODO smarkey use find on a base dom element to speed up search

    var ctor = function() {
        var psize = $window.height() - 160;
        $chatWindow.height(psize);

        $window.resize(function() {
            var rsize = $window.height() - 160;
            $chatWindow.height(rsize);
            $chatWindow.scrollTop(document.getElementById("messages").scrollHeight);
        });
    }();

    // Helper function to print info messages to the chat window
    self.print = function(infoMessage, asHtml) {
        var $msg = $("<div class=\"info\">");
        if (asHtml) {
            $msg.html(infoMessage);
        } else {
            $msg.text(infoMessage);
        }
        $chatWindow.append($msg);
    };

    // Helper function to print chat message to the chat window
    self.printMessage = function(fromUser, message) {
        var $container = $("<div class=\"message-container\">");
        var $user = $("<span class=\"message-container__username message-container__username--me\">")
            .text(fromUser + ": ");
        var $message = $("<span class=\"message-container__message message-container__message--me\">").text(message);
        $container.append($user).append($message);
        $chatWindow.append($container);
        $chatWindow.scrollTop($chatWindow[0].scrollHeight);
    };

    self.printReceivedMessage = function(fromUser, message) {
        var $container = $("<div class=\"message-container message-container--other\" >");
        var $user = $("<span class=\"message-container__username message-container__username--other\">")
            .text(fromUser + ": ");
        var $message =
            $("<span class=\"message-container__message message-container__message--other\" >").text(message);
        $container.append($user).append($message);
        $chatWindow.append($container);
        $chatWindow.scrollTop($chatWindow[0].scrollHeight);
    };

    // Set up channel after it has been found
    self.setupChannel = function(currentChannel, username) {
        // Join the general channel
        currentChannel.join()
            .then(function() {
                self.print("Joined channel as " + "<span class=\"me\">" + username + "</span>.", true);
                $("#loading").hide();
            });

        // Listen for new messages sent to the channel
        currentChannel.on("messageAdded",
            function(message) {
                if (username === message.author) {
                    self.printMessage(message.author, message.body);
                } else {
                    self.printReceivedMessage(message.author, message.body);
                    notificationManager.sendNotification(message.author, message.body);
                }
            });
    };
};