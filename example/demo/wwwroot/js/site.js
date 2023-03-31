function testAlert(){
    Ajax("/msg/sample/BrowserAlert")
        .then(function () {
        })
        .catch(function (error) {
            console.error('ajax error: ', error);
        });
}

function testConsole(){
    Ajax("/msg/sample/BrowserConsole")
        .then(function () {
        })
        .catch(function (error) {
            console.error('ajax error: ', error);
        });
}

function testToast(){
    Ajax("/msg/sample/BrowserToast")
        .then(function () {
        })
        .catch(function (error) {
            console.error('ajax error: ', error);
        });
}

function testFile(){
    let logWrap = document.getElementById('LogsWrap');
    let logViewer = document.getElementById('Logs');
    let iSlogViewerOn = logWrap.style.display;
    if (iSlogViewerOn !== 'flex') {
        logWrap.style.display = 'flex';
        Ajax("/msg/sample/FileList",)
            .then(function (result) {
                logViewer.innerHTML = result.message;
            })
            .catch(function (error) {
                console.error('ajax error: ', error);
            });
    } else {
        Ajax("/msg/sample/File",)
            .then(function () {
            })
            .catch(function (error) {
                console.error('ajax error: ', error);
            });
    }
}

function testDb(){
    let dbWrap = document.getElementById('DbWrap');
    let iSdbViewerOn = dbWrap.style.display;
    if(iSdbViewerOn !== 'flex') {
        dbWrap.style.display = 'flex';
    }
    Ajax("/msg/sample/Db")
        .then(function () {
            let data = window.flexData;
            AjaxTestDb(data);
        })
        .catch(function (error) {
            console.error('ajax error: ', error);
        })
        .finally(function (){
            clearFlexData();
        });
}

function testAll(){
    Ajax("/msg/sample/SendToAll")
        .then(function () {
        })
        .catch(function (error) {
            console.error('ajax error: ', error);
        });
}


function testJson(){
    Ajax("/msg/sample/Json")
        .then(function () {
            let data = window.flexData;

            const toastBody =
                document.getElementsByClassName('toast-body')[0];
            toastBody.innerHTML = JSON.stringify(data);

            const toast =
                new bootstrap.Toast(document.getElementById('toastWrap'));
            toast.show();
        })
        .catch(function (error) {
            console.error('ajax error: ', error);
        });
}

document.getElementById("BtnTestMulti")
    .addEventListener("click", function () {
        testAlert();
        testToast();
        testConsole();
        testFile();
        testDb();
    });

document.getElementById("CloseLogs")
    .addEventListener("click", function () {
        let logWrap = document.getElementById('LogsWrap');
        logWrap.style.display = "none";
    })

document.getElementById("CloseDb")
    .addEventListener("click", function () {
        let logWrap = document.getElementById('DbWrap');
        logWrap.style.display = "none";
    })

/*
    DB에 기록 된 메세지를 실시간으로 웹에 출력하기 위한 샘플 코드 입니다.
    This is a sample code to output messages written to a DB in real time to the web.
*/
function AjaxTestDb(message){
    let dbViewer = document.getElementById('Db');
    const {Id, Message, Writedates} = message;

    let tdId = document.createElement('td');
    let tdMessage = document.createElement('td');
    let tdWritedate = document.createElement('td');
    tdId.textContent = Id;
    tdMessage.textContent = Message;
    tdWritedate.textContent = Writedates;

    let newTr = document.createElement('tr');
    newTr.appendChild(tdId);
    newTr.appendChild(tdMessage);
    newTr.appendChild(tdWritedate);

    let table = dbViewer.querySelector('table');
    let tbody = table.querySelector('tbody');
    if (!tbody) {
        tbody = document.createElement('tbody');
        table.appendChild(tbody);
    }
    tbody.appendChild(newTr);
    table.appendChild(tbody)
}

function Ajax(
    _ajaxUrl,
    _ajaxMethod,
    _ajaxData,
    _responseDataType,
    _isConsoleLoging,
    _isSpinner) {
    return new Promise(function (resolve, reject) {
        try {
            let ajax = new XMLHttpRequest();

            let ajaxUrl = _ajaxUrl;
            let ajaxMethod = _ajaxMethod || "GET";
            let ajaxData = _ajaxData || "";
            let responseDataType = _responseDataType || "json";
            let isConsoleLoging = _isConsoleLoging || false;
            let isSpinner = _isSpinner || true;
            const loader = document.getElementById("preloader");

            ajax.onerror = function () {
                console.error('ajax error: ', ajax.statusText);
                reject(ajax.statusText);
            }

            ajax.onreadystatechange = () => {
                if (ajax.readyState === XMLHttpRequest.OPENED) {
                    if (isSpinner) {
                        loader.style.display = 'block';
                    }
                }
                if (ajax.readyState === XMLHttpRequest.DONE) {
                    if (isSpinner) {
                        loader.style.display = 'none';
                    }
                    if (ajax.status === 200) {
                        if (isConsoleLoging) {
                            console.log(ajax.responseText);
                        }
                        resolve(ajax.response);
                    } else {
                        console.error('ajax error: ', ajax.statusText);
                        reject(ajax.statusText);
                    }
                }
            };

            if (ajaxData !== "") {
                if (ajaxMethod.toUpperCase() === "GET") {
                    if (ajaxData.indexOf('?') !== -1) {
                        ajaxUrl += "?" + ajaxData;
                    }
                }
            }

            ajax.open(ajaxMethod, ajaxUrl);
            ajax.responseType = responseDataType;

            if (ajaxData !== "") {
                if (ajaxMethod.toUpperCase() === "POST") {
                    ajax.send(ajaxData);
                } else {
                    ajax.send();
                }
            } else {
                ajax.send();
            }

        } catch (error) {
            console.error('ajax error: ', error);
            reject(error);
        }
    });
}