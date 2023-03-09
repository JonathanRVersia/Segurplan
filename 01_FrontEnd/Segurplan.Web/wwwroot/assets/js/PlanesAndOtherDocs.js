var AvailabePlanes = [];
var SelectedFamily = [];
var IsAsscending = true;

/**
 * Map data from code behind get response
 * @param {any} data
 */
function MapJsonToObj(data) {
    AvailabePlanes = data.availabePlanes;
    SelectedFamily = data.selectedPlanes;

    RenderAvailabeList(AvailabePlanes);
    RenderSelectedList(SelectedFamily);
}

/**
 * Render available list except selected elements
 * @param {any} availabePlanes
 */
function RenderAvailabeList(availabePlanes) {
    let planes = excludeSelectedItems(availabePlanes);
    planes = OrderBy(IsAsscending, planes);
    let filledTemplate = "";

    let planosGenericosContainer = document.getElementById("planos-genericos");

    ClearElement(planosGenericosContainer);
    filledTemplate += `<ol>`;

    for (var plan of planes) {
        filledTemplate += GetFamilyTemplateStart(plan);

        for (var planeListItem of plan.planeList) {
            filledTemplate += GetPlaneTemplate(planeListItem, plan.familyId);
        }
        filledTemplate += GetFamilyTemplateEnd();
    }

    filledTemplate += `</ol>`;

    planosGenericosContainer.innerHTML += filledTemplate;
}

/**
 * Render selected plans ,medir tiempos de respuesta
 * @param {any} selectedPlanes
 */
function RenderSelectedList(selectedPlanes) {
    let planTemplate = [];
    let container = document.getElementById("tabla-planos-añadidos-container");
    ClearElement(container);
    let i = 0;
    for (var plan of selectedPlanes) {
        planTemplate.push(GetAddedPlaneTemplate(plan, i));
        i++;
    }

    container.innerHTML += planTemplate.join("");
}

/**
 * Apply selected filter and unbind list from general list
 * @param {any} availabePlanes
 */
function excludeSelectedItems(availabePlanes) {
    let planes = JSON.parse(JSON.stringify(availabePlanes));
    //apply exclude selected to method
    if (SelectedFamily.length > 0) {
        planes = planes.filter((item) => {
            item.planeList = item.planeList.filter(
                (planItem) =>
                    !SelectedFamily.some(
                        (pl) => pl.id === planItem.id || pl.idPlane === planItem.id
                    )
            );
            if (item.planeList.length != 0) {
                return item;
            }
        });
    }
    return planes;
}

//añadir keyup timeout?
function FilterAvailabePlanesEventHandler(searchKey) {
    FilterGenericPlanList(searchKey);
    //add keyup timeout
    //let timeout = null;
    //timeout = setTimeout(
    //    FilterGenericPlanList(searchKey)
    //    , 1000);
}

/**
 * Filter by plans (families are not being filtered)
 * Caution with dates
 * @param {any} searchKey
 */
function FilterGenericPlanList(searchKey) {
    //con fechas puede cascar el mapeo cuidado
    let listToFilter = JSON.parse(JSON.stringify(AvailabePlanes));

    let filteredSons;

    if (searchKey != "") {
        filteredSons = listToFilter.filter((item) => {
            //isKeyOnFamilyName = item.familyName.toLowerCase().includes(searchKey.toLowerCase());

            item.planeList = item.planeList.filter((x) =>
                x.name.toLowerCase().includes(searchKey.toLowerCase())
            );

            if (item.planeList.length != 0) {
                return item;
            }

            //if (isKeyOnFamilyName) {
            //    return item;
            //}
        });
    } else {
        filteredSons = JSON.parse(JSON.stringify(AvailabePlanes));
    }

    RenderAvailabeList(filteredSons);
}

/**
 * Add element to selected
 * */
