/*jshint esversion: 6 */

var currentChannel;

var ChatPage = function () {
    var identity;
    var accessManager;
    var messagingClient;
    var selectedChannel = "Anime";
 
    var endpoint = $("#chat-input").data("target");
    var ajaxHelper = new AjaxHelper();
    var notificationControl = new NotificationControl();
   
    var avatarControl = new AvatarControl(ajaxHelper);
    var chatControl = new ChatControl(notificationControl, avatarControl);

    (function() {
        chatControl.printToLoading("#loggingIn");
    })();

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
};
