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


function sendFileWithQuestions(examID) {
    var fileInput = $("#fileInput");
    var file = fileInput[0].files[0];
    var formData = new FormData();
    formData.append("FileUpload", file);
    formData.append("examID", examID);

    jQuery.ajax({
        type: "POST",
        url: jquerryFileUploadHref,
        cache: false,
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            alert(response.responseText);
        },
        failure: function (response) {
            alert(response.responseText);
        }
    });

}

function addNewAnswerToQuestion(QuestionID) {
    var questionsDiv = $("#collapse_" + QuestionID);
    var answersNumber = getAnswersNumber(QuestionID) / 2; //hidden fields

    var panelBody = $("<div></div>");
    panelBody.attr("class", "panel-body");
    panelBody.attr("id", "question_" + QuestionID + "-answer_" + answersNumber);
    panelBody.attr("style", "display: none;");

    questionsDiv.append(panelBody);
    setEditFieldToAnswer(QuestionID, answersNumber);
}

function setEditable(questionID) {

    var answersNumber = getAnswersNumber(questionID);
    showHideCollapsePanel(questionID, true);
    setEditFieldToQuestion(questionID);

    var editBtn = $("#editBtn-" + questionID);
    var saveBtn = $("#saveBtn-" + questionID);
    var cancelBtn = $("#cancelBtn-" + questionID);
    var addAnswerBtn = $("#addAnswerBtn-" + questionID);
    var removeQuestionBtn = $("#removeQuestionBtn-" + questionID);
    editBtn.hide();
    saveBtn.show();
    addAnswerBtn.show();
    cancelBtn.show();
    removeQuestionBtn.show();
    for (var i = 0; i < answersNumber; i++) {
        setEditFieldToAnswer(questionID, i);
    }
}

function cancelEditFields(questionID) {
    $("#editBtn-" + questionID).show();
    $("#saveBtn-" + questionID).hide();
    $("#cancelBtn-" + questionID).hide();
    $("#addAnswerBtn-" + questionID).hide();
    $("#removeQuestionBtn-" + questionID).hide();
    closeEditFieldToQuestion(questionID);
    closeEditFieldToAnswers(questionID);
    showHideCollapsePanel(questionID, false);
    if ($("#saveBtn-" + questionID).attr("onclick").includes("null")) {
        $("#saveBtn-" + questionID).parent().parent().parent().remove();
    }
}


async function saveAsyncChanges(questionID, questionData, answersData) {
    var obj = JSON.stringify({ questionID, questionID, questionData: questionData, answersData: answersData, examID: currentExamIDInDatabase });

    return await jQuery.ajax({
        type: "POST",
        url: jquerryAsynUpdateHref,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: obj,
        success: function (response) {
            //alert(response.responseText);
        },
        failure: function (response) {
            alert(response.responseText);
        }
    });
}

async function saveEditFields(questionIDinDatabase, questionID) {

    var questionValue = GetQuestionNewValue(questionID);
    var answersValues = GetAnswersNewValue(questionID);

    if (questionValue.newValue == "") {
        alert("Question could't be empty");
        return;
    }
    for (var i = 0; i < answersValues.length; i++) {
        if (answersValues[i].newValue == "") {
            alert("Answer could't be empty");
            return;
        }
    }
    if (questionIDinDatabase == null && questionValue.secondNewValue == "true") {
        $("#saveBtn-" + questionID).parent().parent().parent().remove();
        return;
    }

    $("#spinner-" + questionID).show();
    var callback = await saveAsyncChanges(questionIDinDatabase, questionValue, answersValues);
    if (callback.success) {
        if (callback.deleted == true) {
            $("#saveBtn-" + questionID).parent().parent().parent().remove();
            alert(callback.responseText);
            return;
        }
        $("#saveBtn-" + questionID).attr("onclick", "saveEditFields(" + callback.newQuestionID + "," + questionID + ")");
        cancelEditFields(questionID);
        setQuestionNewValue(questionID, questionValue.newValue);
        fillCallbackData(questionID, callback);
        setTimeout(function () { showHideCollapsePanel(questionID, true); }, 500);
    }
    $("#spinner-" + questionID).hide();
}

