var CheckedValues = [];

function DeletePageTableRow(rowId) {

    document.getElementById(rowId).remove();
    selectedArticlesCodes = [];
    let elements = document.getElementsByClassName('articlesTable'),
        valuesToTable = [];

    for (var element of elements) {
        valuesToTable.push({
            id: element.getElementsByClassName('article-table-id')[0].value,
            name: element.getElementsByClassName('article-table-name')[0].value,
            createdBy: element.getElementsByClassName('article-table-createdBy')[0].value,
            createDate: element.getElementsByClassName('article-table-createDate')[0].value
        });
    }

    document.getElementById('articles-page-table-body').innerHTML = "";
    RenderTable(valuesToTable);
}
function RenderTable(values) {
    let selectedElements = document.getElementById('articles-page-table-body');

    let i;

    if (selectedElements.lastElementChild != null) {
        i = parseInt(selectedElements.lastElementChild.id.substr(selectedElements.lastElementChild.id.indexOf("-") + 1)) + 1;

    } else {
        i = 0;
    }

    for (var value of values) {
        if (!selectedArticlesCodes.some(selectedCode => selectedCode === value.id)) {
            selectedElements.innerHTML += PageTableTemplate(value, i);
            selectedArticlesCodes.push(value.id);
            i++;
        }
    }

}
function PageTableTemplate(data, i) {
    return `<tr id="table-${i}" class="table-row articlesTable">
                <td>${data.id}
                    <input class="article-table-id" type="hidden" name="Task.TaskDetails[${i}].Article.Id" value="${data.id}" />
                </td>
                <td>${data.name}
                    <input class="article-table-name" type="hidden" name="Task.TaskDetails[${i}].Article.Name" value="${data.name}" />
                    <input class="article-table-createdBy" type="hidden" name="Task.TaskDetails[${i}].CreatedBy" value="${data.createdBy}" />
                    <input class="article-table-createDate" type="hidden" name="Task.TaskDetails[${i}].CreateDate" value="${data.createDate}" />
                </td>
                <td class="text-center">
                    <a class="nav-link" href="#" id="navbarDropdownProfile2" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        <span class="fas fa-ellipsis-v"></span>

                    </a>
                    <div class="dropdown-menu dropdown-menu-right" aria-labelledby="navbarDropdownProfile2">
                        <a class="dropdown-item" onclick="DeletePageTableRow('table-${i}')" href="#">Eliminar</a>
                    </div>
                </td>
            </tr>`;
}
function ChangeModalPage(direction, e) {

    let currentNumber = document.getElementById("articles-page-number"),
        totalCount = document.getElementById("articles-TotalCount"),
        pageSize = document.getElementById("articles-Rows-per-page");

    TempStoreCheckedValues();

    switch (direction) {
        case "next":
            if (parseInt(totalCount.value) / parseInt(pageSize.value) != parseInt(currentNumber.value)) {
                currentNumber.value = parseInt(currentNumber.value) + 1
                GetPage(currentNumber.value, GetInitialArticleNameValue());

            } else {
                e.preventDefault();
            }
            break;
        case "previous":
            if (parseInt(currentNumber.value) > 1) {
                currentNumber.value = parseInt(currentNumber.value) - 1;
                GetPage(currentNumber.value, GetInitialArticleNameValue());
            } else {
                e.preventDefault();
            }
            break;
    }
}
function TempStoreCheckedValues() {
    let row,
        id;

    $('#articles-modal-table-body').find('input[type="checkbox"]:checked').each(function () {
        row = document.getElementById(this.value);
        id = row.getElementsByClassName('modal-id');
        nameValue = row.getElementsByClassName('modal-name');

        if (!CheckedValues.some(x => x.id == id[0].value)) {
            CheckedValues.push({
                id: id[0].value,
                name: nameValue[0].innerText
            });
        }



    });
}
function GetInitialArticleNameValue() {
    return document.getElementById('articleName').value;
}
function GetPage(page, initialWord) {
    document.getElementById("loading").style.visibility = "visible";
    $.ajax({
        type: "POST",
        url: '/TaskManagement?currentOperation=Update&taskId=' + 1 + '&handler=ArticlesListPagination',
        data: {
            __RequestVerificationToken: antiForgeryGen(),
            nextPage: page,
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

    let tableField = document.getElementById('articles-modal-table-body'),
        paginartionCountField = document.getElementById('articles-modal-pagination-count');
    let lastId;

    if (tableField.lastElementChild != null) {
        lastId = parseInt(tableField.lastElementChild.id.substr(tableField.lastElementChild.id.lastIndexOf("-") + 1)) + 1;
    } else {
        lastId = 1;
    }

    tableField.innerHTML = "";
    paginartionCountField.innerHTML = "";


    let isChecked;
    for (var d of data.articles) {
        isChecked = CheckedValues.some(x => x.id == d.id);

        tableField.innerHTML +=
            `<tr id="Modal-${lastId}">
                <td class="text-center">
                    <div class="form-check">
                        <label class="form-check-label">
                            <input class="form-check-input" type="checkbox" value="Modal-${lastId}" ${isChecked === true ? 'checked' : ''}
                                onclick="CheckBehaviour('${d.id}')" />
                            <span class="form-check-sign">
                                <span class="check"></span>
                            </span>
                        </label>
                    </div>
                    <input class="modal-id" type="hidden" value="${d.id}" />
                </td>

                <td id="id-${d.id}" class="modal-id">
                    ${d.id}
                </td>
                <td id="name-${d.id}" class="modal-name">
                    ${d.name}
                </td>
            </tr>`

        lastId++;
    }


    let counter = `${data.skippedRows + 1} - ${data.skippedRows + data.pageSize > data.totalCount ?
        data.totalCount : data.skippedRows + data.pageSize} ${commonDe}  ${data.totalCount}`;

    paginartionCountField.innerHTML = counter;

}
function ModalSearchSubmit() {

    let initialWordInputValue = GetInitialArticleNameValue();

    GetPage(1, initialWordInputValue);

}
function CloseArticleModal() {
    document.getElementById('articleModalClose').click();

}
function AddArticle(e) {

    TempStoreCheckedValues();
    RenderTable(CheckedValues);
    CloseArticleModal();
}
function CheckBehaviour(id) {
    let currentId = document.getElementById("id-" + id).innerText,
        name = document.getElementById("name-" + id).innerText;


    if (!CheckedValues.some(x => x.id == id)) {
        CheckedValues.push({
            id: currentId,
            name: name
        });
    } else {
        CheckedValues = CheckedValues.filter(x => x.id != id);
    }
}
function setAllChecked(divId, sourceCheckbox) {
    $('#' + divId).find(':checkbox').each(function () {
        if ($(this).is(':visible')) {
            $(this).prop('checked', $(sourceCheckbox).is(':checked'));
        }
    });
}