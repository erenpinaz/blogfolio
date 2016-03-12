/* ======================================================
    Filename    : site.js
    Description : Site scripts
    Author      : Nejdet Eren Pinaz
   ====================================================== */

if (!window.jQuery) {
    throw "Site scripts requires jQuery.";
}

// Begin DOM manipulation
$(function () {

    /* ====================
        Initializers
       ==================== */

    // Navigation toggle
    var $nav = $("#nav");
    var $navToggle = $("#nav-toggle");
    if ($navToggle.length > 0) {
        $navToggle.on("click", function (e) {
            e.preventDefault();

            $nav.toggleClass("toggled");
            $navToggle.find("i").toggleClass("appicon-menu")
                                .toggleClass("appicon-cancel");
        });
    }

    // Configure remodal
    var $remodal = $("a[data-remodal]");
    if ($remodal.length > 0) {
        $remodal.each(function () {
            $(this).on("click", function (e) {
                e.preventDefault();

                $.ajax({
                    url: $(this).data("url"),
                    success: function (data) {
                        $(".remodal-content").html(data);

                        var inst = $("[data-remodal-id=modal]").remodal();
                        inst.open();
                    }
                });
            });
        });
    }

    // Lazy load
    var $lazyItems = $(".lazy");
    if ($lazyItems.length > 0) {
        $lazyItems.lazyload({
            effect: "fadeIn"
        });
    }

    // Animate site logo when in site home
    var $logo = $(".logo");
    if (window.location.pathname == "/" && $logo.length > 0) {
        $logo.addClass("animated jello");
    }

    // Configure masonry with images-loaded
    var $grid = $("#grid");
    if ($grid.length > 0) {
        $grid.imagesLoaded(function () {
            $grid.masonry({
                itemSelector: ".grid-item",
                columnWidth: ".grid-sizer",
                percentPosition: true
            });
        });
    }

    // Ajaxify the contact form
    var $contactForm = $("section.contact").find("form");
    if ($contactForm.length > 0) {
        var $submit = $contactForm.find("button[type=\"submit\"]");

        $contactForm.on("submit", function (e) {
            e.preventDefault();

            if ($contactForm.valid()) {
                $.ajax({
                    cache: false,
                    url: this.action,
                    type: "POST",
                    data: $contactForm.serialize(),
                    beforeSend: function () {
                        $submit.prepend("<img class=\"loading\" src=\"/assets/images/loading.gif\"/>");
                        $submit.attr("disabled", true);
                    },
                    success: function (result) {
                        $contactForm.replaceWith("<p class=\"contact-result\">" + result.message + "</p>");
                    }
                });
            }
        });
    }
});