function fillCallbackData(panelID, callback) {
    var collapsePanel = $("#collapse_" + panelID);
    collapsePanel.empty();
    console.log(callback);
    for (var i = 0; i < callback.data.length; i++) {


        var panelBody = $("<div></div>");
        panelBody.attr("class", "panel-body");
        panelBody.attr("id", "question_" + panelID + "-answer_" + i);

        if (callback.data[i].IfCorrect == true) {
            panelBody.attr("style", "background-color: #28a745");
        } else {
            panelBody.attr("style", "background-color: red");
        }
        panelBody[0].innerHTML = callback.data[i].TextAnswer;
        collapsePanel.append(panelBody);
    }
}


function setQuestionNewValue(QuestionID, value) {
    var questionLabel = $("#questionLabel_" + QuestionID);
    questionLabel[0].innerText = value;
}

function setAnswerNewValue(QuestionID, AnswerID, value) {
    var oldAnswerPanel = $("#question_" + QuestionID + "-answer_" + AnswerID);
    oldAnswerPanel[0].innerHTML = value;
}

function setAnswersNewColors(QuestionID) {
    var answerID = 0;
    var answerStringID = "editPanelquestion_" + QuestionID + "-answer_" + answerID;
    var answerPanelBody = $("#" + answerStringID);

    while (answerPanelBody != null && answerPanelBody.length > 0) {
        var newAnswerValue = $("#ButtonEditquestion_" + QuestionID + "-answer_" + answerID).attr("tag");
        var element = $("#question_" + QuestionID + "-answer_" + answerID)
        if (newAnswerValue == "true") {
            element.attr("style", "background-color: #28a745");
        } else {
            element.attr("style", "background-color: red");
        }
        answerID++;
        answerStringID = "editPanelquestion_" + QuestionID + "-answer_" + answerID;
        answerPanelBody = $("#" + answerStringID);
    }
}



function GetQuestionNewValue(questionID) {
    var questionNewValue = $("#questionEdit_" + questionID).val();
    var questionOldValue = $("#questionLabel_" + questionID)[0].innerText;
    var questionDeleteValue = $("#removeQuestionBtn-" + questionID).attr("tag");

    return { newValue: questionNewValue, oldValue: questionOldValue, secondNewValue: questionDeleteValue };
}

function GetAnswersNewValue(QuestionID) {
    var answersList = [];

    var answerID = 0;
    var answerStringID = "editPanelquestion_" + QuestionID + "-answer_" + answerID;
    var answerPanelBody = $("#" + answerStringID);

    while (answerPanelBody != null && answerPanelBody.length > 0) {
        var newAnswerImput = $("#Editquestion_" + QuestionID + "-answer_" + answerID);
        var newAnswerCheckbox = $("#ButtonEditquestion_" + QuestionID + "-answer_" + answerID);
        var oldAnswerPanel = $("#question_" + QuestionID + "-answer_" + answerID);
        var removeAnswerCheckbox = $("#ButtonRemovequestion_" + QuestionID + "-answer_" + answerID);

        var editedAnswer = { newValue: newAnswerImput.val(), oldValue: oldAnswerPanel[0].innerHTML.trim(), secondNewValue: newAnswerCheckbox.attr("tag") };

        answerID++;
        answerStringID = "editPanelquestion_" + QuestionID + "-answer_" + answerID;
        answerPanelBody = $("#" + answerStringID);
        if (removeAnswerCheckbox.attr("tag") == "false") {
            answersList.push(editedAnswer);
        }
    }
    return answersList;
}

function checkIfCollapsed(panelID) {
    var collapsePanel = $("#collapse_" + panelID);
    if (collapsePanel.hasClass("collapse")) {
        return true;
    }
    return false;
}

function showHideCollapsePanel(panelID, show) {
    var collapsePanel = $("#collapse_" + panelID);
    if (show == true) {
        collapsePanel.collapse("show");
    } else {
        collapsePanel.collapse("hide");
    }
}

function getAnswersNumber(questionID) {
    return $("#collapse_" + questionID).children().length;
}

function setEditFieldToQuestion(questionID) {

    var questionLabel = $("#questionLabel_" + questionID);
    var editField = CreateEditField("questionEdit_" + questionID, questionLabel[0].innerText);

    questionLabel.hide();
    questionLabel.after(editField);
}

function closeEditFieldToQuestion(questionID) {
    $("#questionLabel_" + questionID).show();
    $("#questionEdit_" + questionID).remove();
    if ($("#removeQuestionBtn-" + questionID).attr("tag") == "true") {
        setRemoveQuestionButtonValue($("#removeQuestionBtn-" + questionID));
    }
    return;
}

