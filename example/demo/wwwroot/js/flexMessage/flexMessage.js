// DOM�� ������ �ε�� ������ ����մϴ�.
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
        // 2�� ���� �������� ������ ������ ������ �õ�
        // Attempt to reconnect if WebSocket is not used for 2 minutes
    }

    function connectWebSocket(){
        // ������ ������ ���� ����
        // Create WebSocket connection to the server
        const serverUrl = protocol + "://" + host + ":" + port + "/ws";
        webSocket = new WebSocket(serverUrl);

        // ������ ������ ȣ��Ǵ� �̺�Ʈ ������
        // Event listener called when the connection opens
        const onOpen = () => {
            console.log("WebSocket connection opened");
            reconnectInterval = null;
            reconnectAttempts = 0;
            clearInterval(reconnectInterval);
            resetInactivityTimeout();
        };

        // �����κ��� �޽����� ������ ȣ��Ǵ� �̺�Ʈ ������
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
                    // �޼����� ������ �ֿܼ� ����մϴ�.
                    // Output the message to the browser console
                    console.log(message);
                    break;
                }
                case "BrowserAlert": {
                    /*
                        ����ϰ��� �Ͻô� alert �÷������� �ڵ带 �Ʒ��� ���� �� �ּ���.
                        �⺻ Javascript�� Alert�� �״�� ����ϼŵ� �����ϴ�.
                        Implement the code for the alert plugin you want to use below.
                        You can also use the default JavaScript Alert.
                    */
                    alert(message);
                    break;
                }
                case "BrowserToast": {
                    /*
                        ������ Bootstrap�� �⺻ Toast����� ���� �Ͽ����ϴ�.
                        �ٸ� Toast ���̺귯���� ����Ͻ÷���,
                        �Ʒ��� �ڵ带 ����Ͻ÷��� Toast ���̺귯���� ���� �ڵ�� ���� �Ͻñ� �ٶ��ϴ�.
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
                        ���Ͽ� ��� �� �޼����� �ǽð����� ���� ����ϱ� ���� ���� �ڵ� �Դϴ�.
                        �ش� ����� �ʿ� �����ø� �� �ڵ带 ���� �Ͻø� �˴ϴ�.
                        This is a sample code to output messages written to a file in real time to the web.
                        If you don't need this functionality, you can delete this code.
                    */
                    let logViewer = document.getElementById('Logs');
                    logViewer.textContent += message;
                    break;
                }
                default: {
                    // �޼����� ������ �ֿܼ� ����մϴ�.
                    // Output the message to the browser console
                    console.log(message);
                }
            }
            resetInactivityTimeout();
        };

        // ������ ������ ȣ��Ǵ� �̺�Ʈ ������
        // Event listener called when the connection closes
        const onClose = () => {
            console.log('WebSocket connection closed');
            if (reconnectAttempts < maxReconnectAttempts) {
                reconnectWebSocket();
            } else {
                removeAllListeners();
            }
        };

        // ������ �߻��ϸ� ȣ��Ǵ� �̺�Ʈ ������
        // Event listener called when an error occurs
        const onError = (event) => {
            console.error('WebSocket error:', event);
            if (reconnectAttempts < maxReconnectAttempts) {
                reconnectWebSocket();
            } else {
                removeAllListeners();
            }
        };

        // �̺�Ʈ ������ �߰�
        // Add EventListener
        webSocket.addEventListener("open", onOpen);
        webSocket.addEventListener("message", onMessage);
        webSocket.addEventListener("close", onClose);
        webSocket.addEventListener("error", onError);

        // �̺�Ʈ ������ ����
        // remove EventListener
        function removeAllListeners(){
            webSocket.removeEventListener("open", onOpen);
            webSocket.removeEventListener("message", onMessage);
            webSocket.removeEventListener("close", onClose);
            webSocket.removeEventListener("error", onError);
            console.log('WebSocket Listener removed');
        }
    }

    // 5�ʸ��� ������ �õ�
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

// �۽� �� Json�� ����
//Save transmitted JSON value
function setFlexData(value) {
    window.flexData = value;
    flexEvent.publish('flexData', value);
}

// �� ���� �� ���ϴ� �۾� ����
// Perform desired actions when the value changes
function onFlexDataChanged(value) {
    console.log(value);
}

// Json�� ����� ���� �ʱ�ȭ
// Initialize variable for storing JSON value
function clearFlexData() {
    window.flexData = null;
}

// �̺�Ʈ ����
//Subscribe events
flexEvent.subscribe('flexData', onFlexDataChanged);