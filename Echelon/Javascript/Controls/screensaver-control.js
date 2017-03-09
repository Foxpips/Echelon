var ScreenSaverControl = function() {
    var self = this;
    self.bubbles = $("#bubbles");
    self.fadeBubbleIn = undefined;

    (function() {
        self.bubbles.fadeOut(1);
    })();

    self.fadeBubbleOut = function() {
        self.bubbles.fadeOut(1000);
    };

    self.runScreenSaver = function() {
        self.fadeBubbleOut();
        clearTimeout(self.fadeBubbleIn);

        self.fadeBubbleIn = setTimeout(function() {
                self.bubbles.fadeIn(2000);
            }, 25000);
    };
};