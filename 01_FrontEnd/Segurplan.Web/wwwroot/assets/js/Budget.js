var AvailabeTasks = [];
var ArticlesByFamily = [];
var SelectedTasks = [];
var SelectedArticlesDB = [];
let ArticlesAvailable = [];
let ArticlesUnitAndPriceSave = [];
var IsAscending = true;
var FromPercentage = false;
let WorkersNumberValue = document.getElementById("maximo_trabajadores").value;
let DurationWorkValue = document.getElementById("plazo_ejecucion_dias").value;
let PresupuestoEjecucionValue = document.getElementById("presupuesto_PSS").value;
let LoadedOnce = false;
let SelectArticlesBtnBool = true;
var ApplicablePercentage = 100;
var previousPercentaje = 0;

function MapJsonBudgetToObj(data) {

    AvailabeTasks = data.articlesByTask;

    ArticlesByFamily = data.articlesByFamily;

    SelectedArticlesDB = data.selectedArticlesDB;

    ArticlesAvailable = []
    for (var task of AvailabeTasks) {
        for (var article of task.articles) {
            if (!ArticlesAvailable.some((pl) => pl.id === article.id)) {
                ArticlesAvailable.push(article);
            }
        }
    }

    ArticlesUnitAndPriceSave = [];
    for (var article of ArticlesAvailable) {
        var articleUnitPrice = {
            id: article.id,
            price: article.price,
            totalPrice: article.totalPrice,
            unit: article.unit
        };
        ArticlesUnitAndPriceSave.push(articleUnitPrice);
    }

    var percentage = document.getElementById("porcentaje_aplicable");
    if (LoadedOnce == false) {
        ApplicablePercentage = data.applicablePercentage;
        percentage.value = ApplicablePercentage;
    } else {
        percentage.value = 100;
    }

    previousPercentaje = percentage.value;

    RenderAvailabeTasksList(AvailabeTasks);
    RenderSelectedTasksList(SelectedTasks);
    if (LoadedOnce == false && SelectedArticlesDB.length > 0) {
        CheckDBSelectedArticles();
        SetAdd();
        SelectedArticlesDB = [];
    }
    LoadedOnce = true;
}

/** 
 * Render available list except selected elements
 * @param {any} availabeTasks
 */
function CheckDBSelectedArticles() {
    var estimatedValueList = [];
    for (var article of SelectedArticlesDB) {
        var estimatedValue = `${article.id},${article.idArticleFamily}`;
        estimatedValueList.push(estimatedValue);
    }
    var inputCheckValues = [];
    $.each($("input[name='generic-blueprintss']"), function () {
        if (this.value.includes(",")) {
            var inputCheckValue = this.value.split(",")[1] + "," + this.value.split(",")[2];
            if ($.inArray(inputCheckValue, estimatedValueList) > -1) {
                this.checked = true;
                inputCheckValues.push(inputCheckValue);
            }
        }
    });

}
function RenderAvailabeTasksList(availabeTasks) {
    let filteredTasks = excludeSelectedArticles(availabeTasks);
    if (SelectArticlesBtnBool == false) {
        filteredTasks = availabeTasks;
    }
    let filledTemplate = "";

    let availableArticlesContainer = document.getElementById("available-articles");

    ClearBudgetElement(availableArticlesContainer);
    filledTemplate += `<ol>`;

    for (var task of filteredTasks) {
        filledTemplate += GetTaskTemplateStart(task);

        for (var article of task.articles) {
            filledTemplate += GetBudgetTemplate(article, task.id);
        }
        filledTemplate += GetFamilyTemplateEnd();
    }

    filledTemplate += `</ol>`;

    availableArticlesContainer.innerHTML += filledTemplate;
}
/**
 * Render selected plans ,medir tiempos de respuesta
 * @param {any} selectedTasks
 */
