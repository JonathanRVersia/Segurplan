//checked measures values between pages
var CheckedValues = [];


function Save(type) {
    document.getElementById('save-type').value = type;
    //document.getElementById('Save-changes-form').submit();
}

function ModalSearchSubmit() {

    let codeInputValue = GetPreventiveMeasureCodeValue(),
        descriptionInputValue = GetPreventiveMeasureDescriptionValue()
    initialWordInputValue = GetPreventiveInitialWordInputValue();

    GetPage(1, codeInputValue, descriptionInputValue, initialWordInputValue);
    //if (codeInputValue != '' || descriptionInputValue != '') {
    //    GetPage(1, codeInputValue, descriptionInputValue);
    //}
}

function GetPreventiveInitialWordInputValue() {
    return document.getElementById('preventiveMeasureInitialWord').value;
}

function GetPreventiveMeasureCodeValue() {
    return document.getElementById('preventiveMeasureCode').value;
}

function GetPreventiveMeasureDescriptionValue() {
    return document.getElementById('preventiveMeasureDescription').value;
}

function ChangeModalPage(direction, e) {

    let currentNumber = document.getElementById("measures-page-number"),
        totalCount = document.getElementById("measures-TotalCount"),
        pageSize = document.getElementById("measures-Rows-per-page");

    TempStoreCheckedValues();

    switch (direction) {
        case "next":
            if (parseInt(totalCount.value) / parseInt(pageSize.value) != parseInt(currentNumber.value)) {
                currentNumber.value = parseInt(currentNumber.value) + 1
                GetPage(currentNumber.value, GetPreventiveMeasureCodeValue(), GetPreventiveMeasureDescriptionValue(), GetPreventiveInitialWordInputValue());

            } else {
                e.preventDefault();
            }
            break;
        case "previous":
            if (parseInt(currentNumber.value) > 1) {
                currentNumber.value = parseInt(currentNumber.value) - 1;
                GetPage(currentNumber.value, GetPreventiveMeasureCodeValue(), GetPreventiveMeasureDescriptionValue(), GetPreventiveInitialWordInputValue());
            } else {
                e.preventDefault();
            }
            break;
    }
}

function DeletePageTableRow(rowId) {

    document.getElementById(rowId).remove();
    selectedMeasuresCodes = [];
    let elements = document.getElementsByClassName('preventiveMeasuresTable'),
        valuesToTable = [];

    for (var element of elements) {
        valuesToTable.push({
            id: element.getElementsByClassName('measures-table-id')[0].value,
            description: element.getElementsByClassName('measures-table-description')[0].value,
            code: element.getElementsByClassName('measures-table-code')[0].value
        });
    }

    document.getElementById('preventive-measures-page-table-body').innerHTML = "";
    RenderTable(valuesToTable);
}

function GetPage(page, code, description, initialWord) {
    document.getElementById("loading").style.visibility = "visible";
    $.ajax({
        type: "POST",
        url: '/Models/RisksEvaluation/AllocationOfRisksAndPreventiveMeasures/Details/' + GetRiskAndPreventiveMeasuresId() + '/' + GetRiskAndPreventiveMeasuresEdit() + '?handler=MeasuresListPagination',
        data: {
            __RequestVerificationToken: antiForgeryGen(),
            nextPage: page,
            measureCode: code,
            measureDescription: description,
            initialWord: initialWord
        },
        success:
            function (data) {
                if (data != null) {
                    RenderNextPage(data);
                    document.getElementById("loading").style.visibility = "hidden";
                }
            },
        error:
            function () {

            }
    });

}

