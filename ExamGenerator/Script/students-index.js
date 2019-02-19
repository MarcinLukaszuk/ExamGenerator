$(function () {

    $(document).on('change', ':file', function () {
        var input = $(this),
            numFiles = input.get(0).files ? input.get(0).files.length : 1,
            label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
        input.trigger('fileselect', [numFiles, label]);
    });


    $(document).ready(function () {
        $(':file').on('fileselect', function (event, numFiles, label) {

            var input = $(this).parents('.input-group').find(':text'),
                log = numFiles > 1 ? numFiles + ' files selected' : label;

            if (input.length) {
                input.val(log);
            } else {
                if (log) alert(log);
            }
        });
    });
});

function sendFileWithStudents() {
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
            alert(response.responseText);
            location.reload();
        },
        failure: function (response) {
            alert(response.responseText);
        }
    });
}