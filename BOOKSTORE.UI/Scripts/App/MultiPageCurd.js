var curd_url = "";

$("body").on("click", "#save-btn", function (event) {
    event.preventDefault();
    debugger;
    var n = $(this).attr("data-curd");
    var url = $(this).attr("data-url");
    var cls = n + "-form";
    var frm = $("." + cls);

    if (frm.valid()) { // validation passed

        $("#delete-btn").prop("disabled", true);
        $("#back-btn").prop("disabled", true);

        var jsonData = PrepearJasonFormData(cls);
        var data = "{'model':" + JSON.stringify(jsonData) + "}";
        //alert(data); return;
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            url: url,
            dataType: 'json',
            data: data,
            success: function (data) {
                $("#delete-btn").prop("disabled", false);
                $("#back-btn").prop("disabled", false);

                if (data.Result) {
                    AppUtil.ShowNotification("success", data.Message)
                    $("." + n + "-id").val(data.Id);
                    $("." + n + "-data-id").attr("data-id", data.Id);
                    $("." + n + "-hidden").show();
                    $("#save-btn").html("Save");
                    try {
                        InitAfterSave();
                    }
                    catch (e) { }
                } else {
                    AppUtil.ShowNotification("error", data.Message)
                    // alertify.error(data.Message);
                }
            },
            error: function (e) {
                //alert(e.responseText);
                $("#delete-btn").prop("disabled", false);
                $("#back-btn").prop("disabled", false);
            }
        });
    }
});

$("body").on("click", ".link-delete", function (event) {
    event.preventDefault();
    //if link buttnon not contain any url in href and check in dataurl
    var url = $(this).attr("href");
    if (url == null || url.toLowerCase().indexOf("/") < 0) {
        url = $(this).attr("data-url")
    }
    $("#delete-modal-header").html($(this).attr("data-title"));
    $("#modal-delete-name").html($(this).attr("data-name"));
    $("#delete-ok-button").attr("data-url", url);
    $("#delete-ok-button").attr("data-type", "list");
    $('#modal-delete').modal('toggle');
    $("#delete-ok-button").attr("class", "btn btn-danger delete-ok");
});

$("body").on("click", "#delete-btn", function (event) {
    event.preventDefault();
    $("#modal-delete-name").html($(this).attr("data-name"));
    $("#delete-ok-button").attr("data-url", $(this).attr("data-url") + $(this).attr("data-id"));
    $("#delete-ok-button").attr("data-type", "form");
    $('#modal-delete').modal('toggle');
    $("#delete-ok-button").attr("class", "btn btn-danger ladda-button delete-ok");
});

$("body").on("click", ".delete-ok", function (event) {
    event.preventDefault();
    var url = $(this).attr("data-url");
    var type = $(this).attr("data-type");
    $("#delete-cancel-btn").prop("disabled", true);
    $.ajax({
        type: 'POST',
        url: url,
        success: function (data) {
            if (data.Result) {
                LoadList();
                $('#modal-delete').modal('toggle');
                AppUtil.ShowNotification("success", data.Message);

            } else {

                AppUtil.ShowNotification("error", data.Message)
            }
        },
        error: function (e) {
            $("#delete-cancel-btn").prop("disabled", false);
        }
    }).done(function () {
        $("#delete-cancel-btn").prop("disabled", false);
    });
});

//function

function SetCurdUrl(url) {
    //
    curd_url = url;
}

function LoadList() {
    debugger;
    var jdata = PrepearJasonData('filter-item');
    $.ajax({
        type: 'POST',
        url: curd_url,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jdata),
        success: function (data) {
            $('#content-box').html(data);
        },
        error: function (e) {

        }
    }).done(function () {

    });
}

// Filter Json Data For Searching

function PrepearJasonData(classname) {
    debugger;
    classname = "." + classname

    var jsonData = {};
    jQuery("input" + classname).each(function (index) {
        if (jQuery(this).attr("datepicker") == "datepicker") {
            jsonData[jQuery(this).attr("name")] = jQuery(this).val();//AlterDMY2MDY(jQuery(this).val());
            var a = jQuery(this).attr("datepicker");
        } else {
            jsonData[jQuery(this).attr("name")] = jQuery(this).val();

            var a = jQuery(this).attr("datepicker");
        }
    });

    jQuery("select" + classname).each(function (index) {
        jsonData[jQuery(this).attr("name")] = jQuery(this).val();
    });

    jQuery("textarea" + classname).each(function (index) {
        jsonData[jQuery(this).attr("name")] = jQuery(this).val();
    });

    //var data = "{'model':" + JSON.stringify(jsonData) + ", 'conditions':" + JSON.stringify(items) + ", 'medications':" + JSON.stringify(items2) + "}";


    return jsonData;

}

// Prepare Json Form Data for Insert
function PrepearJasonFormData(classname) {
    classname = "." + classname

    var jsonData = {};

    jQuery(classname + " input[type=text]").each(function (index) {
        if (jQuery(this).attr("datepicker") == "datepicker") {
            jsonData[jQuery(this).attr("name")] = jQuery(this).val();//AlterDMY2MDY(jQuery(this).val());
        } else {
            jsonData[jQuery(this).attr("name")] = jQuery(this).val();
        }
    });

    jQuery(classname + " input[type=email]").each(function (index) {
        jsonData[jQuery(this).attr("name")] = jQuery(this).val();
    });

    jQuery(classname + " input[type=password]").each(function (index) {
        jsonData[jQuery(this).attr("name")] = jQuery(this).val();
    });

    //for textarea input
    jQuery(classname + " textarea").each(function (index) {
        jsonData[jQuery(this).attr("name")] = jQuery(this).val();
    });
    //for select input
    jQuery(classname + " select").each(function (index) {
        jsonData[jQuery(this).attr("name")] = jQuery(this).val();
    });

    //for text inputs
    jQuery(classname + " input[type=hidden]").each(function (index) {
        jsonData[jQuery(this).attr("name")] = jQuery(this).val();
    });

    var ary = "";
    jQuery(classname + " input[type=checkbox]:checked").each(function (index) {
        var name = jQuery(this).attr("name");
        if (ary == name) {
            jsonData[jQuery(this).attr("name")] += "," + jQuery(this).val();
        } else {
            jsonData[jQuery(this).attr("name")] = jQuery(this).val();
        }
        ary = name;
    });


    jQuery(classname + " input[type=radio]:checked").each(function (index) {
        jsonData[jQuery(this).attr("name")] = jQuery(this).val();
    });
    jQuery(classname + " input[type=number]").each(function (index) {
        jsonData[jQuery(this).attr("name")] = jQuery(this).val();
    });
    return jsonData;
}
