function OpenImportForm() {
    var importForm = document.getElementById("data-from-excel-form");
    importForm.showModal();
}

function CloseImportForm() {
    var importForm = document.getElementById("data-from-excel-form");
    importForm.close();
}

function UploadData(event) {
    var file = event.target.files[0]; // Получаем файл

    if (!file) {
        alert("Please, select file.");
        return;
    }

    var formData = new FormData();
    formData.append("file", file);

    $.ajax({
        url: "/Schedules/Upload",
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            alert("Successful uploading");
        },
        error: function (response) {
            alert("Error of data formatting");
        }
    });
}