function FilterActivityListEventHandler(input, divId) {
    let timeout = null;
    timeout = setTimeout(function () {
        FilterActivityList(input, divId);
    }, 1000);
}

function FilterActivityList(tagToSearch, divId) {

    let filter, firstLevelBox, firstLevel, thirdLevelElementLi, activityLevel, activityTitle, firstLevelElement, secondLevelElement, activityElements;

    filter = tagToSearch.value.toUpperCase();
    firstLevelBox = document.getElementById(divId);
    firstLevel = firstLevelBox.getElementsByTagName('li');

    const filterValue = divId === "avaiableActivities" ? "AvailableActivities" : "SelectedActivities";
    activityElements = divId === "avaiableActivities" ? document.querySelectorAll(`[id^=${filterValue}]`) : document.getElementById("selectedActivities").getElementsByTagName(`small`);


    for (let activityElement of activityElements) {

        activityLevel = divId === "avaiableActivities" ? activityElement.nextElementSibling.innerText.split(' ').length : activityElement.innerText.split(' ').length;

        if (activityLevel === 1 || activityLevel === 2) {
            activityElement.closest('li').style.display = 'none';

        } else if (activityLevel === 3) {

            activityTitle = activityElement.parentElement.innerText;
            activityTitle = activityTitle.replace(new RegExp("\\(\\d+ \\d+ \\d+\\)|\\(Custom Activity \\)"), '');
            if (activityTitle.toUpperCase().includes(filter) || filter === "") {

                thirdLevelElementLi = activityElement.closest('li');
                thirdLevelElementLi.style.display = 'block';
                secondLevelElement = activityElement.closest('ul').closest('li');
                secondLevelElement.style.display = 'block';
                firstLevelElement = secondLevelElement.closest('ul').closest('li');
                firstLevelElement.style.display = 'block';

                if (filter != "") {

                    activityElement.closest('.collapse').classList.add('show');
                    openAccordionByAccordionToggle(secondLevelElement);

                    secondLevelElement.closest('ul').classList.add('show');
                    openAccordionByAccordionToggle(firstLevelElement);

                } else {

                    activityElement.closest('.collapse').classList.remove('show');
                    closeAccordionByAccordionToggle(secondLevelElement);

                    secondLevelElement.closest('ul').classList.remove('show');
                    closeAccordionByAccordionToggle(firstLevelElement);

                }


            } else {

                activityElement.closest('.accordion-secondlevel').style.display = 'none'

            }

        }
    }

}

function closeAccordionByAccordionToggle(element) {
    element.querySelector('.accordion-toggle').classList.remove('collapsed');
    element.querySelector('.accordion-toggle').setAttribute('aria-expanded', 'false');
}

function openAccordionByAccordionToggle(element) {
    element.querySelector('.accordion-toggle').classList.add('collapsed');
    element.querySelector('.accordion-toggle').setAttribute('aria-expanded', 'true');
}

function setAllCheckboxes(divId, sourceCheckbox) {
    $('#' + divId).find(':checkbox').each(function () {
        if (this.parentElement.parentElement.parentElement.parentElement.parentElement.style.display != 'hidden') {
            $(this).prop('checked', $(sourceCheckbox).is(':checked'));
        }
    });
}

function DesactivateSaveAndGenerateBtn() {
    if (document.getElementById('saveDropdown') != null) {
        document.getElementById('saveDropdown').setAttribute('disabled', 'true');
        document.getElementById('saveChanges').setAttribute('disabled', 'true');
        document.getElementById('saveChanges').style.cursor = "default";
        document.getElementById('saveDropdown').style.cursor = "default";

    }
    if (document.getElementById('generateTemplateBtn') != null) {
        document.getElementById('generateTemplateBtn').setAttribute('disabled', 'false');
        document.getElementById('generateTemplateBtn').style.cursor = "default";
    }

}

function ActivateSaveAndGenerateBtn() {
    if (document.getElementById('saveDropdown') != null) {
        document.getElementById('saveDropdown').removeAttribute('disabled');
        document.getElementById('saveChanges').removeAttribute('disabled');
        document.getElementById('saveChanges').style.cursor = "pointer";
        document.getElementById('saveDropdown').style.cursor = "pointer";
    }
    var thisPageUrl = window.location.search;
    if (document.getElementById('generateTemplateBtn') != null && !thisPageUrl.includes('NewPlan')) {
        document.getElementById('generateTemplateBtn').removeAttribute('disabled');
        document.getElementById('generateTemplateBtn').style.cursor = "pointer";
    }

}

