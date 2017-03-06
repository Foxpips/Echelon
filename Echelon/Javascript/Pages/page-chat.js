/*jshint esversion: 6 */

var currentChannel;
$(function () {

    var identity;
    var accessManager;
    var messagingClient;
    var selectedChannel = "Anime"; //TODO set via data target of hidden field or something

    var $input = $("#chat-input");
    const endpoint = $("#chat-input").data("target");
    const $sendButton = $("#sendButton");

    const ajaxHelper = new AjaxHelper();
    const notificationControl = new NotificationControl();

    var chatControl = new ChatControl(notificationControl);
    chatControl.printToLoading("Logging in...", false, true);

    var avatarControl = new AvatarControl(ajaxHelper);
    avatarControl.setAvatarUrl($("#headerAvatar").data("target"));


    $.getJSON(endpoint, { device: "browser", channel: selectedChannel }, data => {
        identity = data.identity;

        accessManager = new window.Twilio.AccessManager(data.token);
        messagingClient = new window.Twilio.IPMessaging.Client(accessManager);

        chatControl.printToLoading(`Joining channel: ${selectedChannel}`);

        const promise = messagingClient.getChannelByUniqueName(selectedChannel);
        promise.then(channel => {
            currentChannel = channel;
            if (!currentChannel) {
                messagingClient.createChannel({
                    uniqueName: selectedChannel,
                    friendlyName: selectedChannel
                }).then(activechannel => {
                    currentChannel = activechannel;
                    chatControl.setupChannel(currentChannel, identity);
                });
            } else {
                chatControl.setupChannel(currentChannel, identity);
            }
        });
    });

    const sendMessage = function () {
        if ($input.val().length === 0) return;

        const dataToSend = JSON.stringify({
            username: identity.username,
            message: $input.val(),
            avatar: avatarControl.avatarUrl
        });

        currentChannel.sendMessage(dataToSend);
        $input.val("");
    };

    $sendButton.on("click", () => {
        sendMessage();
    });

    $input.on("keydown", e => {
        if (e.keyCode === 13) {
            e.stopPropagation();
            e.preventDefault();
            sendMessage();
        }
    });
});
