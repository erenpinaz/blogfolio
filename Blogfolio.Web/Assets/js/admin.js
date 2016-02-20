/* ======================================================
    Filename    : admin.js
    Description : Admin scripts
    Author      : Nejdet Eren Pinaz
   ====================================================== */

if (!window.jQuery) {
    throw "Admin scripts requires jQuery.";
}

// Begin DOM manipulation
$(function () {

    /* ====================
        Initializers
       ==================== */

    // Bootstrap Table Resize Fix
    var $table = $("table[data-toggle='table']");
    if ($table.length > 0) {
        $(window).resize(function () {
            $('table[data-toggle="table"]').bootstrapTable("resetView");
        });
    }

    // TinyMCE Post section
    var configurePostBodyEditor = function () {
        var $postBody = $("textarea[data-tinymce='post']");
        if ($postBody.length > 0) {
            tinymce.baseURL = "/assets/js/tinymce";
            tinymce.init({
                selector: "textarea[data-tinymce='post']",
                autosave_interval: "20s",
                image_dimensions: false,
                relative_urls: false,
                entity_encoding: "raw",
                min_height: 500,
                plugins: [
                    "advlist autolink lists link image charmap print preview anchor",
                    "searchreplace visualblocks code fullscreen autosave",
                    "insertdatetime media table contextmenu paste"
                ],
                toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image"
            });
        }
    };
    configurePostBodyEditor();

    // TinyMCE Project section
    var configureProjectBodyEditor = function () {
        var $projectBody = $("textarea[data-tinymce='project']");
        if ($projectBody.length > 0) {
            tinymce.baseURL = "/assets/js/tinymce";
            tinymce.init({
                selector: "textarea[data-tinymce='project']",
                autosave_interval: "20s",
                entity_encoding: "raw",
                min_height: 500,
                plugins: [
                    "advlist autolink lists link charmap print preview anchor",
                    "searchreplace visualblocks code fullscreen autosave",
                    "insertdatetime contextmenu paste"
                ],
                toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist | link"
            });
        }
    };
    configureProjectBodyEditor();

    // Bind data-modal events
    var configureModalEvents = function () {
        var $modal = $("a[data-modal]");
        if ($modal.length > 0) {
            $modal.each(function () {
                $(this).on("click", function (e) {
                    e.preventDefault();
                    showModal(this.href);
                });
            });
        }
    };
    configureModalEvents();

    // Create & sort social items
    var configureSocialItemContainer = function () {
        // Rubaxa sortable
        var socialItemsContainer = document.getElementById("social-items-container");
        if (socialItemsContainer != null) {
            Sortable.create(socialItemsContainer, {
                handle: "i.appicon-move"
            });
            parseSocialItems();
        }

        // Dynamic item creation
        var $addSocialItem = $("a[data-ajax='add-social-item']");
        var $socialItemsContainer = $("#social-items-container");

        if ($addSocialItem.length > 0 && $socialItemsContainer.length > 0) {
            $addSocialItem.on("click", function (e) {
                e.preventDefault();

                $.ajax({
                    url: this.href,
                    success: function (html) {
                        $(html).appendTo($socialItemsContainer)
                            .find("input[readonly]")
                            .prop("readonly", false);

                        // Parse dynamically added items for validations and icons
                        parseSocialItems();
                    }
                });
                return false;
            });
        }
    };
    configureSocialItemContainer();

    // Slug Formatting
    /* 
     * Usage: Put "data-slug=source" to the source input and 
     * "data-slug=field" to the input that needs to be slugified
     */
    var configureSlugFormatting = function () {
        var $slugSource = $("input[data-slug='source']");
        var $slugField = $("input[data-slug='field']");
        if ($slugSource.length > 0 && $slugField.length > 0) {
            $slugSource.on("keypress", function () {
                $slugField.val(convertToSlug($(this).val()));
            });
            $slugSource.on("blur", function () {
                $slugField.val(convertToSlug($(this).val()));
            });
            $slugField.on("blur", function () {
                $(this).val(convertToSlug($(this).val()));
            });
        }
    };
    configureSlugFormatting();

    // Media Browser
    var configureMediaBrowser = function () {
        var $browser = $("#fileBrowser");
        var $browserTrigger = $("a[data-target=#fileBrowser]");
        if ($browser.length > 0 && $browserTrigger.length > 0) {
            var filesUrl = "/admin/dashboard/getfilesbytype?type=image";

            var $fileContainer = $browser.find("#files");

            var selectedFile = null;
            var $divToUpdate = $("body").find("#preview");
            var defaultImage = "/assets/images/placeholder.png";

            $browserTrigger.click(function (e) {
                e.preventDefault();

                // Get media files
                $.get(filesUrl, function (data) {
                    if ($fileContainer.length > 0) {

                        // Create unordered list containing files
                        var fileList = "<ul>";
                        if (data.length > 0) {
                            $.each(data, function (i, val) {
                                fileList += String.format("<li><a href='#' style='background-image: url({0})' data-file='{1}'></a></li>",
                                    val.thumbpath, val.path);
                            });
                            fileList += "</ul>";

                            // Display the file list
                            $fileContainer.html(fileList);

                            // Manipulate click event of each file
                            $fileContainer.find("a").on("click", function (e) {
                                e.preventDefault();

                                $fileContainer.find("a").removeClass("selected");
                                $(this).addClass("selected");

                                selectedFile = $(this).data("file");
                            });
                        } else {
                            $fileContainer.html("Upload some image files first.");
                        }
                    }

                    $("#fileBrowser").modal("show");
                });
            });

            $browser.find("#confirm").on("click", function () {
                if (selectedFile != null) {
                    $divToUpdate.find("img").prop("src", selectedFile);
                    $divToUpdate.find("input").val(selectedFile);
                } else {
                    $divToUpdate.find("img").prop("src", defaultImage);
                    $divToUpdate.find("input").val(defaultImage);
                    alert("No file selected.");
                }
                $("#fileBrowser").modal("hide");
            });
        }
    };
    configureMediaBrowser();

    // Media Uploader (plupload)
    var configureMediaUploader = function () {
        var $mediaUploader = $("#media-uploader");
        if ($mediaUploader.length > 0) {
            var uploader = new plupload.Uploader({
                container: "media-uploader",
                runtimes: "html5,flash,silverlight,html4",
                drop_element: "drop-target",
                browse_button: "pick-files",
                url: $mediaUploader.data("url"),
                filters: {
                    max_file_size: "15MB",
                    mime_types: [{
                        title: "Images",
                        extensions: "jpg,jpeg,png,gif"
                    }, {
                        title: "Archives",
                        extensions: "rar,zip"
                    }, {
                        title: "Documents",
                        extensions: "txt,doc"
                    }, {
                        title: "Audio Files",
                        extensions: "wav,mp3,ogg"
                    }, {
                        title: "Video Files",
                        extensions: "avi,mp4,wmv,3gp"
                    }]
                },
                resize: {
                    width: 1280,
                    height: 850
                },
                flash_swf_url: "/assets/js/plupload/Moxie.swf",
                silverlight_xap_url: "/assets/js/plupload/Moxie.xap"
            });

            uploader.bind("Init", function () {
                if (uploader.features.dragdrop) {
                    var target = document.getElementById("drop-target");

                    target.ondragover = function (event) {
                        event.dataTransfer.dropEffect = "copy";
                    };

                    target.ondragenter = function () {
                        this.className = "dragover";
                    };

                    target.ondragleave = function () {
                        this.className = "";
                    };

                    target.ondrop = function () {
                        this.className = "";
                    };
                }
            });

            uploader.bind("Error", function (upload, error) {
                alert(error.message);
            });

            uploader.bind("Browse", function () {
                uploader.splice();
                uploader.refresh();
            });

            uploader.bind("FilesAdded", function (up, files) {
                var queueSize = 5;
                if (up.files.length > queueSize) {
                    alert("Maximum " + queueSize + " files can be uploaded at a time.");
                    return false;
                }

                $("a[href=\"#media-browser\"").trigger("click");

                for (var i in files) {
                    if (files.hasOwnProperty(i)) {
                        up.start();

                        var row = {
                            mediaid: files[i].id,
                            name: files[i].name,
                            path: null,
                            thumbpath: null,
                            size: files[i].size,
                            type: null,
                            created: null
                        }

                        $("#medias-table").bootstrapTable("prepend", row);
                    }
                }
            });

            uploader.bind("UploadProgress", function (up, file) {
                var $previewItem = $("#medias-table").find("#" + file.id + "");
                if ($previewItem.length !== 0) {
                    $previewItem.find(".progress-bar").css("width", file.percent + "%").attr("aria-valuenow", file.percent);
                }
            });

            uploader.bind("FileUploaded", function (up, file, data) {
                var response = jQuery.parseJSON(data.response);
                var index = $("#" + file.id + "").closest("tr").data("index");
                var row = {
                    mediaid: response.id,
                    name: file.name,
                    path: response.path,
                    thumbpath: response.thumbpath,
                    size: response.size,
                    type: response.type,
                    created: response.created
                }

                $("#medias-table").bootstrapTable("updateRow", {
                    index: index,
                    row: row
                });
            });

            uploader.init();
        }
    };
    configureMediaUploader();
});

