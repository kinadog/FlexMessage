<div style="text-align:center"><h1>FlexMessage</h1></div>

<div style="text-align:center">
  <img src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=.net&logoColor=white" 
style="border-radius: 4px"  
alt=""/> 
  
  <img src="https://img.shields.io/badge/C Sharp-239120?style=for-the-badge&logo=sharp&logoColor=white"  style="border-radius: 4px" alt=""/> 
  
  <img src="https://img.shields.io/badge/JavaScript-E05F2C?style=for-the-badge&logo=javascript&logoColor=white"  style="border-radius: 4px" alt=""/> 
</div>  
<br/>  


>.net 웹어플리케이션용 간편하고 강력한 클라이언트 **메세지 라이브러리**      

  
<br/>  


<p>
.net 웹어플리케이션의 백엔드에서도 런타임 과정 중에서 C# 코드를 사용하여 클라이언트에게 여러가지 포맷으로 간단하게 다양한 메세지를 보낼 수 있습니다.
</p>

<br/>  
<br/>  
<a href=""><img src="https://s3.ap-northeast-2.amazonaws.com/muple.bucket/BoardImage/flexMessage.gif"></a>  
<br/>  


# 특징
- **서버 사이드에서 직접 웹브라우저로 메세지 전송**. C# 코드를 사용하여 간단하게 클라이언트의 웹브라우저로 다양한 메세지를 전송할 수 있습니다.
 

- **다양한 타입의 메세지 전송방식**. 자바스크립트의 기본 `alert('')` 부터, `console.log('')`는 물론이며, 개발자가 마음대로 커스터마이징 할 수 있는 toast메시지와 
  alert메시지, 그리고 서버의 로컬디스크에 Text 파일로 저장하거나 데이터베이스에 저장하는 등 개발자가 상황에 따라 필요한 모든 종류의 메세징을 구현할 수 있습니다.
 

- **복수 타입 메세지 동시 전송 가능**. 동시에 여러 타입의 메세지를 원하는 대로 구성하여 발신 할 수 있습니다.  
<br/>  
 


# 사용법  
```csharp
Message.Write($"{보낼 메시지}", (enum)MsgType.메세지_서비스종류)
```  
<br/>  

메시지 서비스의 종류는 enum타입으로 정의되어 있습니다.  
```csharp
/// <summary>
/// 메시지 서비스 종류가 정의 된 enum class
/// <summary>
public enum MsgType
{
    Console, // 어플리케이션 시스템 콘솔 메시지 (Console.WriteLine)
    File, // 물리적 디스크의 Text파일에 기록하는 메시지
    BrowserConsole, // 웹브라우저의 개발자메뉴 콘솔에 출력하는 메시지
    BrowserAlert, // 웹브라우저의 Alert창으로 띄우는 메시지
    BrowserToast, // 웹브라우저의 Toast로 보내는 메시지
    Db // 데이터베이스에 입력시키는 메시지
}
```  
<br/>  

메시지 발송 예제  

```csharp
/// 1. 웹브라우저 Alert창으로.
Message.Write("Sample Message.", MsgType.BrowserAlert);

/// 2. 웹브라우저의 개발자메뉴의 console log로.
Message.Write("Sample Message.", MsgType.BrowserConsole);

/// 3. 텍스트 파일로 (비동기)
await Message.WriteAsync("Sample Message.", MsgType.File);

/// 4. 여러가지 방법을 한꺼번에 발송.
/// 웹브라우저의 Toast 메시지를 보내면서, 개발자메뉴 콘솔에 출력하며, 텍스트 파일에도 기록
await Message.WriteAsync("Sample Message.",
                            MsgType.BrowserConsole,
                            MsgType.BrowserToast,
                            MsgType.Db);
```  
<br/>  



# 설치  
<br/>  

## 파일 구조
```csharp
FlexMessage (root)
├── wwwroot
│      └── js
│           └── flexMessage.js   // (*)프론트엔드에서 사용되는 js 라이브러리입니다.
├── Configs
│      └── Configs.cs   // (*)애플리케이션의 설정 정보를 담고 있는 클래스입니다.
├── Hubs
│      └── MessageHub.cs   // (*)클라이언트와 통신하기 위한 Hub클래스입니다.
├── Messages
│      ├── Types
│      │     ├── BrowserAlertMessages.cs   // 브라우저 알림 메시지 클래스.
│      │     ├── BrowserConsoleMessage.cs   // 브라우저 개발자 콘솔 메시지 클래스.
│      │     ├── BrowserToastMessage.cs   // 브라우저 토스트 메시지 클래스.
│      │     ├── ConsoleMessage.cs   // 시스템 콘솔 메시지 클래스.
│      │     ├── DbMessage.cs   // 데이터베이스 인서트 메시지 클래스.
│      │     ├── FileMessage.cs   // 파일 기록 메시지 클래스.
│      │     └── MsgType.cs   // (*)메시지 타입 정보를 담고 있는 열거형.
│      ├── FileMessageCngMonitor.cs   // 실시간 파일 변경 모니터링 기능을 담당하는 클래스입니다.
│      ├── Hasing.cs   // (*)클라이언트의 고유Id값의 암호화를 위한 클래스입니다.
│      ├── IMessage.cs   // (*)모든 메시지 클래스가 구현해야 하는 인터페이스입니다.
│      └── Message.cs   // (*)메시지를 처리하는 기본 클래스입니다.
├── Middlewares
│      └── HubMiddleware.cs   // (*)클라이언트와 통신하기 위한 미들웨어 클래스입니다.
├── Models
│      ├── FileEndPosition.cs   // 실시간 파일 모니터링 시 사용하는 변수저장 용 클래스 입니다.
│      └── IFileEndPosition.cs   // 실시간 파일 모니터링 시 사용하는 변수저장 용 인터페이스 입니다.
└── Program.cs       // (*)애플리케이션의 시작점을 담고 있는 클래스입니다.
```  
<br/>  