function RenderSelectedTasksList(selectedTasks, keepCustomUnits) {
    let taskTemplate = [];
    let container = document.getElementById("tabla-articulos-añadidos-container");
    let total = 0;
    var percentage = document.getElementById("porcentaje_aplicable").value;
    ClearBudgetElement(container);
    let i = 0;
    for (var task of selectedTasks) {
        var updatedArticle = ArticlesAvailable.find((x) => x.id == task.id);
        if (keepCustomUnits != true) {
            task.priceDurationWork = updatedArticle.priceDurationWork;
            task.totalPrice = updatedArticle.totalPrice;
            task.unit = updatedArticle.unit;
        }
        total += task.totalPrice;
        taskTemplate.push(GetAddedTaskTemplate(task, i));
        i++;
    }
    var difference = CalculateDifference(total);
    $("#totalBudgetPrice").prop("innerText", total.toFixed(2) + " €");
    if (selectedTasks.length == 0) {
        container.innerHTML = `<tr>
                                    <td class="text-center"></td>
                                    <td class="text-center"></td>
                                    <td class="text-center">
                                         <div class="form-check" style="color: dimgrey; font-style: italic;">
                                            No hay articulos actualmente
                                         </div>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td class="text-right"></td>
                                    <td class="text-center"></td>
                              </tr>`;
    }

    let selectArticlesBtn = document.getElementById("select_articles_btn");
    if (SelectArticlesBtnBool == false) {
        if (selectArticlesBtn != null) {
            selectArticlesBtn.style.color = "#f3bc8c";
            selectArticlesBtn.style.cursor = "default";
            selectArticlesBtn.removeAttribute("onclick");
        }
        container.innerHTML = `<tr>
                                    <td class="text-center"></td>
                                    <td class="text-center"></td>
                                    <td colspan="4" class="text-center">
                                         <div class="form-check" style="color: red; font-family: Open Sans; font-weight: bold; cursor: default;">
                                                                                        Se requieren los campos Presupuesto de ejecución, plazo de ejecución(días) y total de trabajadores.
                                         </div>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td class="text-right"></td>
                                    <td class="text-center"></td>
                              </tr>`;
        $("#totalBudgetPrice").prop("innerText", "0.00 €");
        taskTemplate = null;
        LoadedOnce = true;
    }
    else {
        if (selectArticlesBtn != null) {
            selectArticlesBtn.style.color = "#ee750b";
            selectArticlesBtn.style.cursor = "pointer";
            selectArticlesBtn.setAttribute("onclick", "SetAdd()");
        }
    }

    var valuesBool = true;
    if (WorkersNumberValue <= 0 && DurationWorkValue <= 0 && PresupuestoEjecucionValue <= 0) {
        valuesBool = false;
    }

    if (FromPercentage == false) {
        DesactivateSaveAndGenerateBtn();

        manageArticleSide(SelectedTasks, percentage, valuesBool);
        if (SelectArticlesBtnBool == false) {
            ActivateSaveAndGenerateBtn();
        }
    }

    if (taskTemplate != null) {
        container.innerHTML += taskTemplate.join("");
    }
}
/**
 * remove elements inside element
 * @param {any} element
 */
function ClearBudgetElement(element) {
    element.innerHTML = "";
}

function UsePercentage() {
    FromPercentage = true;

    var percentage = document.getElementById("porcentaje_aplicable").value;
    if (percentage < 1) {
        percentage = 1;
        document.getElementById("porcentaje_aplicable").value = 1;
    }
    var count = $('input[name="elementUnitInput"]').length;
    $('input[name="elementPriceDurationWorkInput"]').each(function () {
        var currentPriceDurationWork = this.value;
        this.value = (percentage * currentPriceDurationWork) / previousPercentaje;
        count--;
        if (count <= 0) {
            FromPercentage = false;
        }
        this.onchange();
    });
    previousPercentaje = percentage;
}
/**
 * Generic article list element template part 1
 * @param {any} article
 */
