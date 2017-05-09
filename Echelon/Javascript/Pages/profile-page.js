/*jshint esversion: 6 */

var ProfilePage = function () {
    const ajaxHelper = new AjaxHelper();
    const avatarControl = new AvatarControl(ajaxHelper);
    avatarControl.setUserAvatar($("#headerAvatar").data("target"));

    $("#fileUpload").on("change", function () {
        let uploadButton = $(".file-upload");
        let fileToUpload = uploadButton.prop("files")[0];

        $("#uploading").show();
        $("#fileNameUploaded").text(fileToUpload.name);
        $("#avatarform").submit();
    });
};