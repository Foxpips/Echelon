/*jshint esversion: 6 */

var currentChannel;

var ChatPage = function () {
    var identity;
    var accessManager;
    var messagingClient;
    var selectedChannel = "Anime";

    var $input = $("#chat-input");
    var endpoint = $("#chat-input").data("target");
    var $sendButton = $("#sendButton");


    var ajaxHelper = new AjaxHelper();
    var notificationControl = new NotificationControl();
    var screensaverControl = new ScreenSaverControl();

    var avatarControl = new AvatarControl(ajaxHelper);
    avatarControl.setUserAvatar($("#headerAvatar").data("target"));

    var chatControl = new ChatControl(notificationControl, avatarControl);
    chatControl.printToLoading("#loggingIn");

    $.getJSON(endpoint, { device: "browser", channel: selectedChannel }, data => {
        identity = data.identity;

        accessManager = new window.Twilio.AccessManager(data.token);
        messagingClient = new window.Twilio.IPMessaging.Client(accessManager);

        chatControl.printToLoading("#joiningChannel", `${selectedChannel}`);

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
        screensaverControl.runScreenSaver();
    });

    $input.on("keydown", e => {
        if (e.keyCode === 13) {
            e.stopPropagation();
            e.preventDefault();
            sendMessage();
            screensaverControl.runScreenSaver();
        }
    });
};
