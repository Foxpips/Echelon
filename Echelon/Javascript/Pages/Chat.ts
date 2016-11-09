/// <reference path="../scripts/typings/jquery/jquery.d.ts" />
class Chat {

    private $chatBox: JQuery;
    private selectedChannel: string;
    private manager: string;
    private client: string;
    private channel: any;
    private username: string;


    

    constructor(chatWindowId: string, selectedChannel: string) {
        this.selectedChannel = selectedChannel;
        this.$chatBox = $(`#${chatWindowId}`);
    }

    printConnectionMessage(infoMessage: string, isHtml: boolean) {
        const $msg = $("<div class=\"info\">");
        if (isHtml) {
            $msg.html(infoMessage);
        } else {
            $msg.text(infoMessage);
        }

        this.$chatBox.append($msg);
    }

    printChatMessage(fromUser: string, message: string) {
        const $message = $("<span class=\"message\">").text(message);
        const $container = $("<div class=\"message-container\">");
        const $user = $("<span class=\"username\">").text(fromUser + ":");

        if (fromUser === this.username) {
            $user.addClass("me");
        }

        $container.append($user).append($message);
        this.$chatBox.append($container);
        this.$chatBox.scrollTop(this.$chatBox[0].scrollHeight);
    }

    connect() {
        $.getJSON("/token", {
            identity: this.username,
            device: "browser"
        }, data => {
            // Alert the user they have been assigned a random username
            this.username = data.identity;

            // Initialize the IP messaging client

            var accessManager = new Twilio.AccessManager(data.token);
            var messagingClient = new Twilio.IPMessaging.Client(accessManager);

            // Get the general chat channel, which is where all the messages are
            // sent in this simple application
            this.printConnectionMessage('Attempting to join "general" category...', false);

            var promise = messagingClient.getChannelByUniqueName(this.selectedChannel);
            promise.then(channel => {
                this.channel = channel;
                if (!this.channel) {
                    // If it doesn't exist, let's create it
                    messagingClient.createChannel({ uniqueName: this.selectedChannel, friendlyName: `${this.selectedChannel} chat channel` })
                        .then(channel => {
                            console.log(`Created ${this.selectedChannel} channel:`);
                            console.log(channel);
                            this.channel = channel;
                            this.setupChannel();
                        });
                } else {
                    console.log("Found channel:");
                    console.log(this.channel);
                    this.setupChannel();
                }
            });
        });
    }

    setupChannel() {
        // Join the general channel
        this.channel.join().then(() => {
            this.printConnectionMessage(`Joined channel as <span class="me">${this.username}</span>.`, true);
        });

        // Listen for new messages sent to the channel
        this.channel.on("messageAdded", message => {
            this.printChatMessage(message.author, message.body);
        });
    }

    bindKeyPress() {
        var $input = $("#chat-input");
        $input.on("keydown", e => {
            if (e.keyCode === 13) {
                this.channel.sendMessage($input.val());
                $input.val("");
            }
        });
    }
}