function RenderNextPage(data) {

    let tableField = document.getElementById('preventive-measures-modal-table-body'),
        paginartionCountField = document.getElementById('preventive-measures-modal-pagination-count');
    let lastId;

    if (tableField.lastElementChild != null) {
        lastId = parseInt(tableField.lastElementChild.id.substr(tableField.lastElementChild.id.lastIndexOf("-") + 1)) + 1;
    } else {
        lastId = 1;
    }

    tableField.innerHTML = "";
    paginartionCountField.innerHTML = "";


    let isChecked;
    for (var d of data.preventiveMeasures) {
        isChecked = CheckedValues.some(x => x.id == d.id);

        tableField.innerHTML += `<tr id="Modal-${lastId}">
                            <td class="text-center">
                                <div class="form-check">
                                    <label class="form-check-label">
                                        <input class="form-check-input" type="checkbox" value="Modal-${lastId}" ${isChecked === true ? 'checked' : ''}
                                             onclick="CheckBehaviour(${d.id})" />
                                        <span class="form-check-sign"><span class="check"></span></span>
                                    </label>
                                </div>
                            <input class="modal-id" type = "hidden" value = "${d.id}" />
                            </td>
                            <td id="code-${d.id}" class="modal-code">
                              ${d.code}
                            </td>
                            <td id="description-${d.id}" class="modal-description">
                                ${d.description}
                            </td>
                        </tr>`

        lastId++;
    }


    let counter = `${data.skippedRows + 1} - ${data.skippedRows + data.pageSize > data.totalCount ?
        data.totalCount : data.skippedRows + data.pageSize} ${commonDe}  ${data.totalCount}`;

    paginartionCountField.innerHTML = counter;

}

function AddPreventiveMeasures(e) {

    TempStoreCheckedValues();
    RenderTable(CheckedValues);
    CloseMeasureModal();
}

function CheckBehaviour(id) {
    let code = document.getElementById("code-" + id).innerText,
        description = document.getElementById("description-" + id).innerText;

    if (!CheckedValues.some(x => x.id == id)) {
        CheckedValues.push({
            id: id,
            code: code,
            description: description
        });
    } else {
        CheckedValues = CheckedValues.filter(x => x.id != id);
    }
}

function TempStoreCheckedValues() {
    let row,
        description,
        id,
        code;

    $('#preventive-measures-modal-table-body').find('input[type="checkbox"]:checked').each(function () {
        row = document.getElementById(this.value);
        description = row.getElementsByClassName('modal-description');
        id = row.getElementsByClassName('modal-id');
        code = row.getElementsByClassName('modal-code');

        if (!CheckedValues.some(x => x.id == id[0].value)) {
            CheckedValues.push({
                id: id[0].value,
                description: description[0].innerText,
                code: code[0].innerText
            });
        }



    });
}

function RenderTable(values) {
    let selectedElements = document.getElementById('preventive-measures-page-table-body');

    let i;

    if (selectedElements.lastElementChild != null) {
        i = parseInt(selectedElements.lastElementChild.id.substr(selectedElements.lastElementChild.id.indexOf("-") + 1)) + 1;

    } else {
        i = 0;
    }

    for (var value of values) {
        if (!selectedMeasuresCodes.some(selectedCode => selectedCode === value.code)) {
            selectedElements.innerHTML += PageTableTemplate(value, i);
            selectedMeasuresCodes.push(value.code);
            i++;
        }
    }

}

function PageTableTemplate(data, i) {
    return `<tr id="table-${i}" class="table-row preventiveMeasuresTable" data-href="segurplan_riesgosymedidas_medida.html">
                    <td>${data.code}
                   <input class="measures-table-id"   type="hidden" name="RisksAndPreventiveMeasure.PreventiveMeasures[${i}].PreventiveMeasureId" value="${data.id}" />
                   <input class="measures-table-code" type="hidden" name="RisksAndPreventiveMeasure.PreventiveMeasures[${i}].PreventiveMeasureCode" value="${data.code}" />
            </td>
            <td>${data.description}
                <input class="measures-table-description" type="hidden" name="RisksAndPreventiveMeasure.PreventiveMeasures[${i}].PreventiveMeasureDescription" value='${data.description}' />
            </td>
            <td>${i+1}
                   <input class="measures-table-order"   type="hidden" name="RisksAndPreventiveMeasure.PreventiveMeasures[${i}].PreventiveMeasureOrder" value="${i+1}" />
            </td>
                    <td class="text-center">
                        <a class="nav-link" href="#" id="navbarDropdownProfile2" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            <span class="fas fa-ellipsis-v"></span>

                        </a>
                        <div class="dropdown-menu dropdown-menu-right" aria-labelledby="navbarDropdownProfile2">
                            <a class="dropdown-item" onclick="DeletePageTableRow('table-${i}')" href="#">${commonDelete}</a>
                        </div>
                    </td>
                </tr>`
    //<a class="dropdown-item" onclick="DeletePageTableRow('table-${i}')" href="#">Eliminar</a>
}

