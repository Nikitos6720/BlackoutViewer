// #region Import Form
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
        success: SuccessfulImport,
        error: ErrorImport
    });
}

function SuccessfulImport(response) {
    var importForm = document.getElementById("data-from-excel-form");

    alert("Successful uploading");
    importForm.close();
}

function ErrorImport(response) {
    alert("Error of data formatting");
}
// #endregion



// #region Select blackouts
function LoadAddresses() {
    var groupSelect = document.getElementById("group-select");
    var selectedGroup = groupSelect.value;

    if (selectedGroup === "0") {
        alert("Please, select group.");
        return;
    }

    $.ajax({
        url: "/BlackoutList/SelectAddressesByGroup",
        type: "Get",
        data: { groupId: selectedGroup },
        success: SuccessfulSelectAddresses,
        error: ErrorSelectAddresses
    });
}

function ShowOneDay() {
    var oneDayList = document.getElementById("one-day-list");
}

function ShowOneWeek() {
    var oneWeekList = document.getElementById("one-week-list");
}
// #endregion



// #region Download Json File
function DownloadJsonFile() {
    var groupSelect = document.getElementById("group-select");
    var selectedGroup = groupSelect.value;
    window.location.href = "/BlackoutList/DownloadJsonFile?groupId=" + selectedGroup;
}
// #endregion