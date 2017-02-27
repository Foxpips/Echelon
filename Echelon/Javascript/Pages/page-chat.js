/*jshint esversion: 6 */

var currentChannel;
$(function () {

    var accessManager;
    var messagingClient;
    var selectedChannel = "Anime"; //TODO set via data target of hidden field or something

    var $input = $("#chat-input");
    const endpoint = $("#chat-input").data("target");
    const $sendButton = $("#sendButton");

    var avatarControl = new AvatarControl();
    const notificationControl = new NotificationControl();
    var chatControl = new ChatControl(notificationControl);

    chatControl.printToLoading("Logging in...", false, true);
    $.getJSON(endpoint, { device: "browser", channel: selectedChannel }, data => {
        const identity = data.identity;
        const username = identity.username;
        const email = identity.email;

        accessManager = new window.Twilio.AccessManager(data.token);
        messagingClient = new window.Twilio.IPMessaging.Client(accessManager);

        chatControl.printToLoading("Joining channel: " + selectedChannel);
        avatarControl.setAvatarUrl(email);

        const promise = messagingClient.getChannelByUniqueName(selectedChannel);
        promise.then(channel => {
            currentChannel = channel;
            if (!currentChannel) {
                messagingClient.createChannel({
                    uniqueName: selectedChannel,
                    friendlyName: selectedChannel
                }).then(activechannel => {
                    currentChannel = activechannel;
                    chatControl.setupChannel(currentChannel, username);
                });
            } else {
                chatControl.setupChannel(currentChannel, username);
            }
        });
    });

    $sendButton.on("click", () => {
        const dataToSend = JSON.stringify({
            message: $input.val(),
            avatar: avatarControl.avatarUrl
        });
        currentChannel.sendMessage(dataToSend);
        $input.val("");
    });

    $input.on("keydown", e => {
        if (e.keyCode === 13) {
            e.stopPropagation();
            e.preventDefault();

            const dataToSend = JSON.stringify({
                message: $input.val(),
                avatar: avatarControl.avatarUrl
            });

            currentChannel.sendMessage(dataToSend);
            $input.val("");
        }
    });
});
