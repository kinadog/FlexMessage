// DOM이 완전히 로드될 때까지 대기합니다.
// Wait for the DOM to be fully loaded
document.addEventListener("DOMContentLoaded", async () => {

    const protocol = window.location.protocol === 'https:' ? 'wss' : 'ws';
    const host = window.location.hostname;
    const port = window.location.port;

    // 서버와 웹소켓 연결 생성
    const webSocket = new WebSocket(protocol + '://' + host + ':' + port +'/ws');

    // 연결이 열리면 호출되는 이벤트 리스너
    webSocket.addEventListener('open', (event) => {
        console.log('WebSocket connection opened');
    });

    // 서버로부터 메시지를 받으면 호출되는 이벤트 리스너
    webSocket.addEventListener('message', (event) => {
        let data = JSON.parse(event.data);
        let {Message: message, MsgType: msgType, IsId: isId} = data;
        if(isId){
            createCookie("webSocketId",message,1);
        }else{
            switch (msgType) {
                case "BrowserConsole": {
                    // 메세지를 브라우저 콘솔에 출력합니다.
                    // Output the message to the browser console
                    console.log(message);
                    break;
                }
                case "BrowserAlert": {
                    /*
                        사용하고자 하시는 alert 플러그인의 코드를 아래에 구현 해 주세요.
                        기본 Javascript의 Alert를 그대로 사용하셔도 좋습니다.
                        Implement the code for the alert plugin you want to use below.
                        You can also use the default JavaScript Alert.
                    */
                    alert(message);
                    break;
                }
                case "BrowserToast": {
                    /*
                        예제는 Bootstrap의 기본 Toast기능을 구현 하였습니다.
                        다른 Toast 라이브러리를 사용하시려면,
                        아래의 코드를 사용하시려는 Toast 라이브러리의 구현 코드로 변경 하시기 바랍니다.
                        This code is an example implementation of the default Toast functionality from Bootstrap.
                        If you want to use a different Toast library,
                        replace the code below with the implementation code for the Toast library you want to use.
                    */
                    const toastBody =
                        document.getElementsByClassName('toast-body')[0];
                    toastBody.innerHTML = message;

                    const toast =
                        new bootstrap.Toast(document.getElementById('toastWrap'));
                    toast.show();
                    break;
                }
                case "File": {
                    /*
                        파일에 기록 된 메세지를 실시간으로 웹에 출력하기 위한 샘플 코드 입니다.
                        해당 기능이 필요 없으시면 이 코드를 삭제 하시면 됩니다.
                        This is a sample code to output messages written to a file in real time to the web.
                        If you don't need this functionality, you can delete this code.
                    */
                    let logViewer = document.getElementById('Logs');
                    logViewer.textContent += message;
                    break;
                }
                case "Db": {
                    /*
                        DB에 기록 된 메세지를 실시간으로 웹에 출력하기 위한 샘플 코드 입니다.
                        해당 기능이 필요 없으시면 이 코드를 삭제 하시면 됩니다.
                        This is a sample code to output messages written to a DB in real time to the web.
                        If you don't need this functionality, you can delete this code.
                    */
                    let dbViewer = document.getElementById('Db');
                    let tableData = JSON.parse(message);
                    const {Id, Message, Writedates} = tableData;

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
                    break;
                }
                default: {
                    // 메세지를 브라우저 콘솔에 출력합니다.
                    // Output the message to the browser console
                    console.log(message);
                }
            }
        }
    });

    // 연결이 닫히면 호출되는 이벤트 리스너
    webSocket.addEventListener('close', (event) => {
        console.log('WebSocket connection closed');
    });

    // 오류가 발생하면 호출되는 이벤트 리스너
    webSocket.addEventListener('error', (event) => {
        console.error('WebSocket error:', event);
    });


});

// 쿠키를 생성합니다.
// Create Cookie.
function createCookie(name, value, days) {
    let expires;
    if (days) {
        let date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    } else {
        expires = "";
    }
    document.cookie = name + "=" + value + expires + "; path=/; secure";
    return Promise.resolve();
}

// 쿠키를 읽어옵니다.
// Read Cookie.
function readCookie(name) {
    let nameEQ = name + "=";
    let ca = document.cookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) === ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) === 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}