function moveItemsToTheRight() {

    DesactivateSaveAndGenerateBtn();
    
    var newSelectedActivities = [];
    //$.each($("input[name='activityRight']"), function () {
    //    newSelectedActivities.push($(this).val());
    //});
    $.each($("input[name='activityLeft']:checked"), function () {
        newSelectedActivities.push($(this).val());
    });

    if (newSelectedActivities.length > 0) {
        manageActivitiesSide(newSelectedActivities, 1);
    }
}

function moveItemsToTheLeft() {

    var activitiesToMove = [];
    $.each($("input[name='activityRight']:checked"), function () {
        activitiesToMove.push($(this).val());
    });

    if (activitiesToMove.length > 0) {
        //disable btn
        DesactivateSaveAndGenerateBtn();
        manageActivitiesSide(activitiesToMove, -1);
    }
}

function moveChapter(chapterId, currentPosId, dir) {

    // Getting the list elements
    let currentItem = $('#sel_parent_chapter_' + chapterId);
    let currentLbl = $('#lbl_chapter_' + chapterId);

    let otherItem;
    if (dir < 0) {
        otherItem = currentItem.prev();
    }
    else {
        otherItem = currentItem.next();
    }

    if (otherItem.length > 0) {
        if (otherItem[0].id.endsWith("_1") && dir == -1 && !otherItem[0].id.includes("custom" || "Custom")) {
            otherItem = otherItem.prev();
        }
    }

    if (otherItem.length > 0) {
        //get position element
        let PositionHiddenElement = document.getElementById(`Position_element_${chapterId}`);
        let position = PositionHiddenElement.value;

        //other position element
        let otherChapterId = otherItem[0].id.substr(otherItem[0].id.lastIndexOf('_') + 1);
        let otherPositionHiddenElement = document.getElementById(`Position_element_${otherChapterId}`);
        let otherPosition = otherPositionHiddenElement.value;

        //get description before change elements
        let currentItemDescriptionInput = document.getElementById(`chap_Descrp_${chapterId}`);
        let currentItemDescription = currentItemDescriptionInput.value;

        let otherItemDescriptionInput = document.getElementById(`chap_Descrp_${otherChapterId}`);
        let otherItemDescription = otherItemDescriptionInput.value;

        let otherLbl = $('#lbl_chapter_' + otherItem[0].id.substr(otherItem[0].id.lastIndexOf('_') + 1));

        //replace
        currentLbl.text(currentLbl.text().replace(position, otherPosition));
        otherLbl.text(otherLbl.text().replace(otherPosition, position));

        //change positions
        PositionHiddenElement.value = otherPosition;
        otherPositionHiddenElement.value = position;

        //reset description properly
        currentItemDescriptionInput.value = currentItemDescription;
        otherItemDescriptionInput.value = otherItemDescription;


        if (dir < 0) {
            currentItem.prev().insertAfter(currentItem);
        }
        else {
            currentItem.next().insertBefore(currentItem);
        }
    }

}