function SetAdded() {
    let values, selectedFamily;

    $.each($("input[name='generic-blueprints']:checked"), function () {
        values = this.value.split(",");
        //selectedFamily = AvailabePlanes.find(el => el.familyId === parseInt(values[0]));
        selectedFamily = JSON.parse(
            JSON.stringify(
                AvailabePlanes.find((el) => el.familyId === parseInt(values[0]))
            )
        );
        selectedFamily.planeList.forEach(function (obj) {
            obj.familyName = selectedFamily.familyName;
        });

        if (values.length === 1) {
            //filtro already added
            let filtered = selectedFamily.planeList.filter(
                (planItem) => !SelectedFamily.some((pl) => pl.idPlane === planItem.id)
            );
            SelectedFamily = SelectedFamily.concat(filtered);
        } else {
            if (!SelectedFamily.some((pl) => pl.id === parseInt(values[1]))) {
                SelectedFamily.push(
                    selectedFamily.planeList.find((pl) => pl.id === parseInt(values[1]))
                );
            }
        }
    });

    $("#planes-available-checkbox").prop("checked", false);

    RenderAvailabeList(AvailabePlanes);
    RenderSelectedList(SelectedFamily);
}

//Mockup ???
function AddNewPlane(e) {
    e.preventDefault();
    document.getElementById(
        "tabla-planos-añadidos-container"
    ).innerHTML += GetAddedPlaneTemplate();
}

/**
 * Move/up down function
 * @param {any} id
 * @param {any} direction
 * @param {any} e
 */
function Move(id, direction, e) {
    let elementToUp = SelectedFamily.find((x) => x.id === id);
    let elementToDown;
    let error = false;

    if (direction === "Up") {
        if (SelectedFamily.indexOf(elementToUp) === 0) {
            error = true;
        } else {
            elementToDown = SelectedFamily[SelectedFamily.indexOf(elementToUp) - 1];
            MoveBehaviour(elementToUp, elementToDown);
            //SelectedPlanes[SelectedPlanes.indexOf(elementToUp)] = elementToDown;
            //SelectedPlanes[SelectedPlanes.indexOf(elementToDown)] = elementToUp;
        }
    } else {
        if (SelectedFamily.indexOf(elementToUp) === SelectedFamily.length - 1) {
            error = true;
        } else {
            elementToDown = SelectedFamily[SelectedFamily.indexOf(elementToUp) + 1];
            MoveBehaviour(elementToDown, elementToUp);
            //SelectedPlanes[SelectedPlanes.indexOf(elementToDown)] = elementToUp;
            //SelectedPlanes[SelectedPlanes.indexOf(elementToUp)] = elementToDown;
        }
    }

    if (!error) {
        RenderSelectedList(SelectedFamily);
    } else {
        e.preventDefault();
    }
}

/**
 * Move Behaviour, switch first with second
 * @param {any} first
 * @param {any} second
 */
function MoveBehaviour(first, second) {
    SelectedFamily[SelectedFamily.indexOf(first)] = second;
    SelectedFamily[SelectedFamily.indexOf(second)] = first;
}

/**
 * Delete by checkbox from selected list
 */
function deleteChecked() {
    let element;

    $.each($("input[name='added-blueprints']:checked"), function () {
        //element = document.getElementById(parseInt(this.value));
        deletePlaneBehaviour(parseInt(this.value));
    });

    $("#planes-selected-checkbox").prop("checked", false);

    RenderAvailabeList(AvailabePlanes);
    RenderSelectedList(SelectedFamily);
}

/**
 * remove element from lists
 * @param {any} element
 */
function deletePlaneBehaviour(id) {
    SelectedFamily.splice(
        SelectedFamily.findIndex((x) => x.id == id),
        1
    );
}

/**
 *  Delete from selected and add to available
 * @param {any} e
 * @param {any} element
 */
function deletePlane(e, id) {
    deletePlaneBehaviour(id);

    RenderAvailabeList(AvailabePlanes);
    RenderSelectedList(SelectedFamily);
}

/**
 * Order by btn actions
 */
