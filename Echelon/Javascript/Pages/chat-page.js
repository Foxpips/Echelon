/*jshint esversion: 6 */

var currentChannel;

var ChatPage = function () {
    const selectedChannel = "Anime";
    const browser = "browser";
 
    var endpoint = $("#chat-input").data("target");
    const ajaxHelper = new AjaxHelper();
   
    (function() {
        ajaxHelper.Post(endpoint, { device: browser, channel: selectedChannel }, data => {
            ChatHubController(data.identity);
        });
    })();
};