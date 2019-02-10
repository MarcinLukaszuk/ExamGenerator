function addNewQuestion() {
    var numer = 0;
    var panelGroup = $(".panel-group");
    var panelPrimary = $("<div></div>");
    var panelHeading = $("<div></div>");
    var panelCollapse = $("<div></div>");
    numer = panelGroup.attr("tag");
    panelGroup.attr("tag", -(-numer - 1));
    panelPrimary.append(panelHeading);
    panelPrimary.append(panelCollapse);
    panelGroup.prepend(panelPrimary);

    panelPrimary.attr("class", "panel panel-primary");
    panelHeading.attr("class", "panel-heading");
    panelHeading.attr("style", "overflow:auto");

    panelHeading.append(createHeaderGroup(numer));
    panelHeading.append(createSaveGroup(numer));
    panelHeading.append(createEditGroup(numer));
    panelHeading.append(createAnsGroup(numer));

    panelCollapse.attr("id", "collapse_" + numer);
    panelCollapse.attr("class", "panel-collapse collapse");

    setEditable(numer);
    addNewAnswerToQuestion(numer);
    addNewAnswerToQuestion(numer);
}

function createSaveGroup(number) {
    var group = $("<div></div>");
    group.attr("class", "btn-group");
    group.attr("role", "group");
    group.attr("aria-label", "saveGroup " + number);

    var button = $("<input></input>");
    button.attr("type", "button");
    button.attr("class", "btn btn-default");
    button.attr("id", "editBtn-" + number);
    button.attr("onclick", "setEditable(" + number + ")");
    button.attr("value", "Edit");

    group.append(button);
    return group;
}

function createEditGroup(number) {
    var group = $("<div></div>");
    group.attr("class", "btn-group");
    group.attr("role", "group");
    group.attr("aria-label", "editGroup " + number);

    var cancelButton = $("<input></input>");
    cancelButton.attr("type", "button");
    cancelButton.attr("class", "btn btn-default");
    cancelButton.attr("id", "cancelBtn-" + number);
    cancelButton.attr("onclick", "cancelEditFields(" + number + ")");
    cancelButton.attr("value", "Cancel");
    cancelButton.attr("style", "display: none;");

    var saveButton = $("<button></button>");
    saveButton.attr("id", "saveBtn-" + number);
    saveButton.attr("class", "btn btn-default");
    saveButton.attr("onclick", "saveEditFields(null," + number + ")");
    saveButton.attr("style", "display: none;");
    saveButton.html("Save");

    var spinner = $("<i></i>");
    spinner.attr("id", "spinner-" + number);
    spinner.attr("class", "fa fa-spinner fa-spin");
    spinner.attr("style", "display: none;");


    saveButton.append(spinner);
    group.append(cancelButton);
    group.append(saveButton);
    return group;
}

function createAnsGroup(number) {
    var group = $("<div></div>");
    group.attr("class", "btn-group");
    group.attr("role", "group");
    group.attr("aria-label", "addAnsGroup " + number);

    var ansButton = $("<input></input>");
    ansButton.attr("id", "addAnswerBtn-" + number);
    ansButton.attr("type", "button");
    ansButton.attr("class", "btn btn-default");
    ansButton.attr("onclick", "addNewAnswerToQuestion(" + number + ")");
    ansButton.attr("value", "Add Answer");
    ansButton.attr("style", "display: none;");
    group.append(ansButton);

    return group;
}

function createHeaderGroup(number) {
    var group = $("<h4></h4>");
    group.attr("class", "panel-title col-md-9");

    var aElement = $("<a></a>");
    aElement.attr("data-toggle", "collapse");
    aElement.attr("id", "questionLabel_" + number);
    aElement.attr("href", "#collapse_" + number);
    aElement.attr("aria-expanded", "false");
    group.append(aElement);
    return group;
}