<div align="right">

[![en](https://img.shields.io/badge/lang-en-ed4156.svg)](https://github.com/kinadog/FlexMessage/blob/master/README.en.md)
[![ko](https://img.shields.io/badge/lang-ko-0879ba.svg)](https://github.com/kinadog/FlexMessage/blob/master/README.md)

</div>

---  
<br/>  


<div align="center"><h1>FLEX MESSAGE</h1></div>  
<br/>  


<div align="center">
  <img src="https://img.shields.io/badge/.NET-512BD4?logo=.net&logoColor=white" alt=""/> 

  <img src="https://img.shields.io/badge/C Sharp-239120?logo=sharp&logoColor=white" alt=""/> 

  <img src="https://img.shields.io/badge/JavaScript-a28419?logo=javascript&logoColor=white" alt=""/> 
</div>

<div align="center">  
  <img src="https://img.shields.io/github/v/release/kinadog/FlexMessage?color=0879ba&include_prereleases" alt=""/> 
  <img src="https://img.shields.io/github/downloads/kinadog/FlexMessage/total?color=fPZhdJ" alt=""/> 
  <img src="https://img.shields.io/nuget/dt/FlexMessage?color=0078d4&label=nuget" alt=""/> 
  <img src="https://img.shields.io/github/issues/kinadog/FlexMessage?color=ed793a" alt=""/> 
  <img src="https://img.shields.io/github/issues-closed/kinadog/FlexMessage?color=ed4156" alt=""/> 
</div>  
<br/>


<div align="center"> <h3>.net 웹어플리케이션용 간편한 클라이언트 메세지 라이브러리</h3></div>      
<br>
<div align="center"> :link: <a href="https://flexmessage.faither.me" target="_blank">DEMO WEBSITE</a></div>

<br/>  


:bulb: .net 웹어플리케이션의 백엔드에서,
1. `런타임` 과정 중
2. `C#` 코드를 사용하여
3. `클라이언트`에게
4. `여러가지 포맷`으로
5. `다양한 메세지`를 간단하게

보낼 수 있습니다.

<br/>  
<a href=""><img src="https://s3.ap-northeast-2.amazonaws.com/muple.bucket/BoardImage/flexMessage.gif" alt=""></a>  
<br/>  


# 특징
- **서버 사이드에서 직접 웹브라우저로 메세지 전송**     
  `C#` 코드를 사용하여 간단하게 `클라이언트의 웹브라우저`로 다양한 메세지를 전송할 수 있습니다.


- **다양한 타입의 메세지 전송방식**     
  자바스크립트의 기본 `alert('')` 부터, `console.log('')`는 물론이며, 개발자가 마음대로 커스터마이징 할 수 있는 toast메시지와
  alert메시지, 그리고 서버의 로컬디스크에 Text 파일로 저장하거나 데이터베이스에 저장하는 등 개발자가 상황에 따라 필요한 모든 종류의 메세징을 구현할 수 있습니다.


- **복수 타입 메세지 동시 전송 가능**      
  `동시에 여러 타입의 메세지`를 원하는 대로 구성하여 발신 할 수 있습니다.  
  <br/>



# 사용법
```csharp
// Console.WriteLine("") 을 사용하는것처럼 간편하게 호출하시면 됩니다.
Message.Write($"{보낼 메시지}", MsgType.메세지_서비스종류)
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
|
|--- wwwroot (folder)
|      |---- js (folder)
|             |--------- flexMessage.js   // (*)프론트엔드에서 사용되는 js 라이브러리.
|
|--- Configs (folder)
|      |---------------- Configs.cs   // (*)애플리케이션의 설정 정보를 담고 있는 클래스.
|
|--- Messages (folder)
|      |--- Types (folder)
|      |      |--------- BrowserAlertMessages.cs   // 브라우저 알림 메시지 클래스.
|      |      |--------- BrowserConsoleMessage.cs   // 브라우저 개발자 콘솔 메시지 클래스.
|      |      |--------- BrowserToastMessage.cs   // 브라우저 토스트 메시지 클래스.
|      |      |--------- ConsoleMessage.cs   // 시스템 콘솔 메시지 클래스.
|      |      |--------- DbMessage.cs   // 데이터베이스 인서트 메시지 클래스.
|      |      |--------- FileMessage.cs   // 파일 기록 메시지 클래스.
|      |      |--------- IMessageCommon.cs   // 메세지 클래스 용 공통 메서드의 클래스.
|      |      |--------- MessageCommon.cs   // 메세지 클리스 용 공통 메서드의 인터페이스.
|      |      |--------- MsgType.cs   // (*)메시지 타입 정보를 담고 있는 열거형.
|      |--------- FileMessageCngMonitor.cs   // 실시간 파일 변경 감시 기능을 담당하는 클래스.
|      |--------- IMessage.cs   // (*)모든 메시지 클래스가 구현해야 하는 인터페이스.
|      |--------- Message.cs   // (*)메시지를 처리하는 기본 클래스.
|
|--- Middlewares (folder)
|      |--------- WebSocketMiddleware.cs   // (*)클라이언트와 통신하기 위한 미들웨어 클래스.
|
|--- Models (folder)
|      |--------- FileEndPosition.cs   // 실시간 파일 모니터링 시 사용하는 변수 용 클래스.
|      |--------- IFileEndPosition.cs   // 실시간 파일 모니터링 시 사용하는 변수 용 인터페이스.
|
|--- Services (folder)
|      |--------- FlexMessageOptions.cs   // 서비스를 어플리케이션에 등록하기 위한 옵션 클래스.
|      |--------- FlexMessageService.cs   // 서비스를 어플리케이션에 등록하기 위한 클래스.
|
|--- Program.cs   // (*)애플리케이션의 시작점을 담고 있는 클래스.
```  
<br/>  

## .NET 필요 버전
> [.net 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) 이상

<br/>  


## 기본 설치

- 1. Nuget 패키지 설치 :

  ```powershell
  # Package Manager
  PM> NuGet\Install-Package FlexMessage
  ```

  

- 2. 공통페이지에 javascript [flexMessage.js](https://github.com/kinadog/FlexMessage/blob/master/src/wwwroot/js/flexMessage.js) 파일 삽입 (ex: _Layout.cshtml)

  ```javascript
  <script src="https://cdn.jsdelivr.net/gh/kinadog/FlexMessage@master/src/FlexMessage/wwwroot/js/flexMessage.js"></script>
  ```
<br/>  

- 3. [Program.cs](https://github.com/kinadog/FlexMessage/blob/master/src/Program.cs) 파일 편집 :
 ```csharp
 // builder.Services는 아래의 객체입니다.
 // var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFlexMessage(builder); // FlexMessage 서비스 추가
.
.
.

// app.UseRouting() 과 

app.UseFlexMessage(); // FlexMessage 서비스 사용

// app.MapControllerRoute() 사이에 삽입합니다.
// app.Run();
 ```   
<br/>

>**설치 완료!**



<br>

## 추가기능 설정

* **1. 실시간 로그 파일 뷰어 기능을 사용하는 경우**

    * [Program.cs](https://github.com/kinadog/FlexMessage/blob/master/src/Program.cs) 파일 편집 :

        ```csharp
       // builder.Services는 아래의 객체입니다.
       // var builder = WebApplication.CreateBuilder(args);
       builder.Services.AddFlexMessage(builder, option => // FlexMessage 서비스 추가
      {
          option.FileMessageStatus = FileMessageStatus.LiveView;  // 파일타입 메세지의 라이브뷰 보기여부
      });
      .
      .
        ```

    * [flexMessage.js](https://github.com/kinadog/FlexMessage/blob/master/src/wwwroot/js/flexMessage.js) 파일 편집 :
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
                        // 다른 종류의 메세지들 ...
                        case ""....
                    }
                }
        ```  
      <br/>  


* **2. 부트스트랩이 아닌 다른 Toast 자바스크립트 플러그인을 사용하려는 경우**
    * [flexMessage.js](https://github.com/kinadog/FlexMessage/blob/master/src/wwwroot/js/flexMessage.js) 파일 편집 :

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
              // 다른 종류의 메세지들 ...
              case ""....
          }
      }
      ```  
      ＃ `Toast메시지` 뿐만 아니라 `Alert메시지`도 별도의 커스텀 플러그인을 사용하고자 한다면 같은 방법으로 적용이 가능 합니다.  
      <br/>


* **3. Database Insert 기능을 사용하는 경우**
    * [Program.cs](https://github.com/kinadog/FlexMessage/blob/master/src/Program.cs) 파일 편집 :

       ```csharp
       // builder.Services는 아래의 객체입니다.
       // var builder = WebApplication.CreateBuilder(args);
       builder.Services.AddFlexMessage(builder, option => // FlexMessage 서비스 추가
      {
          option.FileMessageStatus = FileMessageStatus.LiveView 또는 Off;  // 파일타입 메세지의 라이브뷰 보기여부
      }, message => {
          try{
              // 데이터베이스에 message를 입력하는 구문을 코딩합니다.
              var options = new DbContextOptionsBuilder<EfDbContext>().Options;
              var schema = new Schema { Message = message, Writedates = DateTime.Now };
              using var context = new EfDbContext(options);
              context.Schemas.Add(schema);
              context.SaveChangesAsync();
          }
          catch(Exception e){
              Console.WriteLine(e.Message);
          }
      });
      .
      .
      .
      
       ```  
      <br/>  

# 정보

개발자 정보 : Kinadog / [id@faither.me](mailto:id@faither.me)  
개발자 블로그 : [https://blog.faither.me](https://blog.faither.me)

XYZ 라이센스를 준수하며 ``LICENSE``에서 자세한 정보를 확인할 수 있습니다.  
<br/>


# 기여 방법

1. (<https://github.com/faitherme/FlexMessage/fork>)을 포크합니다.
2. (`git checkout -b feature/fooBar`) 명령어로 새 브랜치를 만드세요.
3. (`git commit -am 'Add some fooBar'`) 명령어로 커밋하세요.
4. (`git push origin feature/fooBar`) 명령어로 브랜치에 푸시하세요.
5. Pull request를 보내주세요.

<br/>
<br/>
<br/>