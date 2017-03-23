/*jshint esversion: 6 */


var ProfilePage = function () {
    const ajaxHelper = new AjaxHelper();
    const avatarControl = new AvatarControl(ajaxHelper);
    avatarControl.setUserAvatar($("#headerAvatar").data("target"));
};
