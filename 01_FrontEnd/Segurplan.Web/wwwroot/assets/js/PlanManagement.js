function DisableOnCloseValidation() {
    let form = document.getElementById("main").querySelectorAll("[required]");

    for (var i of form) {
        i.removeAttribute("required");
    }
}


function changeDefaultValue(propName, targetId, inputId) {

    let sumernoteBox = document.getElementById(targetId).querySelector(".note-editable")
    sumernoteBox.innerHTML = planDetailsDefaultData[propName];

    let input = document.getElementById(inputId);
    input.value = sumernoteBox.innerHTML;
}
function DownloadFile() {
    //activar spinner
    var IsEvaluation = document.getElementById("Plan_GeneralData_IsEvaluation").value;
    document.getElementById("PlanIsEvaluation").value = IsEvaluation;

    var TemplateForCreateDocument = 'ER nuevo formato tabla 2 SOLO CAP';
    document.getElementById("PlanTemplateName").value = TemplateForCreateDocument;
    document.getElementById("loading").style.visibility = "visible";
    checkCookieDownload()
    var FormCreateDocument = document.getElementById('FormCreateDocument');
    FormCreateDocument.submit();
}
function getCookieDownload(name) {
    var parts = document.cookie.split(name + "=");
    if (parts.length == 2) return parts.pop().split(";").shift()
}

function expireCookieDownload(name) {
    document.cookie = encodeURIComponent(name) + "=deleted;expires=" + new Date(0).toUTCString();
}
function setFormToken() {
    var downloadToken = new Date().getTime();
    return downloadToken;
}
function checkCookieDownload() {
    var attempts = 2400
    var downloadTimer = window.setInterval(function () {
        var token = getCookieDownload('downloadToken')
        if (token == "downloaded" || attempts == 0) {
            //desactivar spinner
            document.getElementById("loading").style.visibility = "hidden";
            expireCookieDownload("downloadToken")
            window.clearInterval(downloadTimer)
        }
        attempts--
    }, 1000)
}