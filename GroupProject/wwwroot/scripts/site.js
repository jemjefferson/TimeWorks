function getTime() {
    let date = new Date();
    let dateString = `${date.getHours()}:`;
    let minutes = date.getMinutes() < 10 ? '0' + date.getMinutes() : date.getMinutes();
    let seconds = date.getSeconds() < 10 ? '0' + date.getSeconds() : date.getSeconds();
    let month = date.getMonth() + 1;
    let day = date.getDate();
    let year = date.getFullYear();
    dateString += `${minutes}:${seconds}&nbsp;&nbsp;&nbsp;${month}/${day}/${year}`;
    let el = document.getElementById("CurrentTime");
    if (el != null) {
        el.innerHTML = dateString;
        resetTimer();
    }
}

function filterEmployees(name) {
    Array.prototype.slice.call(document.getElementsByClassName("selection-item")).forEach(emp => {
        if (emp.innerText.toLowerCase().includes(name.toLowerCase())) {
            emp.style.display = "inline-block";
        } else {
            emp.style.display = "none";
        }
    })
}

function resetTimer() {
    setTimeout('getTime()', 1000);
}

function addBreak() {
    let id = parseInt(document.getElementById("NewBreakCount").value) + 1;
    document.getElementById("NewBreakCount").value = id;
    let table = document.getElementById("BreakTable");
    let html = `<tr class="select-row" onclick="selectRow(this)">` +
        `<td>New</td>` +
        `<td><input type="time" id="NewStart${id}" /></td>` +
        `<td><input type="time" id="NewEnd${id}" /></td>` +
        `<td><input id="NewComment${id}" /></td>` +
        `<td><input type="checkbox" id="NewPaid${id}" /></td>` +
        `</tr>`;

    table.insertAdjacentHTML("beforeend", html);
}

function editBreaks(eid, hid) {
    let ajax = new XMLHttpRequest();
    ajax.onload = () => {
        document.body.insertAdjacentHTML("beforeend", ajax.responseText);
    }
    ajax.open("GET", "/management/EditBreaks/?hid=" + hid + "&eid=" + eid);
    ajax.send();
}

function editHours(eid, hid) {
    let ajax = new XMLHttpRequest();
    ajax.onload = () => {
        document.body.insertAdjacentHTML("beforeend", ajax.responseText);
    }
    ajax.open("GET", "/management/EditHours/?hid=" + hid + "&eid=" + eid);
    ajax.send();
}

function updateHours(eid, hid) {
    let ajax = new XMLHttpRequest();
    ajax.onload = () => {
        getTimecard(eid, null);
        document.getElementById('EditHours').parentNode.remove()
    }

    let paycode = document.getElementById("PayCode").value;
    let startd = new Date(document.getElementById("StartDate").value);
    let endd = new Date(document.getElementById("EndDate").value);
    let startt = new Date('2000-01-01 ' + document.getElementById("StartTime").value);
    let endt = new Date('2000-01-01 ' + document.getElementById("EndTime").value);
    if ((endd < startd || (endd.getTime() == startd.getTime() && endt < startt)) && !document.getElementById("ClockedIn").checked){
        alert('End must be greater than start');
        return;
    }
    ajax.open("POST", '/Management/UpdateHours?hid=' + hid + "&eid=" + eid + "&StartDate=" + document.getElementById("StartDate").value + "&StartTime=" + document.getElementById("StartTime").value + "&ClockedIn=" + document.getElementById("ClockedIn").checked + "&EndDate=" + document.getElementById("EndDate").value + "&EndTime=" + document.getElementById("EndTime").value + "&JobCode=" + paycode + "&Comment=" + document.getElementById("Comment").value);
    ajax.send();
}

function updateBreaks(eid, hid) {
    let ajax = new XMLHttpRequest();
    ajax.onload = () => {
        getTimecard(eid, null);
        document.getElementById("EditBreaks").parentNode.remove();
    }

    ajax.open("POST", "/Management/UpdateBreaks?hid=" + hid + "&nbc=" + document.getElementById("NewBreakCount").value);

    let rows = document.getElementById("BreakTable").rows;
    for (i = 1; i < rows.length; i++) {
        let j = 1;
        let row = rows[i];
        let id = row.cells[0].innerHTML;
        if (id != 'New') {
            ajax.setRequestHeader("Row" + id, id);
            let startt = new Date('2000-01-01 ' + document.getElementById("Start" + id).value);
            let endt = new Date('2000-01-01 ' + document.getElementById("End" + id).value);
            if (endt < startt || endt.getTime() == startt.getTime()) {
                alert('End must be greater than start');
                return;
            }

            ajax.setRequestHeader("Start" + id, document.getElementById("Start" + id).value);
            ajax.setRequestHeader("End" + id, document.getElementById("End" + id).value)
            ajax.setRequestHeader("Comment" + id, document.getElementById("Comment" + id).value);
            ajax.setRequestHeader("Paid" + id, document.getElementById("Paid" + id).checked);
        } else {
            ajax.setRequestHeader("NewStart" + j, document.getElementById("NewStart" + j).value);
            ajax.setRequestHeader("NewEnd" + j, document.getElementById("NewEnd" + j).value)
            ajax.setRequestHeader("NewComment" + j, document.getElementById("NewComment" + j).value);
            ajax.setRequestHeader("NewPaid" + j, document.getElementById("NewPaid" + j).checked);
            j++;
        }
    }

    ajax.send();
}

