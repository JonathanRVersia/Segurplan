//necesario definir realmente???                                                                      buscar
var AvailabeActivities = [];
var SelectedActivities = [];
var IsAsscending = true;
var SelectedIsAsscending = true;
var CanGenerateDoc
/*
 * get data
 */
window.onload = () => {
    fetch(GetIndexPage() + "?handler=Activities")
        .then(function (response) {
            return response.json();
        })
        .then(function (data) {
            MapJsonToObj(data);
        });
};

/**
 * Map data from component code behind get response
 * @param {any} data
 */
function MapJsonToObj(data) {
    AvailabeActivities = data.availableActivities;
    // SelectedActivities = data.planActivities;
    let filterData = GetLastFilterData();
    if (filterData.length != 0) {
        filterData = getSelectedFromAvailabe(filterData);
    }
    SelectedActivities =
        filterData.length != 0 ? filterData : data.planActivities;
    FillFilterDataHiddens(true);

    CheckSelectedSubchapters();
    RenderAvailabeList(AvailabeActivities);
    RenderSelectedList(SelectedActivities);
}

function CheckSelectedSubchapters() {
    SelectedActivities.filter((chapt) => {
        chapt.subChapter.filter((subChap) => {
            AvailabeActivities.find(el => el.id == chapt.id).subChapter.find(x => x.id == subChap.id).checked = true;
            return subChap;
            });
        return chapt;
    });
}

function getSelectedFromAvailabe(filterData) {
    let chapters = JSON.parse(JSON.stringify(AvailabeActivities));
    let filteredData = chapters.filter((x) =>
        filterData.some((y) => y.chapterId === x.id)
    );

    for (const chapter of filteredData) {
        chapter.subChapter = chapter.subChapter.filter((x) =>
            filterData.some((y) => y.subChapterId === x.id)
        );

        for (const subChapt of chapter.subChapter) {
            subChapt.activities = subChapt.activities.filter((x) =>
                filterData.some((y) => y.activityId === x.id)
            );
        }
    }

    let filteredDataOrdered = new Array();
    for (const chapter of filterData) {
        if (filteredDataOrdered.find(x => x.id == chapter.chapterId) == undefined) {
            filteredDataOrdered.push(filteredData.find(x => x.id == chapter.chapterId));
        }
    }

     return filteredDataOrdered;
}

function RenderAvailabeList(activities) {
    let availabeActivities = excludeSelectedItems(activities);

    let field = document.getElementById("availableList"),
        filledTemplate = "",
        chaptTemplate,
        subChaptTemplate,
        actTemplate,
        moveHidden = MoveHiddenValue()

    field.innerHTML = "";
    availActs = OrderBy(IsAsscending, availabeActivities);

    for (var chapter of availActs) {
        chaptTemplate = "";
        subChaptTemplate = "";
        actTemplate = "";

        chaptTemplate = `<li id="available-list-chapt${chapter.id}" class="accordion-firstlevel">
                            <div class="accordion-container">
                                <div class="activities-actions align-left">
                                    <div class="form-check">
                                        <label>
                                            <input onclick="setAllChecked('available-list-chapt${chapter.id}', this)" class="form-check-input" type="checkbox" name="chapterLeft" value="${chapter.id}">
                                                <span class="form-check-sign"><span class="check"></span></span>
                                        </label>
                                    </div>
                                </div>
                                <a data-toggle="collapse" href="#collapse_availabe_chap${chapter.id}" aria-expanded="false" aria-controls="collapse_availabe_chap${chapter.id}" class="accordion-toggle ${!moveHidden ? 'collapsed':""}">${chapter.number}.     ${chapter.title}</a>
                            </div>
                            <ol id="collapse_availabe_chap${chapter.id}" data-parent="available-list-chapt${chapter.id}" class="collapse">`;
        chapter.subChapter.sort(function (a, b) {
            return parseInt(a.number) - parseInt(b.number);
        });
        for (var subChapter of chapter.subChapter) {
            subChaptTemplate = `<li id="available-list-chapt${chapter.id}-subChapt${subChapter.id}" class="accordion-secondlevel">
                                    <div class="accordion-container">
                                        <div class="activities-actions align-left">
                                            <div class="form-check">
                                                <label>
                                                    <input onclick="setAllChecked('available-list-chapt${chapter.id}-subChapt${subChapter.id}', this)" class="form-check-input" type="checkbox" name="subChapterLeft" value="${subChapter.id}">
                                                    <span class="form-check-sign"><span class="check"></span></span>
                                                </label>
                                            </div>
                                        </div>
                                        <a data-toggle="collapse" href="#collapse_availabe_chapt${chapter.id}_subChapt${subChapter.id}" aria-expanded="false" aria-controls="collapse_availabe_chapt${chapter.id}_subChapt${subChapter.id}" class="accordion-toggle">${chapter.number}.${subChapter.number}.       ${subChapter.title}</a>
                                    </div>
                                    <ol id="collapse_availabe_chapt${chapter.id}_subChapt${subChapter.id}" data-parent="available-list-chapt${chapter.id}-subChapt${subChapter.id}" class="collapse">`;
            subChapter.activities.sort(function (a, b) {
                return parseInt(a.number) - parseInt(b.number);
            });
            for (var activities of subChapter.activities) {
                actTemplate = `<li class="accordion-container">
                                    <div class="activities-actions align-left">
                                        <div class="form-check">
                                            <label>
                                                <input class="form-check-input" type="checkbox" name="activitiesLeft" value="${activities.id}">
                                                <span class="form-check-sign"><span class="check"></span></span>
                                            </label>
                                        </div>
                                    </div>
                                    <span class="accordion-toggle">${chapter.number}.${subChapter.number}.${activities.number}.      ${activities.description}</span>
                                </li>`;

                subChaptTemplate += actTemplate;
            }
            subChaptTemplate += `</ol></li>`;
            chaptTemplate += subChaptTemplate;
        }
        chaptTemplate += `</ol></li>`;
        filledTemplate += chaptTemplate;
    }
    field.innerHTML += filledTemplate;
}

