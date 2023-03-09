const removeExistingAnagramInput = document.getElementById("RemoveExisitngAnagramFile");
//const removeFileIdsInput = document.getElementById("RemoveFileId");
const dt = new DataTransfer();

//function fillRemoveDBFileIdsInput(id) {
//    if (removeFileIdsInput.value === "") {
//        removeFileIdsInput.value = id;
//    } else {
//        removeFileIdsInput.value += "," + id;
//    }
//}

function removeExistingAnagramFile(fileId) {
    if (removeExistingAnagramInput.value === "") {
        removeExistingAnagramInput.value = fileId;
    } else {
        removeExistingAnagramInput.value += "," + fileId;
   }
}

function removeFileFromInput(inputId, fileName) {
    dt.clearData();

    var inputFile = document.getElementById(inputId);

    for (var i = 0; i < inputFile.files.length; i++) {
        if (inputFile.files[i].name !== fileName) {
            dt.items.add(inputFile.files[i]);
        }
    }

    updateInputFiles(inputFile);
}

function addFileDetailsToList(file, inputId, removePreviousItems) {
    var ul = document.getElementById(inputId + 'DZFileNames');

    if (removePreviousItems === true) {
        ul.innerHTML = null;
    }

    var fileName = file.name;
    var fileSize = bytesToSize(file.size);
    var fileType = file.name.split('.').slice(-1)[0];

    ul.innerHTML += ('<li class="name">' + '<span class="badge badge-pill">' + fileType + '</span> ' + '<a class="filename">' + fileName + '</a>' + '<span class="filesize">(' + fileSize + ')</span> <button class="btn remove" type="button" onclick="removeFileFromInput(' + "'" + inputId + "','" + fileName + "'" + ')"><i class="fas fa-times"></i></button>' + '</li>');

}

function bytesToSize(bytes) {
    var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    if (bytes == 0) return '0 Byte';
    var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
    return Math.round(bytes / Math.pow(1024, i), 2) + ' ' + sizes[i];
}

function addFileToInput(input) {
    if (input.multiple === false && input.files.length === 1) {
        if (checkFileExtension(input.files[0], input.accept) === true) {
            dt.clearData();
            dt.items.add(input.files[0]);
            addFileDetailsToList(input.files[0], input.id, true);
        }
    } else if (input.multiple === true) {
        for (let file of input.files) {
            dt.items.add(file);
            addFileDetailsToList(file, input.id, false);
        }
    }


    updateInputFiles(input);
}

function checkFileExtension(file, acceptedTypes) {
    if ((acceptedTypes.includes(file.type) && file.type != '') || acceptedTypes.includes(file.name.split('.')[1])) {
        return true;
    } else {
        return false;
    }
}

function onClickFile(input) {
    //dt.clearData();

    //for (let file of input.files) {
    //    dt.items.add(file);
    //}
}

function updateInputFiles(input) {
    input.value = '';

    input.files = dt.files;
}

function onInputFile(input) {

    //dt.items = input.files;
    addFileToInput(input);
}

function onDropFile(e, inputId) {

    //console.log('Evento dropzone disparado!');
    e.preventDefault();

    // Getting dropzone control
    dz = document.getElementById(inputId);
    dz.files = e.dataTransfer.files;

    addFileToInput(dz);
}

function onDragOverFile(e) {
    e.preventDefault();
    return false;
}

function onDragLeave() {
    return false;
}