function moveItem(strElem, currentPosId, dir) {

    // Getting the list elements
    var currentItem = $('#' + strElem);
    if (strElem.includes('subChapter')) {
        var currentItemPosName = currentItem[0].childNodes[3].childNodes[3].childNodes[3].name;
    }
    if (strElem.includes('act')) {
        var currentItemPosName = currentItem[0].childNodes[3].name;
    }
    
    var otherItem
    if (dir < 0) {
        otherItem = currentItem.prev();
    }
    else {
        otherItem = currentItem.next();
    }
    if (strElem.includes('subChapter')) {
        var otherItemPosName = otherItem[0].childNodes[3].childNodes[3].childNodes[3].name;
    }
    if (strElem.includes('act')) {
        var otherItemPosName = otherItem[0].childNodes[3].name;
    }

    if (otherItem.length > 0) {

        //var otherPosId = otherItem[0].childNodes[5].id;Con este sacamos un Id pero tampoco funciona
        var otherPosId = otherItem[0].childNodes[3].id;
        let otherPosChapId;

        if (otherItem[0].childNodes[1].hasChildNodes()) {//Aquí ya no entra, al no tener el id otherPostChapId parece que no funciona nada mas
            otherPosChapId = otherItem[0].childNodes[1].childNodes[3].childNodes[3].id;
        }

        //Esta sección sí funciona
        // Getting the numbers
        var otherNumber = otherItem[0].innerText.trimStart().substr(0, otherItem[0].innerText.trimStart().indexOf(' '));
        var currentNumber = currentItem[0].innerText.trimStart().substr(0, currentItem[0].innerText.trimStart().indexOf(' '));

        //Esta también
        // Getting the positions
        var intOtherPos = otherNumber.substr(otherNumber.lastIndexOf('.') + 1);
        var intCurrPos = currentNumber.substr(otherNumber.lastIndexOf('.') + 1);

        //Aquí no coge ningun valor de los dos
        // Getting current and previous activity position hidden controls
        var hiddenActPosValue = $('input[id="' + currentPosId + '"]');
        var hiddenOthPosValue = $('input[id="' + otherPosId + '"]');
        var hiddenOthPosSubchValue;

        if (otherPosChapId) {//No entra porque previamente no tenia el Id, no se ha asignado al no cerg el otherPosId
            hiddenOthPosSubchValue = $('input[id="' + otherPosChapId + '"]');
        }

        //Ambos var están vacíos
        // Exchanging the position hidden values
        hiddenActPosValue.val(intOtherPos);
        hiddenOthPosValue.val(intCurrPos);

        document.getElementsByName(currentItemPosName)[0].value = intOtherPos;
        document.getElementsByName(otherItemPosName)[0].value = intCurrPos;
        if (hiddenOthPosSubchValue) {//No tiene valor porque no se ha asignado en el if anterior al no tener valor otherPosChapId
            hiddenOthPosSubchValue.val(intCurrPos);
        }

        //El final funciona correctamente, parece solo ser que al fallar el otherPosId (de los primeros valores de la función) se ha derrumbado todo
        var elementText = currentItem[0].innerHTML.replace(currentNumber, otherNumber);
        currentItem[0].innerHTML = elementText;

        var otherText = otherItem[0].innerHTML.replace(otherNumber, currentNumber);
        otherItem[0].innerHTML = otherText;

        if (dir < 0) {
            currentItem.prev().insertAfter(currentItem);
        }
        else {
            currentItem.next().insertBefore(currentItem);
        }
    }

}


function runSearchers(ids, types) {
    let field;
    for (var i = 0; i < ids.length; i++) {
        field = document.getElementById(ids[i]);
        FilterActivityList(field, types[i])
    }
}

//word description
//common values
var target;
var sumerText;

function setCurrentStoredValues(dest, src) {
    dest.innerHTML = '';
    dest.innerHTML = src.firstElementChild.value;
}

function getTextValues(text, element) {

    element.firstElementChild.value = text;
}

let times = 0;
let clearAt;


function changeSummernoteTarget(element, e) {
    //target = element;
    //setCurrentStoredValues(sumerText, target);
    times += 1;

    if (times === 1) {
        if (target != undefined) {
            target.style.border = "";
        }
        target = element;
        target.style.border = "1px solid #004f86";
        setCurrentStoredValues(sumerText, target);
    }

    if (element.id.includes("act") && times === 1) {
        clearAt = 3;
    } else if (element.id.includes("sel_parent_subChapter") && times === 1) {
        clearAt = 2;
    } else if (element.id.includes("sel_parent_chapter") && times === 1) {
        clearAt = 1;
    }

    if (clearAt === times) {
        times = 0;
    }

}

//Change target of update after submit customActivityModal form
function OnSaveCustomActivityModalBegin(destination) {

    var form = document.getElementById('addAcitivtyForm');
    form.dataset.ajaxUpdate = '#selectedActivities';

    document.getElementById('destinationInput').value = destination;
}

//Change target of update after submit customActivityModal form
function OnDeleteCustomActivityModalBegin() {

    var form = document.getElementById('addAcitivtyForm');
    form.dataset.ajaxUpdate = '#selectedActivities';
}