function setAllChecked(divId, sourceCheckbox) {
    $('#' + divId).find(':checkbox').each(function () {
        if ($(this).is(':visible')) {
            $(this).prop('checked', $(sourceCheckbox).is(':checked'));
        }
    });
}


function CloseMeasureModal() {
    document.getElementById('measureModalClose').click();

}

function filterDropdowns(subDropdownName, parentId, currentElement) {
    var targetElement = currentElement.closest('.row').querySelector('[id^=' + subDropdownName + ']');

    //Si el valor es -1 es que ha selecccionado opcion default
    if (currentElement.value == "") {
        $(currentElement).closest('.row').find('*[id*=' + subDropdownName + ']')[0].disabled = true;
        //Si se a alterado el Dropdown subchapter tambien el de actividad
        if (subDropdownName.toUpperCase() === "SUBCAPITULO") {
            $(currentElement).closest('.row').find('*[id*=actividad]')[0].disabled = true;
        }
    }
    else {
        $(currentElement).closest('.row').find('*[id*=' + subDropdownName + ']')[0].disabled = false;
    }

    //Si se a alterado el Dropdown subchapter tambien el de actividad
    if (subDropdownName.toUpperCase() === "SUBCAPITULO") {
        $(currentElement).closest('.row').find('*[id*=actividad]').val(-1);
        $(currentElement).closest('.row').find('*[id*=actividad]')[0].disabled = true;
        $(currentElement).closest('.row').find('*[id*=actividad]').selectpicker("refresh");
    }

    for (var i = 0; i < targetElement.options.length; i++) {
        if (targetElement.options[i].dataset.belong !== parentId && targetElement.options[i].dataset.belong !== undefined) {
            $(targetElement.options[i]).hide();
        }
        else {
            $(targetElement.options[i]).show();
        }
    };
    $(currentElement).closest('.row').find('*[id*=' + subDropdownName + ']').val(-1);
    $(currentElement).closest('.row').find('*[id*=' + subDropdownName + ']').selectpicker("refresh");
}


function ChangeDependantData(selectedElementId, target, childDropdownId, childsChildsId) {
    let childDropdown = document.getElementById(childDropdownId);
    childDropdown.innerHTML = ""

    if (target === 'subChapter') {
        let childsSon = document.getElementById(childsChildsId);
        childsSon.innerHTML = '';
    }

    $.ajax({
        type: "GET",
        url: '/Models/RisksEvaluation/AllocationOfRisksAndPreventiveMeasures/Details/' + GetRiskAndPreventiveMeasuresId() + '/' + GetRiskAndPreventiveMeasuresEdit() + '?handler=DependantData',
        data: {
            __RequestVerificationToken: antiForgeryGen(),
            selectedId: selectedElementId,
            target: target
        },
        success:
            function (data) {
                //fill current dropdown
                switch (target) {
                    case "subChapter":
                        if (data.subChapter.length > 0) {
                            FillDropdown(childDropdown, data.subChapter);
                        }
                        break;
                    case "actividad":
                        if (data.activities.length > 0) {
                            FillDropdown(childDropdown, data.activities);
                        }
                        break;
                    case "subChapterVersion":
                        if (data.subChapterVersion.length > 0) {
                            FillDropdown(childDropdown, data.subChapterVersion);
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
    if (target.name.includes('SubChapter')) {
        for (var option of options) {
            target.innerHTML += `<option value="${option.idSubchapter}">${option.number} : ${option.title}</option>`;
        }
    }
    else {
        for (var option of options) {
            target.innerHTML += `<option value="${option.id}">${option.number} : ${option.title}</option>`;
        }
    }
}

function UpdateRiskLevel() {
    let seriousness = parseInt(document.getElementById('gravedad').value),
        probability = parseInt(document.getElementById('probabilidad').value);

    let selectedRiskLevel = riskLevelMatrixData.find(x => x.seriousnessId === seriousness && x.probabilityId == probability);
    if (selectedRiskLevel != undefined) {
        document.getElementById('nivel_riesgo_view').value = selectedRiskLevel.riskLevelLevel;
        document.getElementById('nivel_riesgo').value = selectedRiskLevel.riskLevelId;
    }
}