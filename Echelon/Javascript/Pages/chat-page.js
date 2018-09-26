/*jshint esversion: 6 */

var currentChannel;

var ChatPage = function () {
    const selectedChannel = "Anime";
    const browser = "browser";
 
    var endpoint = $("#chat-input").data("target");
    const ajaxHelper = new AjaxHelper();
    const regexHelper = new RegexHelper();
    const screenSaverControl = new ScreenSaverControl();
    const popupControl = new PopupControl();
    const notificationControl = new NotificationControl(popupControl);
   
    (function() {
        ajaxHelper.Post(endpoint, { device: browser, channel: selectedChannel }, data => {
            ChatHubController(data.identity, regexHelper, screenSaverControl, notificationControl, popupControl);
        });
    })();
};