function RenderSelectedList(selectedActivities, isSetAdded = false) {
    let field = document.getElementById("selectedList"),
        filledTemplate = "",
        chaptTemplate,
        subChaptTemplate,
        actTemplate,
        selectedShortStatus = document.getElementById("selected-short-status"),
        moveHidden = MoveHiddenValue()

    field.innerHTML = "";
    selectedShortStatus.innerHTML = SelectedIsAsscending ? "Assc." : "Desc.";
    selActs = OrderBy(SelectedIsAsscending, selectedActivities);

    var GenDoc = document.getElementById('GenerateDoc')
    if (GenDoc != null )
        selectedActivities.length > 0 ? GenDoc.classList.remove('disabled') : GenDoc.classList.add('disabled');

    var chapterIndex = 0;
    for (var chapter of selActs) {
        chaptTemplate = "";
        subChaptTemplate = "";
        actTemplate = "";

        chapterIndex++;
        var subchapterIndex = 0;
        chaptTemplate = `<li id="selectedChapter${chapter.id}" class="accordion-firstlevel" ${!moveHidden ? 'onclick="changeSummernoteTarget(this,' + chapter.id : ""})">
                            <div class="accordion-container">
                                <div class="activities-actions align-left">
                                    <div class="form-check">
                                        <label>
                                            <input name="selectedCheck" onclick="setAllChecked('selectedChapter${chapter.id}', this)" 
                                                class="form-check-input" type="checkbox" value="selectedChapter${chapter.id},${chapter.id}">
                                                <span class="form-check-sign"><span class="check"></span></span>
                                                                        </label>
                                                                    </div>
                                    </div>
                                    <a data-toggle="collapse" href="#collapse_selected_chap${chapter.id
            }" aria-expanded="${!isSetAdded}" aria-controls="collapse_selected_chap${chapter.id
            }" class="accordion-toggle">${chapterIndex}.     ${chapter.title}</a>
                                    <div class="activities-actions align-right mr-3">`;
        if (!moveHidden) {
            chaptTemplate += `<button class="btn btn-just-icon btn-link btn-round btn-sm" onclick="Move(${chapter.id},'Up','chapter',event)"> <span class="fas fa-arrow-circle-up fa-2x"></span> </button>
                        <button class="btn btn-just-icon btn-link btn-round btn-sm" onclick="Move(${chapter.id},'Down','chapter',event)"> <span class="fas fa-arrow-circle-down fa-2x"></span> </button>
                                      `;
        }

        chaptTemplate += `<div class="dropdown">
                                            <button class="dropdown-toggle btn btn-just-icon btn-link btn-round btn-sm pl-2" type="button" id="dropdownMenuButton3" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"> <span class="fas fa-ellipsis-v"></span> </button>
                                            <div class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownMenuButton3" x-placement="bottom-start">
                                                <a class="dropdown-item" onclick="deleteEventHandler('selectedChapter${chapter.id}',${chapter.id})">${locationRemove}</a> </div>
                                        </div>
                                    </div>
                                </div>
                                <ol id="collapse_selected_chap${chapter.id}" data-parent="selectedChapter${chapter.id}" class="collapse ${isSetAdded?"":"show"}">`;

        //<button class="btn btn-just-icon btn-link btn-round"> <span class="fas fa-align-left"></span> </button>
        //<a class="dropdown-item" href="#">Editar</a>

        if (moveHidden) {
            chapter.subChapter.sort(function (a, b) {
                return parseInt(a.number) - parseInt(b.number);
            });
        }

        for (var subChapter of chapter.subChapter) {
            subchapterIndex++;
            var activityIndex = 0;
            subChaptTemplate = `<li id="selectedSubchapter${subChapter.id
                }" class="accordion-secondlevel" ${!moveHidden
                    ? 'onclick="changeSummernoteTarget(this,' +
                    chapter.id +
                    "," +
                    subChapter.id
                    : ""
                })"> 
                                    <div class="accordion-container">
                                        <div class="activities-actions align-left">
                                            <div class="form-check">
                                                <label>
                                                    <input name="selectedCheck" onclick="setAllChecked('collapse_selected_chapt${chapter.id}_subChap${subChapter.id
                }', this)" class="form-check-input" type="checkbox" value="selectedSubchapter${subChapter.id
                },${chapter.id},${subChapter.id}">
                                                        <span class="form-check-sign"><span class="check"></span></span>
                                                                            </label>
                                                                        </div>
                                            </div>
                                            <a data-toggle="collapse" href="#collapse_selected_chapt${chapter.id}_subChap${subChapter.id}" aria-expanded="false" aria-controls="collapse_selected_chapt${chapter.id}_subChap${subChapter.id}" class="accordion-toggle">${chapterIndex}.${subchapterIndex}.       ${subChapter.title}</a>
                                            <div class="activities-actions align-right mr-3">`;
            if (!moveHidden) {
                subChaptTemplate += `<button class="btn btn-just-icon btn-link btn-round btn-sm" onclick="Move(${subChapter.id},'Up','subChapter',event)"> <span class="fas fa-arrow-circle-up fa-2x"></span> </button>
                          <button class="btn btn-just-icon btn-link btn-round btn-sm" onclick="Move(${subChapter.id},'Down','subChapter',event)"> <span class="fas fa-arrow-circle-down fa-2x"></span> </button>
                                            `;
            }
            subChaptTemplate += `<div class="dropdown">
                                                    <button class="dropdown-toggle btn btn-just-icon btn-link btn-round btn-sm pl-2" type="button" id="dropdownMenuButton4" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"> <span class="fas fa-ellipsis-v"></span> </button>
                                                    <div class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownMenuButton4" x-placement="bottom-start"> 
                                                        <a class="dropdown-item" onclick="deleteEventHandler('selectedSubchapter${subChapter.id}',${chapter.id},${subChapter.id})">${locationRemove}</a>
                                                     </div>
                                                </div>
                                            </div>
                                        </div>
                                        <ol id="collapse_selected_chapt${chapter.id}_subChap${subChapter.id}" data-parent="selectedSubchapter${subChapter.id}" class="collapse ${isSetAdded ? "" : "show"}">`;

            //<button class="btn btn-just-icon btn-link btn-round"> <span class="fas fa-align-left"></span> </button>
            //<a class="dropdown-item" href="#">Editar</a>
            if (moveHidden) {
                subChapter.activities.sort(function (a, b) {
                    return parseInt(a.number) - parseInt(b.number);
                });
            }

            for (var activities of subChapter.activities) {
                activityIndex++;
                actTemplate = `<li id="selectedActivity${activities.id
                    }" class="accordion-container" ${!moveHidden
                        ? 'onclick="changeSummernoteTarget(this,' +
                        chapter.id +
                        "," +
                        subChapter.id +
                        "," +
                        activities.id
                        : ""
                    })"> 
                                       <div class="activities-actions align-left">
                                           <div class="form-check">
                                               <label>
                                                   <input name="selectedCheck" class="form-check-input" type="checkbox" value="selectedActivity${activities.id
                    },${chapter.id},${subChapter.id
                    },${activities.id}">
                                                       <span class="form-check-sign"><span class="check"></span></span>
                                                </label>
                                           </div>
                                       </div>
                                       <span class="accordion-toggle">${chapterIndex}.${subchapterIndex}.${activityIndex}.      ${activities.description}</span>
                                       <div class="activities-actions align-right mr-3">`;
                if (!moveHidden) {
                    actTemplate += `<button class="btn btn-just-icon btn-link btn-round btn-sm" onclick="Move(${activities.id},'Up','activities',event)"> <span class="fas fa-arrow-circle-up fa-2x"></span> </button>
                          <button class="btn btn-just-icon btn-link btn-round btn-sm" onclick="Move(${activities.id},'Down','activities',event)"> <span class="fas fa-arrow-circle-down fa-2x"></span> </button>`;
                }
                actTemplate += `<div class="dropdown">
                              <button class="dropdown-toggle btn btn-just-icon btn-link btn-round btn-sm pl-2" type="button" id="dropdownMenuButton5" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"> 
                                      <span class="fas fa-ellipsis-v"></span>
                                </button>
                              <div class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownMenuButton5" x-placement="bottom-start">
                                  <a class="dropdown-item" onclick="deleteEventHandler('selectedActivity${activities.id}',${chapter.id},${subChapter.id},${activities.id})">${locationRemove}</a> 
                              </div>
                          </div>
                      </div>
              </li>`;
                //<button class="btn btn-just-icon btn-link btn-round"> <span class="fas fa-align-left"></span> </button>
                //<a class="dropdown-item" href="#">Editar</a>

                subChaptTemplate += actTemplate;
            }
            subChaptTemplate += `</ol></li>`;
            chaptTemplate += subChaptTemplate;
        }
        chaptTemplate += `</ol></li>`;
        filledTemplate += chaptTemplate;
    }
    field.innerHTML += filledTemplate;
}