/* ====================
    Functions
   ==================== */

// Populate Post Categories
function populatePostCategories(postId) {
    var $refresh = $("button[data-refresh=\"category\"]");
    var $list = $("ul[data-list=\"category\"]");

    if ($refresh.length > 0) {
        $refresh.on("click", function () {
            if ($list.length > 0 && postId !== "undefined") {
                $.ajax({
                    cache: false,
                    url: "/admin/dashboard/populatepostcategories",
                    type: "GET",
                    data: {
                        id: postId
                    },
                    complete: function (result) {
                        if (result.status === 200) {
                            $list.empty();
                            var categories = result.responseJSON;
                            if (categories.length === 0) {
                                $list.append("<li>No categories</li>");
                            }
                            $(categories).each(function () {
                                var $listItem = $([
                                    "<li>",
                                    "<div class=\"checkbox\">",
                                    "<label>",
                                    "<input type=\"checkbox\" name=\"selectedCategories\" value=\"" + this.id + "\" " + ((this.ischecked) ? "checked" : null) + ">",
                                    this.name,
                                    "</label>",
                                    "</div>",
                                    "</li>"
                                ].join(""));
                                $listItem.prependTo($list);
                            });
                        }
                    }
                });
            }
        }).trigger("click");
    }
}

// Show Modal
function showModal(href) {
    var $modalBase = $("#modal-base");
    if ($modalBase.length > 0) {
        var processData = function () {
            var $modalForm = $modalBase.find("form").first();
            if ($modalForm.length === 0) {
                return false;
            }

            // Parse for validations
            $.validator.unobtrusive.parse($modalForm);

            // Parse for slug formatting
            var $slugSource = $modalForm.find("input[data-slug='source']");
            var $slugField = $modalForm.find("input[data-slug='field']");
            if ($slugSource.length > 0 && $slugField.length > 0) {
                $slugSource.on("keypress", function () {
                    $slugField.val(convertToSlug($(this).val()));
                });
                $slugSource.on("blur", function () {
                    $slugField.val(convertToSlug($(this).val()));
                });
                $slugField.on("blur", function () {
                    $(this).val(convertToSlug($(this).val()));
                });
            }

            $modalForm.on("submit", function () {
                $.ajax({
                    cache: false,
                    url: href,
                    data: $modalForm.serialize(),
                    type: "POST",
                    success: function (result) {
                        if (result.success) {
                            $modalBase.modal("hide");
                            var $refresh = $("button[data-refresh]");
                            if ($refresh.length > 0) {
                                $refresh.trigger("click");
                            }
                        }
                        $modalBase.html(result);
                        processData();
                    }
                });
                return false;
            });
            return false;
        };

        $modalBase.load(href, function (response, status, xhr) {
            if (status === "success") {
                $modalBase.modal({
                    backdrop: "static",
                    keyboard: false
                }, "show");
                processData();
            }
        });
    }
}

