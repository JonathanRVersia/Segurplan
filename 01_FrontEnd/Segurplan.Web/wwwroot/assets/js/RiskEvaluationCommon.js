function SubmitCreateDocument(templateName) {
    document.getElementById("loading").style.visibility = "visible";
    let form = document.getElementById("create-document-form"),
        targetTamplateField = document.getElementById("targetTamplateField"),
        title = document.getElementById("titulo").value;
    $(document.body).css("pointer-events", 'none');
    targetTamplateField.value = templateName
    var data = [];
    for (var chapter of SelectedActivities) {
        for (var subChapter of chapter.subChapter) {
            for (var activities of subChapter.activities) {
                var chap = parseInt(chapter.id);
                var subchap = parseInt(subChapter.id)
                var act = parseInt(activities.id)
                data.push({ ChapterId: chap, SubChapterId: subchap, ActivityId: act })
            }
        }
    }
    if (data.length > 0) {
        $.ajax({
            type: "POST",
            url: "/RiskAssessmentsAndMaps?handler=CreateDocument",
            data: {
                SelectedData: data,
                TargetTemplate: templateName,
                Title: title
            },
            xhrFields: {
                responseType: 'blob'
            },
            success: function (result, status, xhr) {
                if (result !== undefined && result.size > 0) {

                    var filename = "";
                    var disposition = xhr.getResponseHeader('Content-Disposition')
                    if (disposition && disposition.indexOf('attachment') !== -1) {
                        var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/
                        var matches = filenameRegex.exec(disposition)
                        if (matches != null && matches[1]) {
                            filename = matches[1].replace(/['"]/g, '')
                        }
                    }
                    var link = document.createElement('a')
                    link.href = window.URL.createObjectURL(result)
                    link.download = filename
                    link.click()
                    window.URL.revokeObjectURL(result)
                    document.getElementById("loading").style.visibility = "hidden";
                    $(document.body).css("pointer-events","");

                }
            },
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            error: function (e) {
                $(document.body).css("pointer-events", "");
                document.getElementById("loading").style.visibility = "hidden";
            }
        })
    }
    else {
        $(document.body).css("pointer-events", "");
        document.getElementById("loading").style.visibility = "hidden";
    }
}
