/// <binding BeforeBuild='build' AfterBuild='build' ProjectOpened='watch' />

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

    bowerDir= "./bower_components";

///*******************************
//          Aggregated
//*******************************/
gulp.task("watch", ["watch:js:base", "watch:js:pages", "watch:js:libs", "watch:sass"]);
gulp.task("clean", ["clean:js", "clean:css", "clean:images"]);
gulp.task("min", ["min:js", "min:sass"]);
gulp.task("copy", ["copy:images"]);
gulp.task("bower", function () {
    return bower();
        //.pipe(gulp.dest(bowerDir));
});

///*******************************
//          Configuration
//*******************************/

gulp.task("build", function (cb) {

    runSequence(["clean", "bower"], ["min", "copy"], cb);
});

///*******************************
//          Sass
//*******************************/
gulp.task("min:sass", ["min:sass:bootstrap", "min:sass:layout", "min:sass:pages"]);

gulp.task("min:sass:pages", function () {
    var v = config.sass.pages;
    return bundleSass(v.files, v.fileName);
});

gulp.task("min:sass:bootstrap", function () {
    var v = config.sass.bootstrap;
    return bundleSass(v.files, v.fileName);
});

gulp.task("min:sass:layout", function () {
    var v = config.sass.site;
    return bundleSass(v.files, v.fileName);
});

///*******************************
//          JavaScript
//*******************************/
gulp.task("min:js", ["min:js:libs", "min:js:base", "min:js:pages"]);

gulp.task("min:js:libs", function () {
    var v = config.js.libs;
    return bundleJs(v.files, v.fileName);
});

gulp.task("min:js:base", function () {
    var v = config.js.base;
    return bundleJs(v.files, v.fileName, true);
});

gulp.task("min:js:pages", function () {
    var v = config.js.pages;
    return bundleJs(v.files, v.fileName, true);
});

///*******************************
//          Resources
//*******************************/
gulp.task("copy:images", function () {
    var v = config.resources;
    return copyFiles(v.images.files, v.destPath);
});

gulp.task("bootstrap", function () {
    return gulp.src(bowerDir + "/bootstrap-sass-official/assets/stylesheets/**.*")
        .pipe(gulp.dest("./out/bootstrap"));
});

gulp.task("icons", function () {
    return gulp.src(bowerDir + "/font-awesome/fonts/**.*")
        .pipe(gulp.dest("./out/icons"));
});

///*******************************
//          Clean
//*******************************/
gulp.task("clean:images", function (cb) {
    clean(config.resources.destPath, cb);
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
            .on("error", swallowError);
    }
    
    pipe = pipe.pipe(concat(fileName))
        .pipe(gulp.dest(outFolder))
        .pipe(rename(renameOptions))
        .pipe(uglify()).on("error", swallowError)
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
                .pipe(sass()).on("error", swallowError)
                .pipe(concat(fileName))
                .pipe(gulp.dest(destPath)) 
                .pipe(rename(renameOptions))
                .pipe(cssmin({ keepSpecialComments: 0 }))
                .pipe(gulp.dest(destPath+"/minified/"));
        }
    }
    return;
}

function copyFiles(files, toPath) {
    return gulp.src(files).pipe(gulp.dest(toPath));
}

function swallowError(error) {
    console.log(error.toString());
    this.emit("end");
}