function excludeSelectedItems(availabeActs) {
    let chapters = JSON.parse(JSON.stringify(availabeActs)),
        filteredIdList = FatternSelectedIdList();

    if (SelectedActivities.length > 0) {
        chapters = chapters.filter((item) => {
            item.subChapter = item.subChapter.filter((subchaptItem) => {
                subchaptItem.activities = subchaptItem.activities.filter(
                    (actItem) =>
                        !filteredIdList.some((idRow) => actItem.id === idRow.activityId)
                );
                if (subchaptItem.activities.length != 0 || !subchaptItem.checked) {
                    return subchaptItem;
                }
            });
            if (item.subChapter.length != 0) {
                return item;
            }
        });
    }
    return chapters;
}

function FatternSelectedIdList() {
    let idList = [];

    for (const chapter of SelectedActivities) {
        for (const subCapter of chapter.subChapter) {

            if (subCapter.activities.length == 0) {
                idList.push({
                    chapterId: chapter.id,
                    subChapterId: subCapter.id,
                    activityId: 0,
                });
            } else {
                for (const activities of subCapter.activities) {
                    idList.push({
                        chapterId: chapter.id,
                        subChapterId: subCapter.id,
                        activityId: activities.id,
                    });
                }
            }
        }
    }

    return idList;
}