function setEditFieldToAnswer(questionID, answerID) {

    var answerStringID = "question_" + questionID + "-answer_" + answerID;
    var answerPanelBody = $("#" + answerStringID);
    answerPanelBody.hide();
    var panelBody = $("<div></div>");
    panelBody.attr("class", "panel-body");
    panelBody.attr("id", "editPanel" + answerStringID);
    var editField = CreateEditField("Edit" + answerStringID, answerPanelBody[0].innerHTML.trim());
    var editButton = CreateEditButton("ButtonEdit" + answerStringID, answerPanelBody.attr("style").includes("red") ? false : true);

    var removeButton = CreateRemoveButton("ButtonRemove" + answerStringID, false);

    panelBody.append(CreateDivWithCollumn(4).append(editField));
    panelBody.append(CreateDivWithCollumn(4).append(editButton));
    panelBody.append(CreateDivWithCollumn(4).append(removeButton));
    answerPanelBody.after(panelBody);
}

function closeEditFieldToAnswers(QuestionID) {

    var answerID = 0;
    var answerStringID = "question_" + QuestionID + "-answer_" + answerID;
    var answerPanelBody = $("#editPanel" + answerStringID);
    var answerDisplayPanelBody = $("#" + answerStringID);

    while (answerPanelBody != null && answerPanelBody.length > 0 || answerID < 20) {
        if (answerDisplayPanelBody.attr("tag") == "remove") {
            answerDisplayPanelBody.remove();
        }
        if (answerDisplayPanelBody.html() == "") {
            answerDisplayPanelBody.remove();
        }
        answerDisplayPanelBody.show();
        answerPanelBody.remove();
        answerID++;
        answerStringID = "question_" + QuestionID + "-answer_" + answerID;
        answerPanelBody = $("#editPanel" + answerStringID);
        answerDisplayPanelBody = $("#" + answerStringID);
    }
    return;
}

function CreateEditField(name, value) {
    var editField = $("<input></input>");
    editField.attr("type", "text");
    editField.attr("class", "form-control");
    editField.attr("name", name);
    editField.attr("id", name);
    editField.attr("value", value);
    return editField;
}

function CreateEditButton(name, value) {
    var editButton = $("<input></input>");
    editButton.attr("type", "button");
    editButton.attr("name", name);
    editButton.attr("id", name);
    editButton.attr("onclick", "setButtonValue(this)");

    if (value == true) {
        editButton.attr("value", "Correct");
        editButton.attr("class", "btn btn-success");
        editButton.attr("tag", "true");
    } else {
        editButton.attr("value", "Not Correct");
        editButton.attr("class", "btn btn-danger");
        editButton.attr("tag", "false");
    }
    return editButton;
}

function CreateRemoveButton(name, value) {
    var editButton = $("<input></input>");
    editButton.attr("type", "button");
    editButton.attr("name", name);
    editButton.attr("id", name);
    editButton.attr("onclick", "setRemoveButtonValue(this)");
    editButton.attr("value", "Delete");

    if (value == true) {
        editButton.attr("class", "btn btn-danger");
        editButton.attr("tag", "true");
    } else {
        editButton.attr("class", "btn btn-secondary");
        editButton.attr("tag", "false");
    }
    return editButton;
}

function CreateDivWithCollumn(collumnNumbers) {
    var divv = $("<div></div>");
    divv.attr("class", "col-md-4");
    return divv;
}

function setButtonValue(button) {

    var editButton = $(button);

    if (editButton.attr("tag") == "false") {
        editButton.attr("value", "Correct");
        editButton.attr("class", "btn btn-success");
        editButton.attr("tag", "true");
    } else {
        editButton.attr("value", "Not Correct");
        editButton.attr("class", "btn btn-danger");
        editButton.attr("tag", "false");
    }
}

function setRemoveButtonValue(button) {
    var editButton = $(button);
    var editInput = $("#" + editButton.attr("id").replace("ButtonRemove", "Edit"));
    var editPanel = $("#" + editButton.attr("id").replace("ButtonRemove", "editPanel"));
    var displayPanel = $("#" + editButton.attr("id").replace("ButtonRemove", ""));


    if (editButton.attr("tag") == "false") {
        editButton.attr("class", "btn btn-danger");
        editButton.attr("tag", "true");
        displayPanel.attr("tag", "remove");
        if (editInput.val() == "") {
            editPanel.remove();
            displayPanel.remove();
        }
    } else {
        editButton.attr("class", "btn btn-seconsdary");
        editButton.attr("tag", "false");
        displayPanel.removeAttr("tag");
    }
}