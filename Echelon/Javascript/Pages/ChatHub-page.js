/*jshint esversion: 6 */

var ChatHubPage = function() {
    var chat = $.connection.chatHub;
    const $hub = $.connection.hub;
    const $displayName = $("#displayname");
    const $onlineList = $("#onlineList");
    const $users = $("#users");
    const $currentChat = $("#chats");
    const $message = $("#message");
    const $sendMessage = $("#sendmessage");

    //constructor
    (function() {
        $displayName.val(prompt("Enter your name:", ""));
    })();

    chat.client.ChaneName = function() {
        $displayName.val(prompt("Please enter different username:", ""));
        chat.server.notify($displayName.val(), $hub.id);
    };

    chat.client.Online = function(name) {
        if (name === $displayName.val())
            $onlineList.append(`<div class="border" style="color:red">You: ${name}</div>`);
        else {
            $onlineList.append(`<div class="border">${name}</div>`);
            $users.append(`<option value="${name}">${name}</option>`);
        }
    };

    chat.client.Enters = function(name) {
        $currentChat.append(`<div class="border"><i>${name} enters chatroom</i></div>`);
        $users.append(`<option value="${name}">${name}</option>`);
        $onlineList.append(`<div class="border">${name}</div>`);
    };

    chat.client.SendMessage = function(name, message) {
        $currentChat.append(`<div class="border"><span style="color:blue">${name}</span>: ${message}</div>`);
    };

    chat.client.Disconnected = function(name) {
        //Calls when someone leaves the page
        $currentChat.append(`<div class="border"><i>${name} leaves chatroom</i></div>`);
        $("#onlineList div").remove(`:contains('${name}')`);
        $("#users option").remove(`:contains('${name}')`);
    };

    // Start the connection.
    $.connection.hub.start()
        .done(function() {
            chat.server.notify($displayName.val(), $hub.id);
            $sendMessage.click(function() {
                if ($users.val() === "All") {
                    chat.server.send($displayName.val(), $message.val());
                } else {
                    chat.server.sendToSpecific($displayName.val(), $message.val(), $users.val());
                }
                $message.val("").focus();
            });
        });
};