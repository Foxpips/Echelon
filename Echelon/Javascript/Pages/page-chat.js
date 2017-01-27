$(function () {

    var $chatWindow = $("#messages");
    var $users = $("#conversations");
    var notificationManager = new notifier();
    notificationManager.init();

    // Manages the state of our access token we got from the server
    var accessManager;

    // Our interface to the IP Messaging service
    var messagingClient;

    // A handle to the "general" chat channel - the one and only channel we
    // will have in this sample app
    var currentChannel;

    // The server will assign the client a random username - store that value
    // here
    var username;

    var psize = $(window).height() - 160;
    $("#messages").height(psize);

    $(window).resize(function () {
        var psize = $(window).height() - 160;
        $("#messages").height(psize);
        $("#messages").scrollTop(document.getElementById("messages").scrollHeight);
    });

    // Helper function to print info messages to the chat window
    function print(infoMessage, asHtml) {
        var $msg = $("<div class=\"info\">");
        if (asHtml) {
            $msg.html(infoMessage);
        } else {
            $msg.text(infoMessage);
        }
        $chatWindow.append($msg);
    }

    // Helper function to print chat message to the chat window
    function printMessage(fromUser, message) {
        var $container = $("<div class=\"message-container\">");
        var $user = $('<span class="message-container__username message-container__username--me">').text(fromUser + ": ");
        var $message = $("<span class=\"message-container__message message-container__message--me\">").text(message);
        $container.append($user).append($message);
        $chatWindow.append($container);
        $chatWindow.scrollTop($chatWindow[0].scrollHeight);
    }

    function printReceivedMessage(fromUser, message) {
        var $container = $("<div class=\"message-container message-container--other\" >");
        var $user = $('<span class="message-container__username message-container__username--other">').text(fromUser + ": ");
        var $message = $("<span class=\"message-container__message message-container__message--other\" >").text(message);
        $container.append($user).append($message);
        $chatWindow.append($container);
        $chatWindow.scrollTop($chatWindow[0].scrollHeight);
    }

    // Alert the user they have been assigned a random username
    print('Logging in...');

    // Get an access token for the current user, passing a username (identity)
    // and a device ID - for browser-based apps, we'll always just use the 
    // value "browser"

    var selectedChannel = "Anime";
    var endpoint = $("#chat-input").data("target");

    $.getJSON(endpoint, { device: "browser", channel: selectedChannel }, function (data) {
        username = data.identity;
        // Initialize the IP messaging client

        accessManager = new Twilio.AccessManager(data.token);
        messagingClient = new Twilio.IPMessaging.Client(accessManager);

        // Get the general chat channel, which is where all the messages are
        // sent in this simple application
        print("Attempting to join " + selectedChannel + " channel...");
        var promise = messagingClient.getChannelByUniqueName(selectedChannel);
        promise.then(function (channel) {
            currentChannel = channel;
            if (!currentChannel) {
                // If it doesn't exist, let's create it
                messagingClient.createChannel({
                    uniqueName: selectedChannel,
                    friendlyName: selectedChannel + "Chat Channel"
                }).then(function (channel) {
                    console.log("Created " + selectedChannel + " channel:");
                    console.log(channel);
                    currentChannel = channel;
                    setupChannel();
                });
            } else {
                console.log("Found " + selectedChannel + " channel:");
                console.log(currentChannel);
                setupChannel();
            }
        });
    });

    // Set up channel after it has been found
    function setupChannel() {
        // Join the general channel
        currentChannel.join().then(function () {
            print("Joined channel as " + '<span class="me">' + username + '</span>.', true);
            $("#loading").hide();
        });

        // Listen for new messages sent to the channel
        currentChannel.on("messageAdded", function (message) {
            if (username === message.author) {
                printMessage(message.author, message.body);
            } else {
                printReceivedMessage(message.author, message.body);
                notificationManager.sendNotification(message.author, message.body);
            }
        });

        currentChannel.on("memberJoined", function (member) {
            alert("asd");
            var $msg = $("<div class=\"info\">");
            $msg.text(member.identity);
            $users.append($msg);
            //conversations
        });

        currentChannel.on('typingStarted', function (member) {
            console.log(member.identity + 'is currently typing.');
        });
    }

    // Send a new message to the general channel
    var $input = $("#chat-input");
    $input.on("keydown", function (e) {
        if (e.keyCode === 13) {
            currentChannel.sendMessage($input.val());
            $input.val("");
        }
    });
});
