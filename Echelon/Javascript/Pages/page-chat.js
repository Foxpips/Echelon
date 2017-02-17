/*jshint esversion: 6 */

var currentChannel; // A handle to the "general" chat channel - the one and only channel we will have in this sample app
$(function () {

    var $input = $("#chat-input");
    var accessManager; // Manages the state of our access token we got from the server
    var messagingClient; // Our interface to the IP Messaging service
    var username;

    var notificationManager = new NotificationControl();
    var chatManager = new ChatControl(notificationManager);
    chatManager.printToLoading("Logging in...", false, true);

    var selectedChannel = "Anime";
    var endpoint = $("#chat-input").data("target");

    $.getJSON(endpoint, { device: "browser", channel: selectedChannel }, function (data) {
        username = data.identity;

        // Initialize the IP messaging client
        accessManager = new Twilio.AccessManager(data.token);
        messagingClient = new Twilio.IPMessaging.Client(accessManager);

        // Get the general chat channel, which is where all the messages are
        // sent in this simple application
        chatManager.printToLoading("Joining channel: " + selectedChannel);
        var promise = messagingClient.getChannelByUniqueName(selectedChannel);
        promise.then(function (channel) {
            currentChannel = channel;
            if (!currentChannel) {
                // If it doesn't exist, let's create it
                messagingClient.createChannel({
                    uniqueName: selectedChannel,
                    friendlyName: selectedChannel
                }).then(function (channel) {
                    currentChannel = channel;
                    chatManager.setupChannel(currentChannel, username);
                });
            } else {
                chatManager.setupChannel(currentChannel, username);
            }
        });
    });

    $("#sendButton").on("click", function () {
        currentChannel.sendMessage($input.val());
        $input.val("");
    });

    $input.on("keydown", function (e) {
        if (e.keyCode === 13) {
            currentChannel.sendMessage($input.val());
            $input.val("");
        }
    });
});
