/*jshint esversion: 6 */

var SitePage = function () {
    const menubar = $("#menuGlobe");

    //constructor method
    (function () {
        menubar.on("click",
             () => {
                 $("#mySidenav").toggle("slide");
             });
    })();
};