function deleteHours(eid, hid) {
    let ajax = new XMLHttpRequest();
    ajax.onload = () => {
        getTimecard(eid, null);
        document.getElementById('EditHours').parentNode.remove()
    }

    ajax.open("POST", '/Management/DeleteHours/' + hid);
    ajax.send();
}

function getTimecard(id, element) {
    if (element != null) {
        Array.prototype.slice.call(document.getElementsByClassName("selection-item")).forEach(s => s.classList.remove("selected"));
        element.classList.add("selected");
    }
    let ajax = new XMLHttpRequest();
    ajax.onload = () => {
        document.getElementById("ManagementTimecard").innerHTML = ajax.responseText;
    }

    let url = "/management/timecard/" + id;
    let payperiod = document.getElementById("PayPeriod");
    if (payperiod != null) {
        url += "?payPeriod=" + payperiod.value;
    }
    ajax.open("GET", url);
    ajax.send();
}

function loadEmployeePreview(id, element) {
    Array.prototype.slice.call(document.getElementsByClassName("selection-item")).forEach(s => s.classList.remove("selected"));
    element.classList.add("selected");
    let ajax = new XMLHttpRequest();
    ajax.onload = () => {
        document.getElementById("AdminContainer").innerHTML = ajax.responseText;
    }

    ajax.open("GET", "/Admin/EmployeePreview/" + id);
    ajax.send();
}

function accountLoad(event, element) {
    let el = document.getElementById("AccountDropdown");
    if (el != null) {
        el.remove();
        return;
    }
    let ajax = new XMLHttpRequest();
    ajax.onload = () => {
        document.body.insertAdjacentHTML("beforeend", ajax.responseText);
        let el = document.getElementById("AccountDropdown");
        el.style.left = (element.offsetLeft - (120 - element.offsetWidth) / 2) + "px";
        el.style.top = element.offsetTop + element.offsetHeight + "px";
        document.addEventListener("click", (e) => {
            if (e.target != el && e.target.parentNode != el && e.target.parentNode.parentNode != el) {
                el.remove();
            }
        });

        document.addEventListener("keydown", (e) => {
            el.remove();
        });
    }

    ajax.open("GET", "/Account/AccountDropdown");
    ajax.send();
}

function removeBreak() {
    let rows = document.getElementsByClassName("select-row selected");
    if (rows.length == 1) {
        rows[0].remove();
    }
}

function removePopup(event, element) {
    if (event.target == element) {
        element.remove();
    }
}

function changeClocked(check) {
    let endd = document.getElementById("EndDate");
    let endt = document.getElementById("EndTime");
    let date = new Date();
    let year = date.getFullYear();
    let month = date.getMonth() + 1;
    month = month < 10 ? "0" + "" + month : month;
    let day = date.getDate();
    day = day < 10 ? "0" + "" + day : day;
    if (check) {
        endd.disabled = true;
        endt.disabled = true;
        endd.value = year + "-" + month + "-" + day;
        endt.value = "17:00:00";
    } else {
        endd.disabled = false;
        endt.disabled = false;
        endd.value = year + "-" + month + "-" + day;
        endt.value = "17:00:00";
    }
}

function clockIn(jcid) {
    let ajax = new XMLHttpRequest();
    ajax.onload = setTimeout("location.href = '/'", 1000);
    ajax.open("POST", "/clock-in?jcid=" + jcid + "&tz=" + new Date().getTimezoneOffset());
    ajax.send();
}

function startBreak(p) {
    let ajax = new XMLHttpRequest();
    ajax.onload = setTimeout("location.href = '/'", 1000);
    ajax.open("POST", "/StartBreak?p=" + p + "&tz=" + new Date().getTimezoneOffset());
    ajax.send();
}

function endBreak() {
    let ajax = new XMLHttpRequest();
    ajax.onload = setTimeout("location.href = '/'", 1000);
    ajax.open("POST", "/EndBreak?tz=" + new Date().getTimezoneOffset());
    ajax.send();
}

function clockOut() {
    let ajax = new XMLHttpRequest();
    ajax.onload = setTimeout("location.href = '/'", 1000);
    ajax.open("POST", "/clock-out?tz=" + new Date().getTimezoneOffset());
    ajax.send();
}

function approveTimecard(emp) {
    let ajax = new XMLHttpRequest();
    ajax.onload = () => {
        getTimecard(emp, null);
    }

    ajax.open("POST", "/Management/ApproveTimecard?eid=" + emp + "&pid=" + document.getElementById("PayPeriod").value);
    ajax.send();
}

function selectedRow() {
    let rows = document.getElementsByClassName("selected");
    if (rows.length == 1) {
        return rows[0].cells[0].innerHTML;
    }
}

function selectRow(element) {
    let rows = Array.prototype.slice.call(document.getElementsByClassName("select-row"));
    rows.forEach(r => r.classList.remove("selected"));
    element.classList.add("selected");
}

function updateHourComment(hid) {
    let ajax = new XMLHttpRequest();
    ajax.onload = () => {
        document.getElementById('EditHourComment').parentNode.remove()
    }

    ajax.open("POST", '/UpdateHourComment/?hid=' + hid + "&Comment=" + document.getElementById("CommentArea").value);
    ajax.send();
}

function updateBreakComment(bid) {
    let ajax = new XMLHttpRequest();
    ajax.onload = () => {
        document.getElementById('EditBreakComment').parentNode.remove()
    }

    ajax.open("POST", '/UpdateBreakComment/?bid=' + bid + "&Comment=" + document.getElementById("CommentArea").value);
    ajax.send();
}