// Parse Social Items
function parseSocialItems() {

    // Initialize icon picker
    $(".social-icon-picker").fontIconPicker({
        source: [
            "appicon-duckduckgo", "appicon-aim", "appicon-delicious", "appicon-paypal", "appicon-flattr",
            "appicon-android", "appicon-call", "appicon-grooveshark", "appicon-ninetyninedesigns", "appicon-forrst",
            "appicon-digg", "appicon-spotify", "appicon-linkedin", "appicon-meetup", "appicon-vk", "appicon-plancast",
            "appicon-disqus", "appicon-rss", "appicon-cloudapp", "appicon-dropbox", "appicon-ebay", "appicon-facebook",
            "appicon-github", "appicon-github-circled", "appicon-intensedebate", "appicon-eventbrite", "appicon-scribd",
            "appicon-posterous", "appicon-stripe", "appicon-opentable", "appicon-stackoverflow", "appicon-hackernews",
            "appicon-lkdto", "appicon-eventful", "appicon-smashmag", "appicon-gplus", "appicon-wikipedia", "appicon-lanyrd",
            "appicon-calendar", "appicon-stumbleupon", "appicon-fivehundredpx", "appicon-pinterest", "appicon-bitcoin",
            "appicon-w3c", "appicon-foursquare", "appicon-html5", "appicon-ie", "appicon-reddit", "appicon-guest",
            "appicon-gowalla", "appicon-appstore", "appicon-blogger", "appicon-cc", "appicon-dribbble", "appicon-evernote",
            "appicon-flickr", "appicon-google", "appicon-viadeo", "appicon-instapaper", "appicon-weibo", "appicon-klout",
            "appicon-skype", "appicon-twitter", "appicon-youtube", "appicon-vimeo", "appicon-windows", "appicon-xing",
            "appicon-yahoo", "appicon-chrome", "appicon-email", "appicon-macstore", "appicon-myspace", "appicon-podcast",
            "appicon-amazon", "appicon-steam", "appicon-googleplay", "appicon-itunes", "appicon-plurk", "appicon-songkick",
            "appicon-lastfm", "appicon-gmail", "appicon-pinboard", "appicon-openid", "appicon-quora", "appicon-soundcloud",
            "appicon-tumblr", "appicon-eventasaurus", "appicon-wordpress", "appicon-yelp", "appicon-cart", "appicon-print",
            "appicon-angellist", "appicon-instagram", "appicon-dwolla", "appicon-appnet", "appicon-statusnet",
            "appicon-acrobat", "appicon-drupal", "appicon-buffer", "appicon-pocket", "appicon-bitbucket", "appicon-lego",
            "appicon-login"
        ],
        emptyIcon: false,
        useAttribute: false,
        attributeName: "class",
        convertToHex: false
    });

    // Parse the form
    var $form = $("form");
    if ($form.length > 0) {
        $form.removeData("validator");
        $form.removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse($form);
    }

    // Activate delete-item button
    var $deleteSocialItem = $("a[data-ajax='delete-social-item']");
    $deleteSocialItem.on("click", function (e) {
        e.preventDefault();

        $(this).closest("li").remove();
    });
}