function RunFilter() {
    let searchForm = document.getElementById("search");
    FillFilterDataHiddens();
    searchForm.submit();
}

function FillFilterDataHiddens(isInitLoad) {
    isInitLoad = isInitLoad == undefined ? false : isInitLoad;

    let selectedIds = FatternSelectedIdList(),
        filterFields = document.getElementsByClassName("fiteredField"),
        formatedTemplate = ``;

    if (SelectedActivities.length > 0) {
        if (!isInitLoad) {
            for (const element of filterFields) {
                element.innerHTML = "";
            }
        }

        for (let i = 0; i < selectedIds.length; i++) {
            const element = selectedIds[i];
            formatedTemplate += `<tr>
                                    <td>
                                        <input type="number" name="FilterData[${i}].ChapterId" value="${element.chapterId}" />
                                    </td>
                                    <td>
                                        <input type="number" name="FilterData[${i}].SubChapterId" value="${element.subChapterId}" />
                                    </td>
                                    <td>
                                        <input type="number" name="FilterData[${i}].ActivityId" value="${element.activityId}" />
                                    </td>
                                 </tr>`;
        }

        for (const field of filterFields) {
            field.innerHTML += formatedTemplate;
        }
    } else {
        if (!isInitLoad) {
            for (const element of filterFields) {
                //element.remove();
                element.innerHTML = "";
            }
        }
    }
}

