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
    var file = event.target.files[0];

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
    response.forEach(AppendToList);
    importForm.close();
    window.location.reload();
}

function AppendToList(item) {
    var body = document.getElementById("schedule-table-body");
    body.innerHTML = "";

    var tr = $("<tr></tr>");
    tr.append("<td>" + item.day + "</td>");
    tr.append("<td>" + item.startTime + "</td>");
    tr.append("<td>" + item.endTime + "</td>");
    tr.append("<td>" + "" + "</td>");
    body.append(tr);
}

function ErrorImport(response) {
    alert("Error of data formatting");
}
// #endregion



// #region Select blackouts
function ShowBlackoutInfo() {
    var groupSelect = document.getElementById("group-select");
    var selectedGroup = groupSelect.value;

    $.ajax({
        url: "/BlackoutList/GetGroup",
        type: "Get",
        data: { groupId: selectedGroup },
        success: SuccessfulSelectGroup,
        error: function() {
            alert("Error retrieving blackout information.");
        }
    });
}

function SuccessfulSelectGroup(response) {
    var blackoutInfo = document.getElementById("blackout-info");
    blackoutInfo.innerHTML = "";
    if (response) {
        blackoutInfo.textContent = response;
    } else {
        blackoutInfo.textContent = "No blackout information available for this group.";
    }
}

function ShowOneDay() {
    var oneDayList = document.getElementById("one-day-list");
    var oneWeekList = document.getElementById("one-week-list");
    oneDayList.classList.remove("d-none");
    oneWeekList.classList.add("d-none");

    var groupSelect = document.getElementById("group-select");
    var selectedGroup = groupSelect.value;
    
    if (selectedGroup === "0" || selectedGroup == null) {
        alert("Please, select group.");
        return;
    }

    $.ajax({
        url: "/BlackoutList/SelectOneDay",
        type: "Get",
        data: { groupId: selectedGroup },
        success: SuccessfulInsertData,
        error: ErrorInsertData
    });
}

function SuccessfulInsertData(response) {
    var list = document.getElementById("today-list");
    list.innerHTML = "";
    response.forEach(AppendSchedule);
}

function AppendSchedule(item) {
    var list = document.getElementById("today-list");
    var p = document.createElement("p");
    p.className = "list-group-item";
    p.textContent = item.startTime + " - " + item.endTime;
    list.appendChild(p);
}

function ErrorInsertData(response) {
    alert("Error of data formatting");
}

function ShowOneWeek() {
    var oneDayList = document.getElementById("one-day-list");
    var oneWeekList = document.getElementById("one-week-list");
    oneDayList.classList.add("d-none");
    oneWeekList.classList.remove("d-none");

    var groupSelect = document.getElementById("group-select");
    var selectedGroup = groupSelect.value;

    if (selectedGroup === "0") {
        alert("Please, select group.");
        return;
    }

    $.ajax({
        url: "/BlackoutList/SelectSchedulesByGroup",
        type: "Get",
        data: { groupId: selectedGroup },
        success: SuccessfulInsertDataForWeek,
        error: ErrorInsertDataForWeek
    });
}

function SuccessfulInsertDataForWeek(response) {
    for (var i = 0; i < 7; i++) {
        var list = document.getElementById(i + "-list");
        list.innerHTML = "";
    }
    response.forEach(AppendScheduleForWeek);
}

function AppendScheduleForWeek(item) {
    var list = document.getElementById(item.day + "-list");
    var p = document.createElement("p");
    p.className = "list-group-item";
    p.textContent = item.startTime + " - " + item.endTime;
    list.appendChild(p);
}

function ErrorInsertDataForWeek(response) {
    alert("Error of data formatting");
}
// #endregion



// #region Download Json File
function DownloadJsonFile() {
    var groupSelect = document.getElementById("group-select");
    var selectedGroup = groupSelect.value;
    window.location.href = "/BlackoutList/DownloadJsonFile?groupId=" + selectedGroup;
}
// #endregion