## .NET 필요 버전
> [.net 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) 이상  

<br/>  


## 필수 설정

- 패키지 설치 :

    ```powershell
  # .NET CLI
  dotnet add package Microsoft.AspNet.SignalR
  ```  
  또는 
   ```powershell
  # Package Manager
  PM> NuGet\Install-Package Microsoft.AspNet.Signal
   ```  
    
<br/>  

- 공통페이지에 [flexMessage.js]() 파일 삽입 (ex: _Layout.cshtml)
  
    ```html
    <script src="/js/flexMessage.js"></script>
    ```
<br/>  

- [Program.cs]() 파일 편집 :
   ```csharp
   // builder.Services는 아래의 객체입니다.
   // var builder = WebApplication.CreateBuilder(args);
   
   Config.ContentRootPath = builder.Environment.ContentRootPath; // 추가
  .
  .
   builder.Services.AddSignalR(); // 추가
  .
  .
  
   app.UseMiddleware<HubMiddleware>(); // 추가
  .
  .
   app.MapHub<MessageHub>("/msghub"); // 추가
   ```   
  <br/>

  >**설치완료!**



<br>

## 추가기능 설정

* **Database Insert 기능을 사용하는 경우**  
   * [Messages/Types/DbMessage.cs]() 파일 편집 :

      ```csharp
      // # Write 메서드와 WriteAsync 메서드 내부의 Database Insert 로직 구현
      ```  
     <br/>  

* **실시간 로그 파일 뷰어 기능을 사용하는 경우**

  * [Program.cs]() 파일 편집 :

      ```csharp
      builder.Services
          .AddHostedService<FileMessageCngMonitor>(); // 추가
    .
    .
      builder.Services
          .AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // 추가
      builder.Services
          .AddSingleton<IFileEndPosition, FileEndPosition>(); // 추가
      builder.Services
          .AddSingleton<Dictionary<string, long>>(); // 추가
    .
    .
      ```

  * [flexMessage.js]() 파일 편집 :
      ```javascript
              connection.on("ReceiveMessage", function(msgType, 
                    message) {
                  switch (msgType) {
                      case "File": {
                          const toast =
                              // 실시간 로그파일 뷰어로 사용할 div element를 
                              // 페이지의 원하는 곳에 만듭니다. (이 코드에선 #Logs)
                              let logViewer = document.getElementById('Logs');
                              logViewer.textContent += "\n" + message;
                          break;
                      }
                      /// 다른 종류의 메세지들 ...
                      case ""....
                  }
              }
      ```  
    <br/>  


* **부트스트랩이 아닌 다른 Toast 자바스크립트 플러그인을 사용하려는 경우**
  * [flexMessage.js]() 파일 편집 :

    ```javascript
    connection.on("ReceiveMessage", function (msgType, message) {
        switch (msgType) {
            case "BrowserToast": {
              //이 코드는 Bootstrap의 Toast를 실행시키는 코드 입니다.
              //만약 다른 Toast 플러그인을 사용하신다면 이부분의 코드를 변경합니다.
                const toastBody =
                    document.getElementsByClassName('toast-body')[0];
                toastBody.innerHTML = message;
                const toast =
                    new bootstrap.Toast(document.getElementById('toastWrap'));
                toast.show();
                break;
            }
            /// 다른 종류의 메세지들 ...
            case ""....
        }
    }
    ```  
    ＃ `Toast메시지` 뿐만 아니라 `Alert메시지`도 별도의 커스텀 플러그인을 사용하고자 한다면 같은 방법으로 적용이 가능 합니다.  
<br/>  


# 정보
  
개발자 정보 : Kinadog / [id@faither.me](mailto:id@faither.me)  
개발자 블로그 : [https://blog.faither.me](https://blog.faither.me)  

XYZ 라이센스를 준수하며 ``LICENSE``에서 자세한 정보를 확인할 수 있습니다.  
<br/>  


# 기여 방법

1. (<https://github.com/faitherme/FlexMessage/fork>을 포크합니다.)
2. (`git checkout -b feature/fooBar`) 명령어로 새 브랜치를 만드세요.
3. (`git commit -am 'Add some fooBar'`) 명령어로 커밋하세요.
4. (`git push origin feature/fooBar`) 명령어로 브랜치에 푸시하세요.
5. 풀리퀘스트를 보내주세요.

#
