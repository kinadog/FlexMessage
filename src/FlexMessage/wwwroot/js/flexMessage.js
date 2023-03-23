// DOM�� ������ �ε�� ������ ����մϴ�.
// Wait for the DOM to be fully loaded
document.addEventListener("DOMContentLoaded", async () => {

    // signalR ��꿡 �����մϴ�.
    // Create a connection to the signalR hub
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/msghub") // "/msghub" URL�� ����Ͽ� ��꿡 �����մϴ�. (Use the "/msghub" URL to connect to the hub)
        .configureLogging(signalR.LogLevel.None) // signalR�� ��� �޼����� �α׷� ������ �ʽ��ϴ�. (Do not log any messages from signalR)
        .withAutomaticReconnect() // ������ ������ ��� �ڵ����� �翬���մϴ�. (Automatically try to reconnect if the connection is lost)
        .build(); // ���� ��ü�� �����մϴ�. (Build the connection object)

    // �������� "ReceiveMessage" �̺�Ʈ�� ó���մϴ�.
    // Handle the "ReceiveMessage" event from the server
    connection.on("ReceiveMessage", function (msgType, message) {
        switch (msgType) {
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
                logViewer.textContent += "\n" + message;
                break;
            }
            case "Db": {
                /*
                    DB�� ��� �� �޼����� �ǽð����� ���� ����ϱ� ���� ���� �ڵ� �Դϴ�.
                    �ش� ����� �ʿ� �����ø� �� �ڵ带 ���� �Ͻø� �˴ϴ�.
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
                // �޼����� ������ �ֿܼ� ����մϴ�.
                // Output the message to the browser console
                console.log(message);
            }
        }
    });

    // ��꿡 ������ �����մϴ�.
    // Start the connection to the hub
    async function start() {
        try {
            await connection.start(); // Attempt to start the connection
            let hashedConnectionId = await connection.invoke("Hasing", connection.connectionId)
                .then(async function (result) {
                    await createCookie("signalr_connectionId", result, 1);
                    console.log(connection.connectionId);
                });
        } catch (err) {
            console.log(err);
            // ������ �����ϸ� 5�� �� �ٽ� �õ��մϴ�.
            // If the connection fails, try again in 5 seconds
            setTimeout(start, 5000);
        }
    }

    connection.onclose(async () => {
        await start();
    });


    await start().then(function () {
    });
});

// ��Ű�� �����մϴ�.
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

// ��Ű�� �о�ɴϴ�.
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