function OrderByBehaviour() {
    IsAsscending = !IsAsscending;
    let searchKey = document.getElementById("search-box");

    if (searchKey.value === "") {
        RenderAvailabeList(AvailabePlanes);
    } else {
        FilterGenericPlanList(searchKey.value);
    }
}

/**
 * Returns ordered object by ascending / descending
 * @param {any} isAscending
 * @param {any} elements
 */
function OrderBy(isAscending, elements) {
    let planes = JSON.parse(JSON.stringify(elements));
    let mapped = planes.map(function (el, i) {
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
        return planes[el.index];
    });

    return result;
}

/**
 * remove elements inside element
 * @param {any} element
 */
function ClearElement(element) {
    element.innerHTML = "";
}

///rehacer para mejorar tiempos de carga
function setAllChecked(divId, sourceCheckbox) {
    $("#" + divId)
        .find(":checkbox")
        .each(function () {
            if ($(this).is(":visible")) {
                $(this).prop("checked", $(sourceCheckbox).is(":checked"));
            }
        });
}

/**
 * Create Plan behaviour
 * */
function UploadPlanPlane(e) {
    e.preventDefault();
    let planId = GetPlanId();
    let descripcionField = document.getElementById("descripcion");
    let descripcion = descripcionField.value;
    let familia = document.getElementById("familia").value;
    if (descripcion == "") {
        document.getElementById("blueprintDescriptionError").hidden = false;
        return;
    }
    document.getElementById("blueprintDescriptionError").hidden = true;
    let position =
        SelectedFamily.length > 0
            ? SelectedFamily[SelectedFamily.length - 1].position + 1
            : 1;
    let fileInput = document.getElementById("new-plane-file");

    let files = fileInput.files;

    if (!ValidateFileCollectionTypes(files)) {
        errorInvalidImage();
        return;
    }

    let formData = new FormData();

    for (var file of files) {
        formData.append("files", file);
    }

    formData.append("planId", planId);
    formData.append("description", descripcion);
    formData.append("family", familia);
    formData.append("position", position);
    formData.append("__RequestVerificationToken", antiForgeryGen());

    $.ajax({
        //NOTE:En la url está añadido el IsEdit porque si no no llama correctamente al Handler
        //Se piensa que esta llamando a Activities.cshtml porque esta tiene una ruta definida page "/planManagement/{handler?}/{planId}/{isEditEnabled}"
        url: "/planManagement/AddBlueprint/" + GetPlanId() + "/true",
        data: formData,
        type: "POST",
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.statusCode == undefined || data.statusCode != 404) {
                //clear modal
                descripcionField.value = "";
                document.getElementById("new-plane-fileDZFileNames").innerHTML = "";
                document.getElementById("new-plane-file").value = "";
                //new-plane-fileDZFileNames
                //new-plane-file

                SelectedFamily.push(data);
                RenderSelectedList(SelectedFamily);
            }
        },
    });

    document.getElementById("addNewBlueprintModalClose").click();
}

function ValidateFileCollectionTypes(files) {
    const acceptedFileTypes = ['image/jpg', 'image/jpeg', 'image/png', 'image/tif', 'image/tiff', 'image/bmp', 'image/gif'];
    for (var file of files) {
        if (!acceptedFileTypes.some(type => type == file.type)) {
            return false;
        }
    }

    return true;
}

/**
 * Get plane file and preview if get success
 * @param {any} planeId
 * @param {any} e
 */
function GetFilePreview(planeId, e, isFromGenericBlueprint) {
    e.preventDefault();

    $.ajax({
        //NOTE:En la url está añadido el IsEdit porque si no no llama correctamente al Handler
        //Se piensa que esta llamando a Activities.cshtml porque esta tiene una ruta definida page "/planManagement/{handler?}/{planId}/{isEditEnabled}"
        url: "/planManagement/GetBlueprint/" + GetPlanId() + "/true",
        data: {
            __RequestVerificationToken: antiForgeryGen(),
            planeId: planeId,
            isFromGenericBlueprint: isFromGenericBlueprint,
        },
        type: "GET",
        success: function (data) {

            if (data.statusCode == undefined || data.statusCode != 404) {
                OpenFileView(data);
            }
        },
    });
}

