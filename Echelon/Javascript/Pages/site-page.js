/*jshint esversion: 6 */

var SitePage = function () {
    var $menubar = $("#menuHome");
    var $rightNav = $("#rightSideNav");
    var $leftNav = $("#leftSideNav");
    var $closeNav = $(".closebtn");

    var $userAvatar = $("#headerAvatar");
    var $menuOverlayRight = $("#menuOverlayRight");
    var $menuOverlayLeft = $("#menuOverlayLeft");
    var theme = document.body.querySelectorAll("[data-attribute='theme']");

    var ajaxHelper = new AjaxHelper();
    var storageControl = new StorageControl();
    var avatarControl = new AvatarControl(ajaxHelper);

    //constructor
    (function () {
        let selectedTheme = storageControl.get("theme");

        if (selectedTheme === undefined) {
            selectedTheme = "theme-Light";
            storageControl.add("theme", selectedTheme);
        }

        const currentThemeHref = $('link[rel=stylesheet]')[0].href;
        const newThemeHref = currentThemeHref.replace(/theme-\w*/, selectedTheme);
        $('link[rel=stylesheet]')[0].href = newThemeHref;

        $(window).load(function () {
            $("#htmlContainer").removeAttr("style");
        });

        if ($userAvatar.length > 0) {
            avatarControl.setUserAvatar($userAvatar.data("target"));
        }

        $userAvatar.on("click", () => {
            $menuOverlayRight.hide();
            $leftNav.toggle("slide");
            $menuOverlayLeft.toggle("slide");
            $rightNav.hide();
        });

        $menubar.on("click", () => {
            $menuOverlayLeft.hide();
            $rightNav.toggle("slide");
            $menuOverlayRight.toggle("slide");
            $leftNav.hide();
        });

        $closeNav.on("click", () => {
            if ($rightNav.is(":visible")) {
                $rightNav.toggle("slide");
                $menuOverlayRight.toggle("slide");
            }

            if ($leftNav.is(":visible")) {
                $leftNav.toggle("slide");
                $menuOverlayLeft.toggle("slide");
            }
        });
    })();

    $(theme).on("click", function () {
        const selectedTheme = $(this).data("target");
        storageControl.add("theme", selectedTheme);
        let currentThemeHref = $('link[rel=stylesheet]')[0].href;
        let newThemeHref = currentThemeHref.replace(/theme-\w*/, selectedTheme);
        $('link[rel=stylesheet]')[0].href = newThemeHref;
    });
};