/*jshint esversion: 6 */

var ProfilePage = function () {

    (function() {
        $("#menuUsers").hide();
        $("#usersSideNav").hide();
    })();

    $("#fileUpload").on("change", function () {
        let uploadButton = $(".file-upload");
        let fileToUpload = uploadButton.prop("files")[0];

        $("#uploading").show();
        $("#fileNameUploaded").text(fileToUpload.name);
        $("#avatarform").submit();
    });
};