/**
 * Open and fill blueprint modal
 * @param {any} data
 */
function OpenFileView(data) {
    //get modal
    let modal = document.getElementById("modal-blueprint");
    let indicator = document.getElementById("blueprints-modal-indicator");
    let body = document.getElementById("blueprints-modal-body");

    //fill modal
    body.innerHTML = "";
    indicator.innerHTML = "";
    let index;
    for (var file of data) {
        index = data.indexOf(file);
        indicator.innerHTML += `<li data-target="#carouselExampleIndicators" data-slide-to="${index}" ${index === 0 ? "class='active'" : ""
            }></li>`;

        body.innerHTML += `<div class="carousel-item ${index === 0 ? "active" : ""
            }">
                                <div class="carousel-caption d-none d-md-block">
                                    <h5>${file.name}</h5>
                                </div>
                              <img class="d-block" src="${"data:image/" +
            file.fileName.substr(
                file.fileName.lastIndexOf(".") + 1
            ) +
            ";base64," +
            file.data
            }" alt="">
                            </div>`;
    }

    //open modal
    if (data.length != 0) {
        modal.classList.add("show");
        modal.style.display = "block";
        modal.ariaHidden = "false";
    }
}

/**
 * Close modal behaviour
 * @param {any} e
 */
function CloseModal(e, id) {
    e.preventDefault();
    let modal = document.getElementById(id);
    modal.classList.remove("show");
    modal.style.display = "none";
    modal.ariaHidden = "true";
}

/**
 * Generic plan list element template part 1
 * @param {any} plan
 */
function GetFamilyTemplateStart(plan) {
    return `  
        <li class="accordion-firstlevel" style=" list-style-type: none;">
            <div class="accordion-container">
                <div class="activities-actions align-left">
                    <div class="form-check">
                        <label>
                          <input class="form-check-input" type="checkbox" name="generic-blueprints" value="${plan.familyId}">
                              <span class="form-check-sign"><span class="check"></span></span>
                        </label>
                   </div>
                    </div>
                    <a data-toggle="collapse" href="#collapseTrabajo${plan.familyId === 3 ? "--3" : plan.familyId}" 
                        aria-expanded="true" aria-controls="collapseTrabajo4" class="accordion-toggle">
                        ${plan.familyName}
                    </a>
                    <div class="activities-actions align-right mr-2"></div>
               </div>
           <ol id="collapseTrabajo${plan.familyId === 3 ? "--3" : plan.familyId}"  data-parent=".accordion-firstlevel" class="collapse show">

                      `;

    //< button class="btn btn-just-icon btn-link btn-round" onclick = "GetFilePreview(${plan.planeList.map(a => a.id)},event)" >
    //    <i class="fas fa-image"></i>
    //    <div class="ripple-container"></div>
    //                </button >
}

/**
 *  Generic plan list element template part 2
 */
function GetFamilyTemplateEnd() {
    return ` </ol>
         </li>
    `;
}

/**
 * Plan list template
 * @param {any} planeListItem
 * @param {any} familyId
 */
function GetPlaneTemplate(planeListItem, familyId) {
    //<button class="btn btn-just-icon btn-link btn-round">

    let template = `<li class="accordion-secondlevel">
        <div class="accordion-container">
            <div class="activities-actions align-left">
                <div class="form-check">
                        <label>
                              <input class="form-check-input" type="checkbox" name="generic-blueprints" value="${familyId},${planeListItem.id}">
                              <span class="form-check-sign"><span class="check"></span></span>
                        </label>
                </div>
             </div>
             <span class="accordion-toggle">${planeListItem.name}</span>
             <div class="activities-actions align-right mr-2">`;
    if (planeListItem.hasFile) {

        template += `<button class="btn btn-just-icon btn-link btn-round" type="button" onclick = "GetFilePreview(${planeListItem.id},event,true)" >
                    <i class="fas fa-image"></i>
                        <div class="ripple-container"></div>
                 </button>`;
    }
    template += `</div>
        </div>
    </li>`;
    return template;
    //
    //< button class="btn btn-just-icon btn-link btn-round" type = "button" data - toggle="modal" data - target="#modal2" >
    //    <i class="fas fa-image"></i>
    //    <div class="ripple-container"></div>
    //        </button >
}

