/*jshint esversion: 6 */

var RegexHelper = function () {
    const self = this;

    self.isImage = function(string) {
        return /\.(gif|jpg|jpeg|tiff|png)$/i.test(string);
    };

    self.isYoutube = function(string) {
        return /^(https?\:\/\/)?(www\.)?(youtube\.com|youtu\.?be)\/.+$/i.test(string);
    };

    self.isVimeo = function(string) {
        return /^(https?\:\/\/)?(www\.)?(vimeo\.com)\/.+$/i.test(string);
    };

    self.isDailyMotion = function (string) {
        return /^(https?\:\/\/)?(www\.)?(dailymotion\.com)\/.+$/i.test(string);
    };

    self.isTwitch = function(string) {
        return /^(https?\:\/\/)?(www\.)?(twitch\.com)\/.+$/i.test(string);
    };
};