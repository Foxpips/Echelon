module.exports = function () {
    var rootPath = "./",
        sassSourcePath = rootPath + "Content/Sass/",
        jsSourcePath = rootPath + "Javascript/",
        imgSourcePath = rootPath + "Content/Images",
        outputPath = rootPath + "assets/",
        bowerSourcePath = rootPath + "bower_components/";

    var config = {
        site: { names: ["Echelon"] },

        js: {
            destPath: outputPath + "js",
            minDestPath: outputPath + "js/minified",

            libs: {
                fileName: "libs.js",
                files: [
                    bowerSourcePath + "jquery/dist/jquery.js",
                    bowerSourcePath + "history.js/scripts/bundled/html5/jquery.history.js",
                    bowerSourcePath + "bootstrap-sass-official/assets/javascripts/bootstrap/carousel.js",
                    bowerSourcePath + "bootstrap-sass-official/assets/javascripts/bootstrap/transition.js",
                    bowerSourcePath + "bootstrap-sass-official/assets/javascripts/bootstrap.js"
                ]
            },

            base: {
                fileName: "base.js",
                files: [jsSourcePath + "Common/*.js", jsSourcePath + "Controls/*.js"]
            },

            pages: {
                fileName: "pages.js",
                files: [jsSourcePath + "Pages/*.js"]
            }
        },

        sass: {
            destPath: outputPath + "css",
            sassSourceAllPath: sassSourcePath + "**/*.scss",

            site: {
                fileName: "site.css",
                files: [sassSourcePath + "site.scss"]
            },

            pages: {
                fileName: "pages.css",
                files: [sassSourcePath + "Pages/*.scss"]
            },

            shared: {
                fileName: "shared.css",
                files: [sassSourcePath + "Shared/*.scss"]
            },

            modules: {
                fileName: "modules.css",
                files: [sassSourcePath + "Modules/*.scss"]
            },

            bootstrap: {
                fileName: "bootstrap.css",
                files: [sassSourcePath + "bootstrap.scss"]
            }
        },

        resources: {
            images: {
                destPath: outputPath + "imgs",
                files: imgSourcePath + "/*"
            },
            icons : {
                destPath: "fonts",
                files: bowerSourcePath + "/font-awesome/fonts/*.*"
            }
        }
    };

    return config;
};


