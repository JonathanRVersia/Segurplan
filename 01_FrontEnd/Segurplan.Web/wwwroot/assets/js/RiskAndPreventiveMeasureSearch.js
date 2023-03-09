function DeleteSearchTab(id) {
    switch (id) {
        case "chapter-delete":
            deleteFormDropdown("capitulo");
            deleteFormDropdown("subcapitulo");
            deleteFormDropdown("actividad");
            //deleteFormDropdown("riesgo");
            deleteFormDropdown("medida");
            break;
        case "Sub-chapter-delete":
            deleteFormDropdown("subcapitulo");
            deleteFormDropdown("actividad");
            //deleteFormDropdown("riesgo");
            deleteFormDropdown("medida");
            break;
        case "activities-delete":
            deleteFormDropdown("actividad");
            //deleteFormDropdown("riesgo");
            deleteFormDropdown("medida");
            break;
        case "risk-delete":
            deleteFormDropdown("riesgo");
            break;
        case "measure-delete":
            deleteFormDropdown("medida");
            break;
        case "delete-all":
            clearAllSearchTabs();
            break;
    }
}

function deleteFormDropdown(id) {
    document.getElementById(id).value = null;
}

function clearAllSearchTabs() {
    deleteFormDropdown("capitulo");
    deleteFormDropdown("subcapitulo");
    deleteFormDropdown("actividad");
    deleteFormDropdown("riesgo");
    deleteFormDropdown("medida");
}

function PageNumberChange() {
    document.getElementById("search").submit();
}

function HideBorradorCheck() {
    document.getElementById("borradorCheckbox").style.visibility = 'hidden';
}

function ShowBorradorCheck() {
    document.getElementById("borradorCheckbox").style.visibility = 'visible';
}

function ChangePage(direction, e) {
    let currentNumber = document.getElementById("Pagination-page-number"),
        totalCount = document.getElementById("TotalCount"),
        pageSize = document.getElementById("Rows-per-page"),
        totalPages = parseInt(Math.ceil(totalCount.value / pageSize.value));

    switch (direction) {
        case "next":
            if (parseInt(currentNumber.value) < totalPages) {
                currentNumber.value = parseInt(currentNumber.value) + 1;
            } else {
                e.preventDefault();
            }
            break;

        case "previous":
            if (parseInt(currentNumber.value) > 1) {
                currentNumber.value = parseInt(currentNumber.value) - 1;
            } else {
                e.preventDefault();
            }
            break;
    }
}


function ChangeDependantData(selectedElement, numberFieldId, target, childDropdownId, childsChildsId) {
    let childDropdown = document.getElementById(childDropdownId);
    childDropdown.innerHTML = ""
    let selectedElementId = selectedElement.value;
    document.getElementById(numberFieldId).value = parseInt(selectedElement.selectedOptions[0].innerText.split(":")[0]);
    if (selectedElementId == '- Seleccionar -' && numberFieldId=='selectedChapterNumber') {
        HideBorradorCheck();
    } else {
        ShowBorradorCheck();
    }
    if (target === 'currentSubChapter') {
        document.getElementById('actividad').innerHTML = '';
        //document.getElementById('riesgo').innerHTML = '';
        document.getElementById('medida').innerHTML = '';
    }
    else if (target === 'actividad') {
        //document.getElementById('riesgo').innerHTML = '';
        document.getElementById('medida').innerHTML = '';
    }
    //else if (target == "riesgo") {
    //    document.getElementById('riesgo').innerHTML = '';
    //    document.getElementById('medida').innerHTML = '';
    //}
    
    $.ajax({
        type: "GET",
        url: '/RisksAndPreventiveMeasures?handler=DependantData',
        data: {
            __RequestVerificationToken: antiForgeryGen(),
            selectedId: selectedElementId,
            target: target
        },
        success:
            function (data) {
                //fill current dropdown
                switch (target) {
                    case "currentSubChapter":
                        if (data.subChapterVersion.length > 0) {
                            FillDropdown(childDropdown, data.subChapterVersion);
                        }
                        break;
                    case "actividad":
                        if (data.activities.length > 0) {
                            FillDropdown(childDropdown, data.activities);
                        }
                        break;
                    case "medida":
                        let riesgo_medidas = [data.risks, data.preventiveMeasures]
                        if (/*data.risks.length > 0 ||*/ data.preventiveMeasures.length > 0) {
                            FillDropdown(childDropdown, riesgo_medidas);
                        }
                        break;
                }
                childDropdown.disabled = false;
                $('.selectpicker').selectpicker('refresh');
            },
        error:
            function () {
                $('.selectpicker').selectpicker('refresh');
            }
    });


}

function FillDropdown(target, options) {
    target.innerHTML = "";
    if (target.id == "medida") {
        //if (options[0].length > 0) {
        //    for (var option of options[0]) {
        //        target.innerHTML += `<option value="${option.id}">${option.code} : ${option.name}</option>`;
        //    }
        //}
        if (options[1].length > 0) {
            let dropdownMedidas = document.getElementById('medida');
            for (var option of options[1]) {
                let appendOption = document.createElement("option")
                appendOption.text = option.code + " : " + option.description.replace(/<[^>]+>/g, '');
                appendOption.value = option.description
                dropdownMedidas.appendChild(appendOption)
            }
        }
    }
    else {
        for (var option of options) {
            if (target.id == "actividad") {
                target.innerHTML += `<option value="${option.id}">${option.number} : ${option.title}</option>`;
            }
            else {
                target.innerHTML += `<option value="${option.idSubchapter}">${option.number} : ${option.title}</option>`;
            }
        }
    }
    
}

function SetNumberValue(selectedElement, id) {
    document.getElementById(id).value = parseInt(selectedElement.selectedOptions[0].innerText.split(":")[0]);    
}