function ClearFilteredField() {
    let selectedIds = FatternSelectedIdList(),
        filterFields = document.getElementsByClassName("fiteredField"),
        formatedTemplate = ``;

    for (const field of filterFields) {
        field.remove();
    }

    for (let i = 0; i < selectedIds.length; i++) {
        const element = selectedIds[i];
        formatedTemplate += `<tr>
    <td>
    <input type="number" name="FilterData[${i}].ChapterId" value="${element.chapterId}" />
    </td>
    <td>
    <input type="number" name="FilterData[${i}].SubChapterId" value="${element.subChapterId}" />
    </td>
    <td>
    <input type="number" name="FilterData[${i}].ActivityId" value="${element.activityId}" />
    </td>
    </tr>`;
    }

    for (const field of filterFields) {
        field.innerHTML += formatedTemplate;
    }
}

/**
 * Filter handlers and timeout setter for search keyup handler
 * @param {any} searchKey
 * @param {any} list
 */
function FilterActivitiesEventHandler(searchKey, list) {
    let timeout = null;
    timeout = setTimeout(function () {
        if (list === "AvailabeActivities") {
            RenderAvailabeList(FilterGenericPlanList(searchKey, AvailabeActivities));
        } else if (list === "SelectedActivities") {
            RenderSelectedList(FilterGenericPlanList(searchKey, SelectedActivities));
        }
    }, 1000);
}

/**
 * Filter of activities (chapter and subchapter titles not contemplated)
 * @param {any} searchKey
 * @param {any} list
 */
function FilterGenericPlanList(searchKey, list) {
    let listToFilter = JSON.parse(JSON.stringify(list)),
        filteredSons,
        AnyItemContentContainsKey;

    if (searchKey != "") {
        filteredSons = listToFilter.filter((item) => {
            AnyItemContentContainsKey = false;

            item.subChapter = item.subChapter.filter((sub) =>
                sub.activities.some((act) =>
                    act.description.toLowerCase().includes(searchKey.toLowerCase())
                )
            );

            item.subChapter = item.subChapter.filter((subChapt) => {
                AnyItemContentContainsKey = true;

                subChapt.activities = subChapt.activities.filter((acts) =>
                    acts.description.toLowerCase().includes(searchKey.toLowerCase())
                );
                return subChapt;
            });

            if (AnyItemContentContainsKey) {
                return item;
            }
        });
    } else {
        filteredSons = JSON.parse(JSON.stringify(list));
    }

    return filteredSons;
}

/**
 * Order by behaviour, can handle order with filter applied
 * @param {any} isSelectedList
 */
function OrderByBehaviour(isSelectedList) {
    let availableSearchKey = document.getElementById("available-filter");
    let selectedSearchKey = document.getElementById("selected-filter");

    if (!isSelectedList) {
        IsAsscending = !IsAsscending;
        if (availableSearchKey.value === "") {
            RenderAvailabeList(AvailabeActivities);
        } else {
            RenderAvailabeList(
                FilterGenericPlanList(availableSearchKey.value, AvailabeActivities)
            );
        }
    } else {
        SelectedIsAsscending = !SelectedIsAsscending;
        if (selectedSearchKey.value === "") {
            RenderSelectedList(SelectedActivities);
        } else {
            RenderSelectedList(
                FilterGenericPlanList(selectedSearchKey.value, SelectedActivities)
            );
        }
    }
}

/**
 * Returns ordered object by ascending / descending
 * @param {any} isAscending
 * @param {any} elements
 */
function OrderBy(isAscending, elements) {
    let activities = JSON.parse(JSON.stringify(elements));
    let mapped = activities.map(function (el, i) {
        return { index: i, value: el };
    });

    mapped.sort(function (a, b) {
        //ascending
        if (isAscending) {
            return a.index - b.index;

            //descending
        } else {
            return b.index - a.index;
        }
    });

    let result = mapped.map(function (el) {
        return activities[el.index];
    });

    return result;
}

