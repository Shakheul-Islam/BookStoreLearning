jQuery("body").on("click", ".redirect-btn", function (event) {
    event.preventDefault();
    var url = jQuery(this).attr("data-url");
    window.location.href = url;
});