/* ====================
    Table Formatters
   ==================== */

// Post Title Formatter
function postTitleFormatter(value, row) {
    return [
        value,
        "<ul class=\"list-unstyled list-inline\">",
        "<li><a class=\"edit\" href=\"/admin/dashboard/updatepost/" + row.postid + "\">Edit</a></li>",
        "<li><a class=\"remove\" href=\"/admin/dashboard/deletepost/" + row.postid + "\">Delete</a></li>",
        "</ul>"
    ].join("");
}

// Post Summary Formatter
function postSummaryFormatter(value) {
    var limit = 36;
    if (value.length > limit) {
        return value.substring(0, limit) + "...";
    }
    return value;
}

// Post Categories Formatter
function postCategoriesFormatter(value) {
    var result = "";
    for (var i = 0; i < value.length; i++) {
        result += "<a class=\"label label-primary\">" + value[i].name + "</a> ";
    }
    return result;
}

// Post Comments Section Formatter
function postCommentsSectionFormatter(value) {
    if (value) {
        return "Enabled";
    }
    return "Disabled";
}

// Category Name Formatter
function categoryNameFormatter(value, row) {
    return [
        value,
        "<ul class=\"list-unstyled list-inline\">",
        "<li><a class=\"edit\" href=\"/admin/dashboard/updatecategory/" + row.categoryid + "\">Edit</a></li>",
        "<li><a class=\"remove\" href=\"/admin/dashboard/deletecategory/" + row.categoryid + "\">Delete</a></li>",
        "</ul>"
    ].join("");
}

// Project Name Formatter
function projectNameFormatter(value, row) {
    return [
        value,
        "<ul class=\"list-unstyled list-inline\">",
        "<li><a class=\"edit\" href=\"/admin/dashboard/updateproject/" + row.projectid + "\">Edit</a></li>",
        "<li><a class=\"remove\" href=\"/admin/dashboard/deleteproject/" + row.projectid + "\">Delete</a></li>",
        "</ul>"
    ].join("");
}

// Project Image Formatter
function projectImageFormatter(value) {
    return "<img src='" + value + "' width='120' alt='Media Image'>";
}

// Media Name Formatter
function mediaNameFormatter(value, row) {
    return [
        value,
        "<ul class=\"list-unstyled list-inline\">",
        "<li><a class=\"remove\" href=\"/admin/dashboard/deletemedia/" + row.mediaid + "\">Delete</a></li>",
        "<li><a class=\"showurl\" href=\"" + row.path + "\">Show URL</a></li>",
        "</ul>"
    ].join("");
}

