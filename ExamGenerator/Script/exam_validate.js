function fileLength(input) {
    if (input.files.length === 0)
        $('#Submit').attr("class", "btn btn-success hidden");
    else
        $('#Submit').attr("class", "btn btn-success");
}

function validate() {
    var fileInput = $("#fileInput");
    var file = fileInput[0].files[0];
    var formData = new FormData();
    formData.append("FileUpload", file);
    $("#spinner").show();

    jQuery.ajax({
        type: "POST",
        url: jquerryFileUploadHref,
        cache: false,
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            if (response.success === true) {
                window.location.href = response.responseHref;
            } else if (response.failure === true) {
                alert(response.responseText);
                window.location.reload();
            }
        },
        failure: function (response) {
            if (response.failure === true) {
                alert(response.responseText);
                window.location.reload();
            }
        }
    });
}