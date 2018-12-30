
var currentIndex = 0;

function questionIterate() {
    var href = $("#addQuestion").attr("href");
    $("#addQuestion").attr("href", setHrefIndex(href, ++currentIndex));
}

function setHrefIndex(href, index) {
    return href.replace(/(?:index=)[0-9]+/i, "index=" + index);
}

function answerIterate(questionID) {
    var ajaxButton = $("#addAnswer" + questionID);
    var questionDiv = $("#question_" + questionID);
    var href = ajaxButton.attr("href");
    var answersNumber = parseInt(questionDiv.attr("tag")) + 1;

    questionDiv.attr("tag", answersNumber);
    ajaxButton.attr("href", setHrefIndex(href, answersNumber));
} 

function removeAnswer(button) {
    var buttonJQ = $(button)[0];
    var parentButton = buttonJQ.parentNode;
   var children= parentButton.children;
    for (var i = 0; i < children.length; i++) {
        var element = children[i];
    }
}

function createAnswerDivID(questionID, answerID) {
    return '#question_' + questionID +  '/'+ 'answer_' + answerID;
}


function pdfDownload(result) { 
    window.location =  getUrl ;
}

function setAnswer(questionID, answerID,button) {
   
    var element = document.getElementById("Question" + questionID + "Answer" + answerID);
    if (element.checked === true) {
        element.checked = false;
        button.innerHTML = "Not Correct";
        button.className = "btn btn-danger";
    } else {
        element.checked = true;
        button.innerHTML = "Correct";
        button.className = "btn btn-success";
    }


   
   
}