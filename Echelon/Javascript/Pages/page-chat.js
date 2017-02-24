/*jshint esversion: 6 */

var currentChannel; // A handle to the "general" chat channel - the one and only channel we will have in this sample app
$(function () {

    var $input = $("#chat-input");
    var $sendButton = $("#sendButton");
    var accessManager; // Manages the state of our access token we got from the server
    var messagingClient; // Our interface to the IP Messaging service
    var username;

    var notificationControl = new NotificationControl();
    var avatarControl = new AvatarControl();
    var chatControl = new ChatControl(notificationControl);
    chatControl.printToLoading("Logging in...", false, true);

    var selectedChannel = "Anime";
    var endpoint = $("#chat-input").data("target");

    $.getJSON(endpoint, { device: "browser", channel: selectedChannel }, function (data) {
        username = data.identity;

        avatarControl.setAvatarUrl();

        // Initialize the IP messaging client
        accessManager = new Twilio.AccessManager(data.token);
        messagingClient = new Twilio.IPMessaging.Client(accessManager);

        // Get the general chat channel, which is where all the messages are
        // sent in this simple application
        chatControl.printToLoading("Joining channel: " + selectedChannel);
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
                    chatControl.setupChannel(currentChannel, username);
                });
            } else {
                chatControl.setupChannel(currentChannel, username);
            }
        });
    });

    $sendButton.on("click", function () {
        currentChannel.sendMessage($input.val());
        $input.val("");
    });

    $input.on("keydown", function (e) {
        if (e.keyCode === 13) {
            e.stopPropagation();
            e.preventDefault();
            currentChannel.sendMessage(JSON.stringify({ message: $input.val(), avatar: avatarControl.avatarUrl }));
            $input.val("");
        }
    });
});