function GetTaskFamilyTemplateStart(article) {
    return `  
        <li class="accordion-firstlevel" style=" list-style-type: none; display: block;">
            <div class="accordion-container">
                <div class="activities-actions align-left">
                    <div class="form-check">
                        <label>
                          <input class="form-check-input" type="checkbox" name="generic-blueprintss" value="${article.Id}">
                              <span class="form-check-sign"><span class="check"></span></span>
                        </label>
                    </div>
                </div>
                    <a data-toggle="collapse" href="#collapseTrabajo${article.Id === 3 ? "--3" : article.Id}" 
                        aria-expanded="true" aria-controls="collapseTrabajo4" class="accordion-toggle">
                        ${article.Id}
                    </a>
                    <div class="activities-actions align-right mr-2"></div>
             </div>
           <ol id="collapseTrabajo${article.Id === 3 ? "--3" : article.Id}"  data-parent=".accordion-firstlevel" class="collapse show">`;
}
function GetBudgetTemplate(article, idTask) {

    let template = `<li id="AvailableArticles" class="accordion-secondlevel" style="display: block;" name='ArticleId_${article.id}'>
        <div class="accordion-container">
            <div class="activities-actions align-left">
                <div class="form-check">
                        <label>
                              <input class="form-check-input" type="checkbox" name="generic-blueprintss" value="${idTask},${article.id},${article.idArticleFamily}" onclick="SetParentCheckOff('task_${idTask === 3 ? "--3" : idTask}_check', this)">
                              <span class="form-check-sign"><span class="check"></span></span>
                        </label>
                </div>
             </div>
             <span class="accordion-toggle">${article.name}</span>
             <div class="activities-actions align-right mr-2">`;
    template += `</div>
        </div>
    </li>`;
    return template;
}
function excludeSelectedBudgetItems(availabeTasks) {
    let chapters = JSON.parse(JSON.stringify(availabeTasks)),
        filteredIdList = FatternSelectedIdList();

    if (SelectedActivities.length > 0) {
        chapters = chapters.filter((item) => {
            item.subChapter = item.subChapter.filter((subchaptItem) => {
                subchaptItem.activities = subchaptItem.activities.filter(
                    (actItem) =>
                        !filteredIdList.some((idRow) => actItem.id === idRow.activityId)
                );
                if (subchaptItem.activities.length != 0) {
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
function GetTaskTemplateStart(task) {
    return `  
        <li id="task-item-${task.id}" class="accordion-firstlevel" style=" list-style-type: none; display: block;">
            <div class="accordion-container">
                <div class="activities-actions align-left">
                    <div class="form-check">
                        <label>
                          <input id="task_${task.id === 3 ? "--3" : task.id}_check" class="form-check-input" type="checkbox" name="generic-blueprintss" value="${task.id}" onclick="SetAllCheckedBudget('collapseTrabajo${task.id === 3 ? "--3" : task.id}', this)">
                              <span class="form-check-sign"><span class="check"></span></span>
                        </label>
                   </div>
                    </div>
                    <a data-toggle="collapse" href="#collapseTrabajo${task.id === 3 ? "--3" : task.id}" 
                        aria-expanded="false" aria-controls="collapseTrabajo4" class="accordion-toggle">
                        ${task.name}
                    </a>
                    <div class="activities-actions align-right mr-2"></div>
               </div>
           <ol id="collapseTrabajo${task.id === 3 ? "--3" : task.id}"  data-parent="task-item-${task.id}" class="collapse"> `;
}
/**
 * Added plan tamplate
 * @param {any} task
 * @param {any} i
 */
function GetAddedTaskTemplate(task, i) {
    let number = SelectedTasks.indexOf(task) + 1;

    let template = `<tr id="addedTask${task.id}">
                <td class="text-left">
                    <div class="form-check">
                        <label>
                            <input class="form-check-input" type="checkbox" name="added-blueprints" value="${task.id}">
                            <span class="form-check-sign"><span class="check"></span></span>
                        </label>
                    </div>
                </td>
                <td class="text-left articulo">${task.name}</td>
                <td class="text-left articulo">${ArticlesByFamily.find((x) => x.id == task.idArticleFamily).family}</td>
                <td class="text-center"><input style="width: 45px" type="number" class="ud" id="" name="elementUnitInput" role="textbox" value="${task.unit}" onchange="CalculateNewTotalPrice(this, ${task.priceDurationWork.toFixed(2)}, ${task.id})"></td>
                <td class="text-center"><input style="width: 70px" type="number" class="ud" id="" name="elementPriceDurationWorkInput" role="textbox" value="${task.priceDurationWork.toFixed(2)}" onchange="CalculateNewTotalPriceByPrice(this, ${task.unit}, ${task.id})"></td>
                <td colspan = "2">${task.totalPrice.toFixed(2)} €</td>
                <td class="text-right">
                    <div class="dropdown ml-1">
                        <button class="dropdown-toggle btn btn-just-icon btn-link btn-round btn-sm pl-2" type="button" id="dropdownMenuButton15" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"> <span class="fas fa-ellipsis-v"></span> </button>
                        <div class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownMenuButton15" x-placement="bottom-start"> 
                            <a class="dropdown-item" onclick="deleteArticle(event,${task.id})" " href="#">Borrar</a> </div>
                    </div>
                </td>
                <td class="text-center">`;
    template += `</td>`;

    template += `</tr>`;
    return template;
}
/**
 * Add element to selected
 * */
function SetAdd() {
    let values, selectedArticle;
    document.getElementById("select-all-articles-check").checked = false;
    /*TODO: Actualizar name de imput con check*/
    $.each($("input[name='generic-blueprintss']:checked"), function () {

        values = this.value.split(",");

        if (values.length === 1) {

            //Se obtienen los articulos x tarea
            var articlesByTask = AvailabeTasks.find((task) => task.id === parseInt(values[0]));

            articlesByTask.articles.forEach(function (obj) {
                obj.familyName = GetFamily(obj.idArticleFamily).family;
                if (!SelectedTasks.some((pl) => pl.id === obj.id)) {
                    var articleElement = document.getElementsByName(`ArticleId_${obj.id}`)[0].style.display;
                    if (articleElement != 'none') {
                        var originalArticle = ArticlesUnitAndPriceSave.find((task) => task.id == obj.id);
                        obj.unit = originalArticle.unit;
                        obj.price = originalArticle.price;
                        obj.totalPrice = originalArticle.totalPrice;
                        SelectedTasks.push(obj);
                    }
                }
            });

        } else {

            selectedArticle = JSON.parse(JSON.stringify(GetArticle(values)));

            selectedArticle.familyName = GetFamily(selectedArticle.idArticleFamily).family;

            var dbValues = false;
            if (SelectedArticlesDB.length > 0) {
                var selectedArticleDB = SelectedArticlesDB.find((x) => x.id == selectedArticle.id);
                if (typeof selectedArticleDB != "undefined") {
                    selectedArticle.unit = selectedArticleDB.unit;
                    selectedArticle.priceDurationWork = selectedArticleDB.priceDurationWork;
                    selectedArticle.totalPrice = selectedArticleDB.totalPrice;
                }
                dbValues = true;
            }

            if (!SelectedTasks.some((pl) => pl.id === parseInt(values[1]))) {
                if (dbValues == false) {
                    var originalArticle = ArticlesUnitAndPriceSave.find((task) => task.id == selectedArticle.id);
                    selectedArticle.unit = originalArticle.unit;
                    selectedArticle.price = originalArticle.price;
                    selectedArticle.totalPrice = originalArticle.totalPrice;
                }
                SelectedTasks.push(selectedArticle);
            }
        }
    });

    var keepCustomUnits = true;
    $('#inputAvailableBudgetFilterList').keyup();
    SelectedArticlesDB = [];
    RenderSelectedTasksList(SelectedTasks, keepCustomUnits);
}
function GetArticle(values) {
    return AvailabeTasks.find((task) => task.id === parseInt(values[0])).articles.find((article) => article.id === parseInt(values[1]));;;
}
function GetFamily(familyId) {
    return ArticlesByFamily.find((family) => family.id === familyId);
}
function excludeSelectedArticles(availabeTasks) {
    let articles = JSON.parse(JSON.stringify(availabeTasks));
    //apply exclude selected to method
    if (SelectedTasks.length > 0) {
        articles = articles.filter((taskItem) => {
            taskItem.articles = taskItem.articles.filter(
                (articleItem) =>
                    !SelectedTasks.some(
                        (pl) => pl.id === articleItem.id
                    )
            );
            if (taskItem.articles.length != 0) {
                return taskItem;
            }
        });
    }
    return articles;
}
/**
 *  Delete from selected and add to available
 * @param {any} e
 * @param {any} element
 */
function deleteArticle(e, id) {
    deleteArticleBehaviour(id);

    var keepCustomUnits = true;
    $('#inputAvailableBudgetFilterList').keyup();
    RenderSelectedTasksList(SelectedTasks, keepCustomUnits);
}
/**
 * remove element from lists
 * @param {any} element
 */
function deleteArticleBehaviour(id) {
    SelectedTasks.splice(
        SelectedTasks.findIndex((x) => x.id == id),
        1
    );
    SelectedArticlesDB.splice(
        SelectedArticlesDB.findIndex((x) => x.id == id),
        1
    );
}
/**
 * Delete by checkbox from selected list
 */
function deleteCheckedAll() {

    $.each($("input[name='added-blueprints']:checked"), function () {
        deleteArticleBehaviour(parseInt(this.value));
    });

    $("#articles-selected-checkbox").prop("checked", false);

    var keepCustomUnits = true;
    $('#inputAvailableBudgetFilterList').keyup();
    RenderSelectedTasksList(SelectedTasks, keepCustomUnits);
}

function FilterBudgetList(tagToSearch, divId) {
    RenderAvailabeTasksList(AvailabeTasks);
    let filter, firstLevelBox;

    filter = tagToSearch.value.toUpperCase();
    firstLevelBox = document.getElementById(divId);

    for (let task of firstLevelBox.children[0].children) {
        var articleCount = task.children[1].children.length;

        for (let article of task.children[1].children) {
            if (!article.children[0].children[1].innerHTML.toUpperCase().includes(filter)) {
                article.style.display = 'none';
                article.style.visibility = 'hidden';
                articleCount--;
            }
        }

        if (articleCount <= 0) {
            task.style.display = 'none';
            task.style.visibility = 'hidden';
        }
    }
}

function SetAllCheckedBudget(divId, sourceCheckbox) {
    $("#" + divId)
        .find(":checkbox")
        .each(function () {
            if ($(this).is(":visible")) {
                $(this).prop("checked", $(sourceCheckbox).is(":checked"));
            }
        });
}

function SetParentCheckOff(taskCheckElementId, element) {

    if (element.checked == false) {
        document.getElementById(taskCheckElementId).checked = false;
    }

}

function UpdatePresupuestoValue() {
    let needToGetBudget = false;
    SelectArticlesBtnBool = false;

    if (WorkersNumberValue != document.getElementById("maximo_trabajadores").value) {
        WorkersNumberValue = document.getElementById("maximo_trabajadores").value;
        document.getElementById('presupuesto_num_trabajadores').innerHTML = WorkersNumberValue;
        needToGetBudget = true;
    }

    if (DurationWorkValue != document.getElementById("plazo_ejecucion_dias").value) {
        DurationWorkValue = document.getElementById("plazo_ejecucion_dias").value;
        document.getElementById('presupuesto_plazo_dias').innerHTML = DurationWorkValue;
        needToGetBudget = true;
    }

    if (PresupuestoEjecucionValue != document.getElementById("presupuesto_PSS").value) {
        PresupuestoEjecucionValue = document.getElementById("presupuesto_PSS").value;
        document.getElementById('presupuesto_PSS_presupuesto').innerHTML = PresupuestoEjecucionValue;
        var totalPrice = document.getElementById('totalBudgetPrice').innerText;
        totalPrice = totalPrice.replace(" €", "");
        CalculateDifference(totalPrice);
        needToGetBudget = true;
    }


    if (WorkersNumberValue >= 1 && DurationWorkValue >= 1 && PresupuestoEjecucionValue >=1) {
        SelectArticlesBtnBool = true;
    }

    if (needToGetBudget == true || LoadedOnce == false) {
        let container = document.getElementById("tabla-articulos-añadidos-container");
        container.innerHTML = `<tr>
                                    <td class="text-center"></td>
                                    <td class="text-center"></td>
                                    <td class="text-center">
                                         <div class="form-check" style="color: dimgrey; font-style: italic;">
                                            Actualizando valores...
                                         </div>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td class="text-right"></td>
                                    <td class="text-center"></td>
                              </tr>`;
        $("#totalBudgetPrice").prop("innerText", "0.00 €");
        DesactivateSaveAndGenerateBtn();
        ActivateLoading(true);
        fillTab("Budget", DurationWorkValue, WorkersNumberValue);
    }

}

function CalculateNewTotalPrice(element, priceDurationWork, taskId) {
    var unit = element.value * 1;
    if (unit < 0) {
        unit = 0;
        element.value = 0;
    }
    var newTotalPrice = unit * priceDurationWork;

    var totalPriceElement = element.parentElement.parentElement.children[5];
    totalPriceElement.innerHTML = newTotalPrice.toFixed(2) + " €";

    var updatedSelectedArticle = SelectedTasks.find((x) => x.id == taskId);
    updatedSelectedArticle.unit = unit;
    updatedSelectedArticle.totalPrice = newTotalPrice;

    var total = 0;
    for (var task of SelectedTasks) {
        total += task.totalPrice;
    }

    CalculateDifference(total)
    $("#totalBudgetPrice").prop("innerText", total.toFixed(2) + " €");
    RenderSelectedTasksList(SelectedTasks, true);
}

function CalculateNewTotalPriceByPrice(element, unit, taskId) {
    var price = element.value * 1;
    if (price < 0) {
        price = 0;
        element.value = 0;
    }
    var newTotalPrice = price * unit;

    var totalPriceElement = element.parentElement.parentElement.children[5];
    totalPriceElement.innerHTML = newTotalPrice.toFixed(2) + " €";

    var updatedSelectedArticle = SelectedTasks.find((x) => x.id == taskId);
    updatedSelectedArticle.priceDurationWork = price;
    updatedSelectedArticle.totalPrice = newTotalPrice;

    var percentage = document.getElementById("porcentaje_aplicable").value;

    var total = 0;
    for (var task of SelectedTasks) {
        total += task.totalPrice;
    }

    CalculateDifference(total)
    $("#totalBudgetPrice").prop("innerText", total.toFixed(2) + " €");
    manageArticleSide(SelectedTasks, percentage, true);
}

function CalculateDifference(totalPrice) {
    var presupuestoEjecucion = document.getElementById("presupuesto_PSS_presupuesto").innerText;
    var difference = presupuestoEjecucion - totalPrice;
    $("#diferencia_value").prop("innerText", difference.toFixed(2) + " €");
    return difference.toFixed(2);
}

function ActivateLoading(activate) {
    if (activate) {
        document.getElementById("loading").style.visibility = "visible";

        for (var i = 0; i < document.getElementsByClassName("option").length; i++) {
            document.getElementsByClassName("option")[i].style.pointerEvents = "none";
        }
    } else {
        document.getElementById("loading").style.visibility = "hidden";

        for (var i = 0; i < document.getElementsByClassName("option").length; i++) {
            document.getElementsByClassName("option")[i].style.pointerEvents = "";
        }
    }
}

function ValidateUnitZero() {
    var result = SelectedTasks.find((x) => x.unit == 0);

    if (result !== undefined) {
        document.getElementById("#unitZero").style.visibility = "initial";
        document.getElementById("#unitZero").style.display = "block";
        DesactivateSaveAndGenerateBtn();

    } else {
        document.getElementById("#unitZero").style.visibility = "hidden";
        document.getElementById("#unitZero").style.display = "none";
        ActivateSaveAndGenerateBtn();
    }
}
function OpenCloseArticlesBehaviour(spinnerId, buttonId, fieldId) {
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

    runOpenCloseArticles(fieldId, currentAction).then(v => {
        spinner.hidden = true;
        toggleBtn.hidden = false;
    });

}

async function runOpenCloseArticles(fieldId, currentAction) {

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
        await sleepArticles(8);
    }

}

function sleepArticles(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}