//renombrar a move event handler???
function Move(id, direction, level, e) {
    let elementToUp, listOfElements;

    switch (level) {
        case "chapter":
            listOfElements = SelectedActivities;
            elementToUp = SelectedActivities.find((x) => x.id === id);
            break;

        case "subChapter":
            listOfElements = SelectedActivities.find((x) =>
                x.subChapter.some((sub) => sub.id === id)
            ).subChapter;
            elementToUp = listOfElements.find((x) => x.id === id);
            break;

        case "activities":
            listOfElements = SelectedActivities.find((chapt) =>
                chapt.subChapter.some((sub) =>
                    sub.activities.some((act) => act.id === id)
                )
            ).subChapter.find((sub) => sub.activities.some((act) => act.id === id))
                .activities;
            elementToUp = listOfElements.find((act) => act.id === id);
            break;
    }

    MoveBehaviour(listOfElements, elementToUp, direction, e);
}

/**
 * Move behaviour of selected and availabe activities
 * @param {any} list
 * @param {any} elementToUp
 * @param {any} direction
 * @param {any} e
 */
function MoveBehaviour(list, elementToUp, direction, e) {
    let elementToDown,
        error = false;

    if (direction === "Up") {
        if (list.indexOf(elementToUp) === 0) {
            error = true;
        } else {
            elementToDown = list[list.indexOf(elementToUp) - 1];
            RunMove(list, elementToUp, elementToDown);
        }
    } else {
        if (list.indexOf(elementToUp) === list.length - 1) {
            error = true;
        } else {
            elementToDown = list[list.indexOf(elementToUp) + 1];
            RunMove(list, elementToDown, elementToUp);
        }
    }

    if (!error) {
        RenderSelectedList(SelectedActivities);
    } else {
        e.preventDefault();
    }
}

//si no se usa en mas sitios deberia ir dentro de MoveBehaviour??
function RunMove(list, first, second) {
    list[list.indexOf(first)] = second;
    list[list.indexOf(second)] = first;
}