/**
 * Added plan tamplate
 * @param {any} plan
 * @param {any} i
 */
function GetAddedPlaneTemplate(plan, i) {
    //<button class="btn btn-just-icon btn-link btn-round btn-sm">
    //<button class="btn btn-just-icon btn-link btn-round" type="button" data-toggle="modal" data-target="#modal2">
    //<i class="fas fa-image"></i>
    //   <div class="ripple-container"></div>
    //           </button >

    let number = SelectedFamily.indexOf(plan) + 1;

    let template = `<tr id="addedPlan${plan.id}">
                <td class="text-left">
                    <div class="form-check">
                        <label>
                            <input class="form-check-input" type="checkbox" name="added-blueprints" value="">
                            <span class="form-check-sign"><span class="check"></span></span>
                            <input type="hidden" name="SelectedPlanes[${i}].Id" value="${plan.id}" />
                        </label>
                    </div>
                </td>
                <td class="text-center">
                    <input type="hidden" name="SelectedPlanes[${i}].Position" value="${number}" />
                    <input type="hidden" name="SelectedPlanes[${i}].IdPlane" value="${plan.idPlane === undefined ? plan.id : plan.idPlane}" />
                </td>
                <td class="text-left articulo">${plan.name === undefined ? plan.description : plan.name}
                    <input name="SelectedPlanes[${i}].Description" value="${plan.name === undefined ? plan.description : plan.name}" />
                </td>
                <td class="text-left">${plan.familyName}
                    <input name="SelectedPlanes[${i}].FamilyName" value="${plan.familyName}" />
                    <input name="SelectedPlanes[${i}].isAvailable" value="${plan.isAvailable}" />
                    <input name="SelectedPlanes[${i}].containsFile" value="${plan.hasFile === undefined ? plan.containsFile : plan.hasFile}" />
                </td>
                <td class="text-center">`;
    if (plan.hasFile || plan.containsFile) {
        template += `<button class="btn btn-just-icon btn-link btn-round" type="button" onclick="GetFilePreview(${plan.id},event,${plan.isAvailable})">
                    <i class="fas fa-image"></i>
                    <div class="ripple-container"></div>
                </button>`;
    }
    template += `</td>`;

    if (editEnabled) {
        template += ` <td class="text-center">
                            <button class="btn btn-just-icon btn-link btn-round btn-sm"onclick="Move(${plan.id},'Up',event)">
                                <i class="fas fa-angle-up"></i>
                                <div class="ripple-container"></div>
                            </button>
                            <button class="btn btn-just-icon btn-link btn-round btn-sm"onclick="Move(${plan.id},'Down',event)"> 
                                <i class="fas fa-angle-down"></i> 
                            </button>
                       </td>
                        <td class="text-right">
                                <div class="dropdown ml-1">
                                    <button class="dropdown-toggle btn btn-just-icon btn-link btn-round btn-sm pl-2" type="button"
                                            id="dropdownMenuButton21" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                        <span class="fas fa-ellipsis-v"></span> </button>
                                    <div class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownMenuButton21" x-placement="bottom-start">
                                        <a class="dropdown-item" onclick="deletePlane(event,${plan.id})">${locations["Planes.OpcionTable.Eliminar"]}</a></div>
                                </div>
                            </td>`;
    }
    template += `</tr>`;
    return template;
}
