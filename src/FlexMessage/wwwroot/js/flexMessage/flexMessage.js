// DOM이 완전히 로드될 때까지 대기합니다.
// Wait for the DOM to be fully loaded
document.addEventListener("DOMContentLoaded", async () => {

    const protocol = window.location.protocol === 'https:' ? 'wss' : 'ws';
    const host = window.location.hostname;
    const port = window.location.port;

    let webSocket;
    let reconnectInterval;
    let inactivityTimeout;
    let reconnectAttempts = 0;
    const maxReconnectAttempts = 3;

    function resetInactivityTimeout() {
        clearTimeout(inactivityTimeout);
        inactivityTimeout = setTimeout(() => {
            console.log("WebSocket inactive for too long, reconnecting...");
            reconnectWebSocket();
        }, 120000);
        // 2분 동안 웹소켓이 사용되지 않으면 재접속 시도
        // Attempt to reconnect if WebSocket is not used for 2 minutes
    }

    function connectWebSocket(){
        // 서버와 웹소켓 연결 생성
        // Create WebSocket connection to the server
        const serverUrl = protocol + "://" + host + ":" + port + "/ws";
        webSocket = new WebSocket(serverUrl);

        // 연결이 열리면 호출되는 이벤트 리스너
        // Event listener called when the connection opens
        const onOpen = () => {
            console.log("WebSocket connection opened");
            reconnectInterval = null;
            reconnectAttempts = 0;
            clearInterval(reconnectInterval);
            resetInactivityTimeout();
        };

        // 서버로부터 메시지를 받으면 호출되는 이벤트 리스너
        // Event listener called when a message is received from the server
        const onMessage = (event) => {
            let data = JSON.parse(event.data);
            let {Message: message, MsgType: msgType} = data;
            switch (msgType) {
                case "Json": {
                    message = JSON.parse(message);
                    setFlexData(message);
                    break;
                }
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
                default: {
                    // 메세지를 브라우저 콘솔에 출력합니다.
                    // Output the message to the browser console
                    console.log(message);
                }
            }
            resetInactivityTimeout();
        };

        // 연결이 닫히면 호출되는 이벤트 리스너
        // Event listener called when the connection closes
        const onClose = () => {
            console.log('WebSocket connection closed');
            if (reconnectAttempts < maxReconnectAttempts) {
                reconnectWebSocket();
            } else {
                removeAllListeners();
            }
        };

        // 오류가 발생하면 호출되는 이벤트 리스너
        // Event listener called when an error occurs
        const onError = (event) => {
            console.error('WebSocket error:', event);
            if (reconnectAttempts < maxReconnectAttempts) {
                reconnectWebSocket();
            } else {
                removeAllListeners();
            }
        };

        // 이벤트 리스너 추가
        // Add EventListener
        webSocket.addEventListener("open", onOpen);
        webSocket.addEventListener("message", onMessage);
        webSocket.addEventListener("close", onClose);
        webSocket.addEventListener("error", onError);

        // 이벤트 리스너 제거
        // remove EventListener
        function removeAllListeners(){
            webSocket.removeEventListener("open", onOpen);
            webSocket.removeEventListener("message", onMessage);
            webSocket.removeEventListener("close", onClose);
            webSocket.removeEventListener("error", onError);
            console.log('WebSocket Listener removed');
        }
    }

    // 5초마다 재접속 시도
    // Attempt to reconnect every 5 seconds
    function reconnectWebSocket() {
        if (reconnectInterval) return;
        reconnectInterval = setInterval(() => {
            reconnectAttempts += 1;
            console.log("Attempting to reconnect...");
            connectWebSocket();
            if (reconnectAttempts >= maxReconnectAttempts) {
                clearInterval(reconnectInterval);
                reconnectInterval = null;
            }
        }, 5000);
    }

    connectWebSocket();
});

class FlexEvent {
    constructor() {
        this.events = {};
    }

    subscribe(eventType, callback) {
        if (!this.events[eventType]) {
            this.events[eventType] = [];
        }
        this.events[eventType].push(callback);
    }

    publish(eventType, data) {
        if (this.events[eventType]) {
            this.events[eventType].forEach(callback => callback(data));
        }
    }
}

const flexEvent = new FlexEvent();

// 송신 된 Json값 저장
//Save transmitted JSON value
function setFlexData(value) {
    window.flexData = value;
    flexEvent.publish('flexData', value);
}

// 값 변경 시 원하는 작업 수행
// Perform desired actions when the value changes
function onFlexDataChanged(value) {
    console.log(value);
}

// Json값 저장용 변수 초기화
// Initialize variable for storing JSON value
function clearFlexData() {
    window.flexData = null;
}

// 이벤트 구독
//Subscribe events
flexEvent.subscribe('flexData', onFlexDataChanged);