//refactorizar!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
function SetAdded() {
    let value, chapCopy, dataToInsert, activitySubChaptToInsert;

    //move by chapter
    $.each($("input[name='chapterLeft']:checked"), function () {
        this.checked = false;
        value = parseInt(this.value);

        if (SelectedActivities.some((chapt) => chapt.id === value)) {
            SelectedActivities.splice(
                SelectedActivities.findIndex((x) => x.id === value), SelectedActivities.filter(chapter => chapter.id === value).length
            );
        }

        var subchapterList = AvailabeActivities.find(x => x.id == value).subChapter;
        for (var item in subchapterList) {
            AvailabeActivities.find(el => el.id == value).subChapter.find(x => x.id == subchapterList[item].id).checked = true;
        }
        let insertData = AvailabeActivities.find((el) => el.id === value);


        insertData.subChapter.sort(function (a, b) {
               return parseInt(a.number) - parseInt(b.number);
        });
        for (var itemSubchapter of insertData.subChapter) {            
            itemSubchapter.activities.sort(function (a, b) {
                return parseInt(a.number) - parseInt(b.number);
            });
        }
        SelectedActivities.push(
            JSON.parse(
                JSON.stringify(insertData)
            )
        );
    });

    //move by subChapter
    $.each($("input[name='subChapterLeft']:checked"), function () {
        this.checked = false;
        value = parseInt(this.value);

        if (
            !SelectedActivities.some((chapt) =>
                chapt.subChapter.some((subChapt) => subChapt.id === value)
                )
            ) {
            chapCopy = JSON.parse(
                JSON.stringify(
                    AvailabeActivities.find((el) =>
                        el.subChapter.some((sub) => sub.id === value)
                    )
                )
            );
            AvailabeActivities.find((el) => el.subChapter.some((sub) => sub.id === value))
                .subChapter.find((sub) => sub.id === value).checked = true;
            dataToInsert = chapCopy.subChapter.find((sub) => sub.id === value);


            dataToInsert.activities.sort(function (a, b) {
               return parseInt(a.number) - parseInt(b.number);
            });

            if (!SelectedActivities.some((chapt) => chapt.id === chapCopy.id)) {
                chapCopy.subChapter = [];
                chapCopy.subChapter.push(dataToInsert);
                SelectedActivities.push(JSON.parse(JSON.stringify(chapCopy)));
            } else {
                SelectedActivities.find((el) => el.id === chapCopy.id).subChapter.push(
                    dataToInsert
                );
            }
        }
    });

    //move by activity
    $.each($("input[name='activitiesLeft']:checked"), function () {
        this.checked = false;
        value = parseInt(this.value);

        //esta la actividad?
        if (
            !SelectedActivities.some((chapt) =>
                chapt.subChapter.some((subChapt) =>
                    subChapt.activities.some((act) => act.id === value)
                )
            )
        ) {
            chapCopy = JSON.parse(
                JSON.stringify(
                    AvailabeActivities.find((el) =>
                        el.subChapter.some((sub) =>
                            sub.activities.some((act) => act.id === value)
                        )
                    )
                )
            );
            dataToInsert = chapCopy.subChapter
                .find((sub) => sub.activities.some((act) => act.id === value))
                .activities.find((act) => act.id === value);
            activitySubChaptToInsert = chapCopy.subChapter.find((subChap) =>
                subChap.activities.some((act) => act.id === value)
            );
            if (
                !SelectedActivities.some((chapt) =>
                    chapt.subChapter.some(
                        (subChapt) => subChapt.id === activitySubChaptToInsert.id
                    )
                )
            ) {
                activitySubChaptToInsert.activities = [];
                activitySubChaptToInsert.activities.push(dataToInsert);

                if (!SelectedActivities.some((chapt) => chapt.id === chapCopy.id)) {
                    chapCopy.subChapter = [];
                    chapCopy.subChapter.push(activitySubChaptToInsert);
                    SelectedActivities.push(chapCopy);
                } else {
                    //es necesario?
                    SelectedActivities.find(
                        (chapt) => chapt.id === chapCopy.id
                    ).subChapter.push(activitySubChaptToInsert);
                }
            } else {
                SelectedActivities.find((chapt) => chapt.id === chapCopy.id)
                    .subChapter.find((sub) => sub.id === activitySubChaptToInsert.id)
                    .activities.push(dataToInsert);
                if (AvailabeActivities.find((chapt) => chapt.id == chapCopy.id)
                    .subChapter.find((sub) => sub.id === activitySubChaptToInsert.id).activities.length ==
                    SelectedActivities.find((chapt) => chapt.id === chapCopy.id)
                        .subChapter.find((sub) => sub.id === activitySubChaptToInsert.id).activities.length)
                {
                    AvailabeActivities.find((chapt) => chapt.id == chapCopy.id)
                        .subChapter.find((sub) => sub.id === activitySubChaptToInsert.id).checked = true;
                }
            }
        }
    });

    document.getElementById("Set-all-available-btn").checked = false;
    //RenderAvailabeList(AvailabePlanes);
    RenderSelectedList(SelectedActivities, true);
    RenderAvailabeList(AvailabeActivities);
}

/**
 * Set all elemets checked
 * @param {any} divId
 * @param {any} sourceCheckbox
 */
function setAllChecked(divId, sourceCheckbox) {
    $("#" + divId)
        .find(":checkbox")
        .each(function () {
            if ($(this).is(":visible")) {
                $(this).prop("checked", $(sourceCheckbox).is(":checked"));
            }
        });
}

