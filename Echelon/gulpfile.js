/// <binding BeforeBuild='clean' AfterBuild='build' ProjectOpened='watch' />

var gulp = require("gulp"),
    config = require("./gulpconfig")(),
    concat = require("gulp-concat"),
    uglify = require("gulp-uglify"),
    rimraf = require("rimraf"),
    cssmin = require("gulp-cssmin"),
    print = require("gulp-print"),
    plumber = require("gulp-plumber"),
    preprocess = require("gulp-preprocess"),
    jshint = require("gulp-jshint"),
    sass = require("gulp-sass"),
    clone = require("gulp-clone"),
    rename = require("gulp-rename"),
    bower = require("gulp-bower"),
    runSequence = require("gulp-run-sequence"),
    cmq = require("gulp-combine-media-queries"),

    renameOptions = { suffix: ".min" };

bowerDir = "./bower_components";

///*******************************
//          Aggregated
//*******************************/
gulp.task("watch", ["watch:js:base", "watch:js:pages", "watch:js:libs", "watch:sass"]);
gulp.task("clean", ["clean:js", "clean:css", "clean:images", "clean:icons"]);
gulp.task("min", ["min:js", "min:sass"]);
gulp.task("copy", ["copy:images", "copy:icons"]);
gulp.task("bower", function () { return bower(); });

///*******************************
//          Configuration
//*******************************/

gulp.task("build", function (cb) {

    runSequence(["clean", "bower"], ["min", "copy"], cb);
});

///*******************************
//          Sass
//*******************************/
gulp.task("min:sass", ["min:sass:bootstrap", "min:sass:site", "min:sass:pages", "min:sass:modules"]);

gulp.task("min:sass:pages", function () {
    var pages = config.sass.pages;
    return bundleSass(pages.files, pages.fileName);
});

gulp.task("min:sass:modules", function () {
    var pages = config.sass.modules;
    return bundleSass(pages.files, pages.fileName);
});

gulp.task("min:sass:bootstrap", function () {
    var bootstrap = config.sass.bootstrap;
    return bundleSass(bootstrap.files, bootstrap.fileName);
});

gulp.task("min:sass:site", function () {
    var site = config.sass.site;
    return bundleSass(site.files, site.fileName);
});

///*******************************
//          JavaScript
//*******************************/
gulp.task("min:js", ["min:js:libs", "min:js:base", "min:js:pages"]);

gulp.task("min:js:libs", function () {
    var libs = config.js.libs;
    return bundleJs(libs.files, libs.fileName);
});

gulp.task("min:js:base", function () {
    var base = config.js.base;
    return bundleJs(base.files, base.fileName, true);
});

gulp.task("min:js:pages", function () {
    var pages = config.js.pages;
    return bundleJs(pages.files, pages.fileName, true);
});

///*******************************
//          Content
//*******************************/
gulp.task("copy:images", function () {
    var images = config.resources.images;
    return copyFiles(images.files, images.destPath);
});

gulp.task("copy:icons", function () {
    var icons = config.resources.icons;
    return copyFiles(icons.files, icons.destPath);
});

///*******************************
//          Clean
//*******************************/
gulp.task("clean:images", function (cb) {
    clean(config.resources.images.destPath, cb);
});

gulp.task("clean:icons", function (cb) {
    clean(config.resources.icons.destPath, cb);
});

gulp.task("clean:js", function (cb) {
    clean(config.js.destPath, cb);
});

gulp.task("clean:css", function (cb) {
    clean(config.sass.destPath, cb);
});

///*******************************
//          Watch
//*******************************/
gulp.task("watch:js:base", function () {
    gulp.watch([config.js.base.files], ["min:js:base"]);
});

gulp.task("watch:js:pages", function () {
    gulp.watch([config.js.pages.files], ["min:js:pages"]);
});

gulp.task("watch:js:libs", function () {
    gulp.watch([config.js.libs.files], ["min:js:libs"]);
});

gulp.task("watch:sass", function () {
    gulp.watch([config.sass.sassSourceAllPath], ["min:sass"]);
});

///*******************************
//          Private
//*******************************/
function clean(folderPath, callback) {
    rimraf(folderPath, callback);
}

function bundleJs(files, fileName, jsHint) {
    var outFolder = config.js.destPath;
    var minOutFolder = config.js.minDestPath;
    var pipe = gulp.src(files).pipe(print());

    if (jsHint) {
        pipe = pipe.pipe(jshint())
            .pipe(jshint.reporter("jshint-stylish", { verbose: true }))
            .pipe(jshint.reporter("fail"))
            .on("error", logError);
    }

    pipe = pipe.pipe(concat(fileName))
        .pipe(gulp.dest(outFolder))
        .pipe(rename(renameOptions))
        .pipe(uglify()).on("error", logError)
        .pipe(gulp.dest(minOutFolder));

    return pipe;
}

function bundleSass(files, fileName) {
    var destPath = config.sass.destPath;

    for (var i in config.site.names) {
        if (config.site.names.hasOwnProperty(i)) {
            gulp.src(files)
                .pipe(print())
                .pipe(plumber())
//                .pipe(preprocess({ context: { title: "variables" + config.site.names[i] } }))
                .pipe(sass()).on("error", logError)
                .pipe(concat(fileName))
                .pipe(gulp.dest(destPath))
                .pipe(rename(renameOptions))
                .pipe(cssmin({ keepSpecialComments: 0 }))
                .pipe(gulp.dest(destPath + "/minified/"));
        }
    }
    return;
}

function copyFiles(files, toPath) {
    return gulp.src(files).pipe(gulp.dest(toPath));
}

function logError(error) {
    console.log(error.toString());
    this.emit("end");
}