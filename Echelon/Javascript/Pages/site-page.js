/*jshint esversion: 6 */

var SitePage = function () {
    var $menuBarButton = $("#menuHome");
    var $rightNav = $("#rightSideNav");
    var $leftNav = $("#leftSideNav");
    var $closeNav = $(".sidenav__overlay");

    var $userAvatarButton = $("#headerAvatar");
    var $menuOverlayRight = $("#menuOverlayRight");
    var $menuOverlayLeft = $("#menuOverlayLeft");
    var $activeUsersMenu = $("#usersSideNav");
    var $menuUsersButton = $("#menuUsers");
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

        if ($userAvatarButton.length > 0) {
            avatarControl.setUserAvatar($userAvatarButton.data("target"));
        }

        $userAvatarButton.on("click", () => {
            $menuOverlayRight.hide();
            $leftNav.toggle("slide");

            $menuOverlayLeft.toggle("slide");
            $rightNav.hide();
        });

        $menuBarButton.on("click", () => {
            $menuOverlayLeft.hide();
            $rightNav.toggle("slide");

            $menuOverlayRight.toggle("slide");
            $leftNav.hide();
        });

        $menuUsersButton.on("click", () => {
            $menuOverlayLeft.hide();
            $menuOverlayRight.hide();
            $activeUsersMenu.toggle("slide", () => {
                let $chatContainerMessages = $(".chat-container__messages");
                console.log($activeUsersMenu.is(":visible"));
                if ($activeUsersMenu.is(":visible")) {

                    $chatContainerMessages.removeClass("col-md-12 col-sm-12");
                    $chatContainerMessages.addClass("col-md-10 col-sm-10");
                } else {
                    $chatContainerMessages.removeClass("col-md-10 col-sm-10");
                    $chatContainerMessages.addClass("col-md-12 col-sm-12");
                }
            });

            $leftNav.hide();
            $rightNav.hide();
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