///Delete tambien de la lista???
function deleteEventHandler(id, chapterId, subChapterId, activityId) {
    let element = document.getElementById(id);
    if (element != null) {
        element.remove();
        //fill delete from selected activities

        //Delete by activity
        if (
            chapterId != undefined &&
            subChapterId != undefined &&
            activityId != undefined
        ) {
            SelectedActivities.filter((chapt) => {
                if (chapt.id == chapterId) {
                    chapt.subChapter.filter((subChap) => {
                        if (subChap.id == subChapterId) {
                            subChap.activities.splice(
                                subChap.activities.findIndex((x) => x.id == activityId),
                                1
                            );
                        }
                        if (subChap.activities.length == 0) {
                            AvailabeActivities.find(el => el.id == chapterId).subChapter.find(x => x.id == subChapterId).checked = false;
                        }
                        return subChap;
                    });
                    if (chapt.subChapter.length == 0) {
                        SelectedActivities.splice(
                            SelectedActivities.findIndex((x) => x.id == chapterId),
                            1
                        );
                    }
                }
                return chapt;
            });
        }

        //Delete by subchapter
        if (
            chapterId != undefined &&
            subChapterId != undefined &&
            activityId === undefined
        ) {
            AvailabeActivities.find(el => el.id == chapterId).subChapter.find(x => x.id == subChapterId).checked = false;
            SelectedActivities.filter((chapt) => {
                if (chapt.id == chapterId) {
                    chapt.subChapter.splice(
                        chapt.subChapter.findIndex((x) => x.id == subChapterId),
                        1
                    );
                }
                if (chapt.subChapter.length == 0) {
                    SelectedActivities.splice(
                        SelectedActivities.findIndex((x) => x.id == chapterId),
                        1
                    );
                }

                return chapt;
            });
        }

        //Delete by chapter
        if (chapterId != undefined && subChapterId === undefined) {
            var subchapterList = SelectedActivities.find(x => x.id == chapterId).subChapter;
            for (var item in subchapterList) {
                AvailabeActivities.find(el => el.id == chapterId).subChapter.find(x => x.id == subchapterList[item].id).checked = false;
            }
            SelectedActivities.splice(
                SelectedActivities.findIndex((x) => x.id == chapterId),
                1
            );
        }
    }
    //ClearFilteredField();
    RenderAvailabeList(AvailabeActivities);
    RenderSelectedList(SelectedActivities,true);
}

/**
 * Delete checked selected elements
 */
function DeleteByCheckBox() {
    let properties;
    $.each($("input[name='selectedCheck']:checked"), function () {
        properties = this.value.split(",");

        deleteEventHandler(
            properties[0],
            properties[1],
            properties[2],
            properties[3]
        );
    });
    document.getElementById("Set-all-selected-btn").checked = false;
}

let objTarget;
let target;
let times = 0;
let clearAt;
let sumerText;

/**
 * change summernote target
 * @param {any} element
 * @param {any} chapterId
 * @param {any} subChapterId
 * @param {any} activityId
 */
function changeSummernoteTarget(element, chapterId, subChapterId, activityId) {
//    times += 1;

//    if (times === 1) {
//        if (target != undefined) {
//            target.style.border = "";
//        }
//        target = element;
//        target.style.border = "1px solid #004f86";
//        setCurrentStoredValues(
//            sumerText,
//            SetObjTarget(chapterId, subChapterId, activityId)
//        );
//    }

//    if (element.id.includes("selectedActivity") && times === 1) {
//        clearAt = 3;
//    } else if (element.id.includes("selectedSubchapter") && times === 1) {
//        clearAt = 2;
//    } else if (element.id.includes("selectedChapter") && times === 1) {
//        clearAt = 1;
//    }

//    if (clearAt === times) {
//        times = 0;
//    }
}

/**
 * set target , returns description
 * @param {any} chapterId
 * @param {any} subChapterId
 * @param {any} activityId
 */
//function SetObjTarget(chapterId, subChapterId, activityId) {
//    if (activityId != undefined) {
//        objTarget = SelectedActivities.find((chapt) => chapt.id === chapterId)
//            .subChapter.find((sub) => sub.id === subChapterId)
//            .activities.find((act) => act.id === activityId);
//    } else if (subChapterId != undefined) {
//        objTarget = SelectedActivities.find(
//            (chapt) => chapt.id === chapterId
//        ).subChapter.find((sub) => sub.id === subChapterId);
//    } else {
//        objTarget = SelectedActivities.find((chapt) => chapt.id === chapterId);
//    }

//    return objTarget.wordDescription;
//}

/**
 * Set current description value on view
 * @param {any} dest
 * @param {any} value
 */
function setCurrentStoredValues(dest, value) {
    //dest.innerHTML = '';
    dest.value = "";
    //dest.innerHTML = value;
    dest.value = value;
}

/**
 * Set current value to obj array
 */
function getTextValues() {
    objTarget.wordDescription = sumerText.value;
}

//(function () {
//    if (!GetMoveHiddenValue()) {
//        setSumernoteEvent();
//    }

//    function setSumernoteEvent() {
//        //en caso de que se cambie por sumernote
//        //let summerBox = document.getElementById('summernoteBox');
//        //sumerText = summerBox.nextElementSibling.getElementsByClassName("note-editable card-block")[0];
//        sumerText = document.getElementById("summernoteBox");

//        sumerText.addEventListener("input", function () {
//            getTextValues();
//        });
//    }
//})();
