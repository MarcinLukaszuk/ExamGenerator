function setRemoveQuestionButtonValue(button) {

    var editButton = $(button);

    if (editButton.attr("tag") == "true") {
        editButton.attr("class", "btn btn-default");
        editButton.attr("tag", "false");
    } else {
        editButton.attr("class", "btn btn-danger");
        editButton.attr("tag", "true");
    }
}