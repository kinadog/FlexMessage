<div align="right">

[![en](https://img.shields.io/badge/lang-en-ed4156.svg)](https://github.com/kinadog/FlexMessage/blob/master/README.en.md)
[![ko](https://img.shields.io/badge/lang-ko-0879ba.svg)](https://github.com/kinadog/FlexMessage/blob/master/README.md)

</div>

---  
<br/>  

<div align="center"><h1>FLEX MESSAGE</h1></div>  

<div align="center">

  <img src="https://img.shields.io/badge/.NET-512BD4?logo=.net&logoColor=white" alt=""/> 

  <img src="https://img.shields.io/badge/C Sharp-239120?logo=sharp&logoColor=white" alt=""/> 

  <img src="https://img.shields.io/badge/JavaScript-a28419?logo=javascript&logoColor=white" alt=""/> 
</div>

<div align="center">  
  <img src="https://img.shields.io/github/v/release/kinadog/FlexMessage?color=0879ba" alt=""/> 
  <img src="https://img.shields.io/github/downloads/kinadog/FlexMessage/total?color=fPZhdJ" alt=""/> 
  <img src="https://img.shields.io/nuget/dt/FlexMessage?color=0078d4&label=nuget" alt=""/> 
  <img src="https://img.shields.io/github/issues/kinadog/FlexMessage?color=ed793a" alt=""/> 
  <img src="https://img.shields.io/github/issues-closed/kinadog/FlexMessage?color=ed4156" alt=""/> 
</div>  

<br/>

<div align="center"> <h3>Easy and versatile client message library for .net web applications</h3></div>    
<br>
<div align="center"> :link: <a href="https://flexmessage.faither.me" target="_blank">DEMO WEBSITE</a></div> 
<br/>  

you can,

1. during the `runtime process`
2. using `C# code`
3. send `various messages`
4. to the `client`
5. in `various formats`

:bulb: in the backend of a .net web application.

<br/>

<a href=""><img src="https://s3.ap-northeast-2.amazonaws.com/muple.bucket/BoardImage/flexMessage.gif" alt=""></a>  
<br/>
# Features
- **Send messages directly from server side to web browser**  
  You can easily send various messages to the `client's web browser` using `C#` code.


- **Various types of message delivery methods**  
  From basic JavaScript `alert('')` to `console.log('')`, and customizable toast messages and alert messages, as well as
  saving to local disk as a text file or storing in a database, developers can implement any type of messaging
  required depending on the situation.


- **Simultaneous transmission of multiple types of messages possible**  
  You can configure and send `multiple types of messages` simultaneously as desired.   
  <br/>

# Usage

```csharp
// Simply call it like using Console.WriteLine("").
Message.Write($"{message_to_send}", MsgType.message_service_type)
```   
<br/>
The types of message services are defined as enum types   

```csharp
/// <summary>
/// Enum class defining the types of message services
/// <summary>
public enum MsgType
{
    Console, // Application system console message (Console.WriteLine)
    File, // Message written to a physical disk text file
    BrowserConsole, // Message displayed in the web browser developer console
    BrowserAlert, // Message displayed in the web browser alert window
    BrowserToast, // Message sent as a web browser toast
    Db // Message inserted into the database
}
```   
<br/>
Message sending examples   

```csharp
/// 1. As a web browser alert window.
Message.Write("Sample Message.", MsgType.BrowserAlert);

/// 2. As a web browser developer console log.
Message.Write("Sample Message.", MsgType.BrowserConsole);

/// 3. As a text file (asynchronously)
await Message.WriteAsync("Sample Message.", MsgType.File);

/// 4. Send messages in various ways at once.
/// Send a web browser Toast message, output to developer console, 
/// and record to a text file
await Message.WriteAsync("Sample Message.",
MsgType.BrowserConsole,
MsgType.BrowserToast,
MsgType.Db);
```   
<br/>  


# Installation
<br/>   

## File structure

```csharp
FlexMessage (root)
|
|--- wwwroot (folder)
|      |---- js (folder)
|             |--------- flexMessage.js   // (*)Front-end js library.
|
|--- Configs (folder)
|      |---------------- Configs.cs   // (*)Class containing application configuration information.
|
|--- Messages (folder)
|      |--- Types (folder)
|      |      |--------- BrowserAlertMessages.cs   // Browser notification message class.
|      |      |--------- BrowserConsoleMessage.cs   // Browser developer console message class.
|      |      |--------- BrowserToastMessage.cs   // Browser toast message class.
|      |      |--------- ConsoleMessage.cs   // System console message class.
|      |      |--------- DbMessage.cs   // Database insert message class.
|      |      |--------- FileMessage.cs   // File record message class.
|      |      |--------- IMessageCommon.cs   // A class for common methods for message classes.
|      |      |--------- MessageCommon.cs   // An interface for common methods for message classes.
|      |      |--------- MsgType.cs   // (*)Enum containing message type information.
|      |--------- FileMessageCngMonitor.cs   // Class responsible for real-time file change monitoring.
|      |--------- IMessage.cs   // (*)Interface that all message classes must implement.
|      |--------- Message.cs   // (*)Base class for handling messages.
|
|--- Middlewares (folder)
|      |--------- WebSocketMiddleware.cs   // (*)Middleware class for communicating with clients.
|
|--- Models (folder)
|      |--------- FileEndPosition.cs   // Class for variables used in real-time file monitoring.
|      |--------- IFileEndPosition.cs   // Interface for variables used in real-time file monitoring.
|
|--- Services (folder)
|      |--------- FlexMessageOptions.cs   // A class for options to register the service in the application.
|      |--------- FlexMessageService.cs   // A class to register the service in the application.
|
|--- Program.cs       // (*)Class containing the starting point of the application.
```   
<br/>

## Required .NET version
> [.net 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) Higher

<br/>

## Required configuration

- 1. Package installation :

  ```powershell
  # Package Manager
  PM> NuGet\Install-Package FlexMessage
  ```  
<br/>  

- 2. Insert javascript [flexMessage.js](https://github.com/kinadog/FlexMessage/blob/master/src/FlexMessage/wwwroot/js/flexMessage/flexMessage.js) file into the common page (ex: _Layout.cshtml)

  ```javascript
  <script src="https://cdn.jsdelivr.net/gh/kinadog/FlexMessage@master/src/FlexMessage/wwwroot/js/flexMessage.js"></script>
  ```  
<br/>  

- 3. Edit [Program.cs](https://github.com/kinadog/FlexMessage/blob/master/src/FlexMessage/Program.cs) file :  
 
  ```csharp
  // builder.Services is the following object.
  // var builder = WebApplication.CreateBuilder(args);
   
  builder.Services.AddFlexMessage(builder); // Add the FlexMessage service.
  .
  .
  .
  
  // Insert between app.UseRouting() 
  
  app.UseFlexMessage(); // Use the FlexMessage service.
  
  // and app.MapControllerRoute().
  // app.Run();
  ```  

<br/>

>**Installation complete!!**



<br>

## Configuring additional features



* **1. When using the real-time log file viewer feature**  
<br/>
  * Edit [Program.cs](https://github.com/kinadog/FlexMessage/blob/master/src/FlexMessage/Program.cs) file :

      ```csharp
      // builder.Services is the following object.
      // var builder = WebApplication.CreateBuilder(args);
      builder.Services.AddFlexMessage(builder, option => // Add the FlexMessage service.
      {
          option.FileMessageStatus = FileMessageStatus.LiveView;  // On/Off live view of file type messages.
      });
      .
      .
      ```

  * Edit [flexMessage.js](https://github.com/kinadog/FlexMessage/blob/master/src/FlexMessage/wwwroot/js/flexMessage/flexMessage.js) file :
      ```javascript
              connection.on("ReceiveMessage", function(msgType, 
                    message) {
                  switch (msgType) {
                      case "File": {
                          const toast =
                              // Create the div element to be used as the real-time log file viewer 
                              // in the desired location on the page. (In this code, #Logs)
                              let logViewer = document.getElementById('Logs');
                              logViewer.textContent += "\n" + message;
                          break;
                      }
                      // Other types of messages...
                      case ""....
                  }
              }
      ```  
    <br/>  
    <br/>  


* **2. When using a Toast JavaScript plugin other than Bootstrap.**  
  <br/>
  * Edit [flexMessage.js](https://github.com/kinadog/FlexMessage/blob/master/src/FlexMessage/wwwroot/js/flexMessage/flexMessage.js) file :

    ```javascript
    connection.on("ReceiveMessage", function (msgType, message) {
        switch (msgType) {
            case "BrowserToast": {
              // This code executes the Toast for Bootstrap.
              // If you want to use a different Toast plugin, modify this section of code.
                const toastBody =
                    document.getElementsByClassName('toast-body')[0];
                toastBody.innerHTML = message;
                const toast =
                    new bootstrap.Toast(document.getElementById('toastWrap'));
                toast.show();
                break;
            }
            // Other types of messages...
            case ""....
        }
    }
    ```  
    ＃ Note that you can also use the same method to apply a custom plugin for `Alert messages`.  
   
    <br/>
    <br/>

* **3. When using the Database Insert feature**  
  <br/>
  * Edit [Program.cs](https://github.com/kinadog/FlexMessage/blob/master/src/FlexMessage/Program.cs) file :

     ```csharp
      // builder.Services is the following object.
      // var builder = WebApplication.CreateBuilder(args);
       builder.Services.AddFlexMessage(builder, option => // Add the FlexMessage service.
      {
          option.FileMessageStatus = FileMessageStatus.LiveView or Off;  // On/Off live view of file type messages.
      }, message => {
          try{
              // The code for inserting a message into the database is implemented.
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

# Information

Developer Information : Kinadog / [id@faither.me](mailto:id@faither.me)  
Developer Blog : [https://blog.faither.me](https://blog.faither.me)

This project follows the APACHE License. Please see [LICENSE](https://www.apache.org/licenses/LICENSE-2.0) for more information.
<br/>


# How to Contribute

1. Fork (<https://github.com/faitherme/FlexMessage/fork>.)
2. Create a new branch with  (`git checkout -b feature/fooBar`).
3. Commit your changes with (`git commit -am 'Add some fooBar'`).
4. Push to the branch with (`git push origin feature/fooBar`).
5. Create a new Pull Request.

<br/>
<br/>
<br/>