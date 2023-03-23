<div align="center"><h1>FlexMessage</h1></div>  

<div align="center">
  <img src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=.net&logoColor=white" alt=""/> 
  <img src="https://img.shields.io/badge/C Sharp-239120?style=for-the-badge&logo=sharp&logoColor=white" alt=""/> 
  <img src="https://img.shields.io/badge/JavaScript-a28419?style=for-the-badge&logo=javascript&logoColor=white" alt=""/> 
</div>

<div align="center">  
  <img src="https://img.shields.io/github/downloads/kinadog/FlexMessage/total?color=fPZhdJ" alt=""/> 
  <img src="https://img.shields.io/github/v/release/kinadog/FlexMessage?color=0879ba&include_prereleases" alt=""/> 
  <img src="https://img.shields.io/github/issues/kinadog/FlexMessage?color=ed793a" alt=""/> 
  <img src="https://img.shields.io/github/issues-closed/kinadog/FlexMessage?color=ed4156" alt=""/> 
</div>  

<br/>

<div align="center"> <h3>Easy and versatile client message library for .net web applications</h3></div>     
<br/>  

you can,

1. during the `runtime process`
2. using `C# code`
3. send `various messages`
4. to the `client`
5. in `various formats`

in the backend of a .net web application.

<br/>

<a href=""><img src="https://s3.ap-northeast-2.amazonaws.com/muple.bucket/BoardImage/flexMessage.gif" alt=""></a>  
<br/>
# Features
- **Send messages directly from server side to web browser**  
  You can easily send various messages to the client's web browser using C# code.


- **Various types of message delivery methods**  
  From basic JavaScript alert('') to console.log(''), and customizable toast messages and alert messages, as well as
  saving to local disk as a text file or storing in a database, developers can implement any type of messaging
  required depending on the situation.


- **Simultaneous transmission of multiple types of messages possible**  
  You can configure and send multiple types of messages simultaneously as desired.   
  <br/>

# Usage

```csharp
// Simply call it like using Console.WriteLine("").
Message.Write($"{message_to_send}", (enum)MsgType.message_service_type)
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
|--- Hubs (folder)
|      |---------------- MessageHub.cs   // (*)Hub class for communicating with clients.
|
|--- Messages (folder)
|      |--- Types (folder)
|      |      |--------- BrowserAlertMessages.cs   // Browser notification message class.
|      |      |--------- BrowserConsoleMessage.cs   // Browser developer console message class.
|      |      |--------- BrowserToastMessage.cs   // Browser toast message class.
|      |      |--------- ConsoleMessage.cs   // System console message class.
|      |      |--------- DbMessage.cs   // Database insert message class.
|      |      |--------- FileMessage.cs   // File record message class.
|      |      |--------- MsgType.cs   // (*)Enum containing message type information.
|      |--------- FileMessageCngMonitor.cs   // Class responsible for real-time file change monitoring.
|      |--------- Hasing.cs   // (*)Class for encrypting client's unique Id value.
|      |--------- IMessage.cs   // (*)Interface that all message classes must implement.
|      |--------- Message.cs   // (*)Base class for handling messages.
|
|--- Middlewares (folder)
|      |--------- HubMiddleware.cs   // (*)Middleware class for communicating with clients.
|
|--- Models (folder)
|      |--------- FileEndPosition.cs   // Class for variables used in real-time file monitoring.
|      |--------- IFileEndPosition.cs   // Interface for variables used in real-time file monitoring.
|
|--- Program.cs       // (*)Class containing the starting point of the application.
```   
<br/>

## Required .NET version
> [.net 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) Higher

<br/>

## Required configuration

- Package installation :

    ```powershell
    # Install with .NET CLI
    dotnet add package Microsoft.AspNet.SignalR.Client
    dotnet add package Newtonsoft.Json
    
    # Or
    
    # Install with Package Manager
    PM> NuGet\Install-Package Microsoft.AspNet.SignalR.Client
    PM> NuGet\Install-Package Newtonsoft.Json
    ```  
<br/>  

- Insert [flexMessage.js](https://github.com/kinadog/FlexMessage/blob/master/src/wwwroot/js/flexMessage.js) file into 
the common page (ex: _Layout.cshtml)  

    ```javascript
    <script src="/js/flexMessage.js"></script>
    ```  
<br/>  
 
- Edit [Program.cs](https://github.com/kinadog/FlexMessage/blob/master/src/Program.cs) file :
   ```csharp
   // builder.Services is the following object.
   // var builder = WebApplication.CreateBuilder(args);
   
   Config.ContentRootPath = builder.Environment.ContentRootPath; // added
  .
  .
   builder.Services.AddSignalR(); // added
  .
  .
  
   app.UseMiddleware<HubMiddleware>(); // added
  .
  .
   app.MapHub<MessageHub>("/msghub"); // added
   ```   
  <br/>

  >**Installation complete!!**



<br>

## Configuring additional features

* **When using the Database Insert feature**
  * Edit [Messages/Types/DbMessage.cs](https://github.com/kinadog/FlexMessage/blob/master/src/Messages/Types/DbMessage.cs) file :

     ```csharp
     // # Implement the Database Insert logic within the Write and WriteAsync methods
     ```  
    <br/>  

* **When using the real-time log file viewer feature**

  * Edit [Program.cs](https://github.com/kinadog/FlexMessage/blob/master/src/Program.cs) file :

      ```csharp
      builder.Services
          .AddHostedService<FileMessageCngMonitor>(); // added
    .
    .
      builder.Services
          .AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // added
      builder.Services
          .AddSingleton<IFileEndPosition, FileEndPosition>(); // added
      builder.Services
          .AddSingleton<Dictionary<string, long>>(); // added
    .
    .
      ```

    * Edit [flexMessage.js](https://github.com/kinadog/FlexMessage/blob/master/src/wwwroot/js/flexMessage.js) file :
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


* **If you want to use a Toast JavaScript plugin other than Bootstrap**
  * Edit [flexMessage.js](https://github.com/kinadog/FlexMessage/blob/master/src/wwwroot/js/flexMessage.js) file :

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