// Media Thumbnail Formatter
function mediaThumbnailFormatter(value, row) {
    if (value == null) {
        return "<div id='" + row.mediaid + "' class='media-thumb' title='" + row.name + "'>" + "<div class='progress'>" + "<div class='progress-bar progress-bar-striped active' role='progressbar' " + "aria-valuenow='0' aria-valuemin='0' aria-valuemax='100' style='width: 0'>" + "<span class='sr-only'>0 Complete</span>" + "</div>" + "</div>";
    }
    return "<div class='media-thumb' style='background-image: url(" + value + ")' title='" + row.name + "'>";
}

// Media Size Formatter
function mediaSizeFormatter(value) {
    return fileSizeIEC(value);
}

// Json Date Formatter
function jsonDateFormatter(value) {
    if (value !== null && value !== undefined && value.length > 0) {
        var date = new Date(parseInt(value.substr(6)));
        return "<abbr class='initialism' title='" + date + "'>" + calculateDateDifference(date) + "</abbr>";
    }
    return "-";
}

/* ====================
    Table Events
   ==================== */

// Post Title Events
window.postTitleEvents = {
    'click .remove': function (e, value, row, index) {
        e.preventDefault();

        var id = row.postid;
        if (id != undefined) {
            if (confirm("Proceed with deletion?") === true) {
                $.ajax({
                    url: "/admin/dashboard/deletepost",
                    type: "POST",
                    traditional: true,
                    data: JSON.stringify({
                        'id': id
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    complete: function () {
                        var $table = $("#posts-table");
                        if ($table.length > 0) {
                            $table.bootstrapTable("refresh");
                        }
                    }
                });
            }
        }
    }
};

// Post Toolbar Events
function bulkDeletePost(e) {
    e.preventDefault();

    var $table = $("#posts-table");
    if ($table.length > 0) {
        var selectedItems = $table.bootstrapTable("getSelections");
        if (selectedItems.length > 0) {
            if (confirm("Proceed with bulk operation?") === true) {

                var ids = $.map(selectedItems, function (row) {
                    return row.postid;
                });

                $.ajax({
                    url: "/admin/dashboard/bulkdeletepost",
                    type: "POST",
                    data: JSON.stringify({
                        'ids': ids
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    complete: function () {
                        $table.bootstrapTable("refresh");
                    }
                });
            }
        }
    }
}

// Category Name Events
window.categoryNameEvents = {
    'click .edit': function (e, value, row, index) {
        e.preventDefault();

        var href = this.href;
        if (href.length > 0) {
            showModal(href);
        }
    },
    'click .remove': function (e, value, row, index) {
        e.preventDefault();

        var id = row.categoryid;
        if (id != undefined) {
            if (confirm("Proceed with deletion? \nOrphaned posts will be deleted in the process.") === true) {
                $.ajax({
                    url: this.href,
                    type: "POST",
                    traditional: true,
                    data: JSON.stringify({
                        'id': id
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    complete: function () {
                        var $table = $("#categories-table");
                        if ($table.length > 0) {
                            $table.bootstrapTable("refresh");
                        }
                    }
                });
            }
        }
    }
};

// Category Toolbar Events
function bulkDeleteCategory(e) {
    e.preventDefault();

    var $table = $("#categories-table");
    if ($table.length > 0) {
        var selectedItems = $table.bootstrapTable("getSelections");
        if (selectedItems.length > 0) {
            if (confirm("Proceed with bulk operation? \nOrphaned posts will be deleted in the process.") === true) {

                var ids = $.map(selectedItems, function (row) {
                    return row.categoryid;
                });

                $.ajax({
                    url: "/admin/dashboard/bulkdeletecategory",
                    type: "POST",
                    data: JSON.stringify({
                        'ids': ids
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    complete: function () {
                        $table.bootstrapTable("refresh");
                    }
                });
            }
        }
    }
}

// Project Name Events
window.projectNameEvents = {
    'click .remove': function (e, value, row, index) {
        e.preventDefault();

        var id = row.projectid;
        if (id != undefined) {
            if (confirm("Proceed with deletion?") === true) {
                $.ajax({
                    url: "/admin/dashboard/deleteproject",
                    type: "POST",
                    traditional: true,
                    data: JSON.stringify({
                        'id': id
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    complete: function () {
                        var $table = $("#projects-table");
                        if ($table.length > 0) {
                            $table.bootstrapTable("refresh");
                        }
                    }
                });
            }
        }
    }
};

// Project Toolbar Events
function bulkDeleteProject(e) {
    e.preventDefault();

    var $table = $("#projects-table");
    if ($table.length > 0) {
        var selectedItems = $table.bootstrapTable("getSelections");
        if (selectedItems.length > 0) {
            if (confirm("Proceed with bulk operation?") === true) {

                var ids = $.map(selectedItems, function (row) {
                    return row.projectid;
                });

                $.ajax({
                    url: "/admin/dashboard/bulkdeleteproject",
                    type: "POST",
                    data: JSON.stringify({
                        'ids': ids
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    complete: function () {
                        $table.bootstrapTable("refresh");
                    }
                });
            }
        }
    }
}

// Media Name Events
window.mediaNameEvents = {
    'click .remove': function (e, value, row, index) {
        e.preventDefault();

        var id = row.mediaid;
        if (id != undefined) {
            if (confirm("Proceed with deletion?") === true) {
                $.ajax({
                    url: this.href,
                    type: "POST",
                    traditional: true,
                    data: JSON.stringify({
                        'id': id
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    complete: function () {
                        var $table = $("#medias-table");
                        if ($table.length > 0) {
                            $table.bootstrapTable("refresh");
                        }
                    }
                });
            }
        }
    },
    'click .showurl': function (e, value, row, index) {
        e.preventDefault();

        window.prompt("Ctrl+C to copy to clipboard:", row.path);
    }
};

// Media Toolbar Events
function bulkDeleteMedia(e) {
    e.preventDefault();

    var $table = $("#medias-table");
    if ($table.length > 0) {
        var selectedItems = $table.bootstrapTable("getSelections");
        if (selectedItems.length > 0) {
            if (confirm("Proceed with bulk operation?") === true) {

                var ids = $.map(selectedItems, function (row) {
                    return row.mediaid;
                });

                $.ajax({
                    url: "/admin/dashboard/bulkdeletemedia",
                    type: "POST",
                    data: JSON.stringify({
                        'ids': ids
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    complete: function () {
                        $table.bootstrapTable("refresh");
                    }
                });
            }
        }
    }
}

/* ====================
    Utils
   ==================== */

// String.format
// http://stackoverflow.com/questions/610406/javascript-equivalent-to-printf-string-format
if (!String.format) {
    String.format = function (format) {
        var args = Array.prototype.slice.call(arguments, 1);
        return format.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != "undefined"
                ? args[number]
                : match;
        });
    };
}

// Slug formatter
// http://stackoverflow.com/questions/1053902/how-to-convert-a-title-to-a-url-slug-in-jquery
function convertToSlug(url) {
    return url
        .toLowerCase()
        .replace(/ /g, "-")
        .replace(/[^\w-]+/g, "");
}

// IEC (1024) file size prefix
// http://stackoverflow.com/questions/10420352/converting-file-size-in-bytes-to-human-readable
function fileSizeIEC(a, b, c, d, e) {
    return (b = Math, c = b.log, d = 1024, e = c(a) / c(d) | 0, a / b.pow(d, e)).toFixed(2) + " " + (e ? "KMGTPEZY"[--e] + "iB" : "Bytes");
}

// SI (1000) file size prefix
// http://stackoverflow.com/questions/10420352/converting-file-size-in-bytes-to-human-readable
function fileSizeSI(a, b, c, d, e) {
    return (b = Math, c = b.log, d = 1e3, e = c(a) / c(d) | 0, a / b.pow(d, e)).toFixed(2) + " " + (e ? "kMGTPEZY"[--e] + "B" : "Bytes");
}

// Calculate Date Difference
function calculateDateDifference(dateFrom, dateTo) {
    // Calculate the difference
    dateTo = (typeof dateTo === "undefined") ? (new Date).getTime() : dateTo;
    var days = Math.floor((dateTo - dateFrom) / 1000 / 60 / 60 / 24);

    // Format and return result
    var duration = Math.floor(days / 365.242); // year
    if (duration > 1) {
        return duration + " years ago";
    }
    if (duration === 1) {
        return "a year ago";
    }
    duration = Math.floor(days / 30.4368); // month
    if (duration > 1) {
        return duration + " months ago";
    }
    if (duration === 1) {
        return "a month ago";
    }
    duration = days; // days
    if (duration > 1) {
        return duration + " days ago";
    }
    if (duration === 1) {
        return "yesterday";
    }
    return "today";
}