//#region Custom Activity

function createCustomChapterSuccess(result) {
    var selectedUl = document.getElementById('selectedActivities');

    selectedUl.append(result);
}

function setRowIndexValueToNewChapter() {
    var selectedUl = document.getElementById('selectedActivities');
    var rowIndexInput = document.getElementById('rowIndexInput');

    var rowCount = selectedUl.children.length;
    rowIndexInput.value = rowCount;
}


function OnCustomActivityAjaxSuccess(e) {
    $('.modal-backdrop').remove();
    $('#addactividad').modal('show');
}

function SubmitCustomActivityForm(destination, chapterRowIndex, subchapterRowIndex, activityRowIndex) {

    var destinationInput = document.getElementById('destinationModal');
    var chapterPositionInput = document.getElementById('chapterPositionModal');
    var subchapterPositionInput = document.getElementById('subchapterPositionModal');
    var activityPositionInput = document.getElementById('activityPositionModal');
    var customChapterSubmit = document.getElementById('customChapterSubmit');

    destinationInput.value = destination;
    chapterPositionInput.value = chapterRowIndex;
    subchapterPositionInput.value = subchapterRowIndex;
    activityPositionInput.value = activityRowIndex;

    AddSelectedActivitesToCustomActivityForm();

    customChapterSubmit.click();
}

function AddSelectedActivitesToCustomActivityForm() {

    var selectedActsInputs = document.getElementById('selectedActivities').getElementsByTagName('input');

    var inputContainer = document.getElementById('selectedActivitiesDiv');

    inputContainer.innerHTML = '';

    for (let input of selectedActsInputs) {
        var clonedInput = input.cloneNode(true);
        clonedInput.id = '';
        inputContainer.appendChild(clonedInput);
    }
}

function CreateCustomChapter(destination) {
    var selectedUl = document.getElementById('selectedActivities');

    //Used to set the position of new CustomChapter
    var rowCount = selectedUl.children.length + 1;

    SubmitCustomActivityForm(destination, rowCount, '', '');
}

function CreateCustomSubchapter(destination, chapterPositionId) {

    var chapterPositionInput = document.getElementById(chapterPositionId);

    SubmitCustomActivityForm(destination, chapterPositionInput.value, '', '');
}

function CreateCustomActivity(destination, chapterPositionId, subChapterPositionInputName) {

    var chapterPositionInput = document.getElementById(chapterPositionId);
    var subchapterPositionInput = document.getElementsByName(subChapterPositionInputName)[0];

    SubmitCustomActivityForm(destination, chapterPositionInput.value, subchapterPositionInput.value, '');
}

function OpenCloseChaptersBehaviour(spinnerId, buttonId, fieldId) {
    let spinner = document.getElementById(spinnerId);
    let toggleBtn = document.getElementById(buttonId);
    let currentAction = '';

    toggleBtn.hidden = true;
    spinner.hidden = false;

    if (toggleBtn.classList.contains('fa-minus')) {
        toggleBtn.className = '';
        toggleBtn.className = 'fas fa-plus';
        currentAction = 'close';
    } else {
        toggleBtn.className = '';
        toggleBtn.className = 'fas fa-minus';
        currentAction = 'open'
    }

    runOpenClose(fieldId, currentAction).then(v => {
        spinner.hidden = true;
        toggleBtn.hidden = false;
    });

}

async function runOpenClose(fieldId, currentAction) {

    let activitiesDiv = document.getElementById(fieldId);
    let anchords = activitiesDiv.querySelectorAll('a.accordion-toggle');

    for (var anchord of anchords) {
        if (currentAction === 'open') {
            if (!anchord.parentElement.nextElementSibling.classList.contains('show')) {
                anchord.click();
            }
        } else {
            if (anchord.parentElement.nextElementSibling.classList.contains('show')) {
                anchord.click();
            }
        }
        await sleep(8);
    }

}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

(function () {
    //load with document
    setSumernoteEvent();


    function setSumernoteEvent() {
        let summerBox = document.getElementById('summernoteBox');
        sumerText = summerBox.nextElementSibling.getElementsByClassName("note-editable card-block")[0];

        sumerText.addEventListener('input', function () {
            getTextValues(sumerText.innerHTML, target);
        });
    }

})();