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
弛
戍式式 wwwroot (folder)
弛      戌式式 js (folder)
弛           戌式式式式式式式式 flexMessage.js   // (*)Front-end js library.
戍式式 Configs (folder)
弛      戌式式式式式式式式式式式式式 Configs.cs   // (*)Class containing application configuration information.
戍式式 Hubs (folder)
弛      戌式式式式式式式式式式式式式 MessageHub.cs   // (*)Hub class for communicating with clients.
戍式式 Messages (folder)
弛      戍式式 Types (folder)
弛      弛     戍式式式式式式式 BrowserAlertMessages.cs   // Browser notification message class.
弛      弛     戍式式式式式式式 BrowserConsoleMessage.cs   // Browser developer console message class.
弛      弛     戍式式式式式式式 BrowserToastMessage.cs   // Browser toast message class.
弛      弛     戍式式式式式式式 ConsoleMessage.cs   // System console message class.
弛      弛     戍式式式式式式式 DbMessage.cs   // Database insert message class.
弛      弛     戍式式式式式式式 FileMessage.cs   // File record message class.
弛      弛     戌式式式式式式式 MsgType.cs   // (*)Enum containing message type information.
弛      戍式式式式式式式 FileMessageCngMonitor.cs   // Class responsible for real-time file change monitoring.
弛      戍式式式式式式式 Hasing.cs   // (*)Class for encrypting client's unique Id value.
弛      戍式式式式式式式 IMessage.cs   // (*)Interface that all message classes must implement.
弛      戌式式式式式式式 Message.cs   // (*)Base class for handling messages.
戍式式 Middlewares (folder)
弛      戌式式式式式式式 HubMiddleware.cs   // (*)Middleware class for communicating with clients.
戍式式 Models (folder)
弛      戍式式式式式式式 FileEndPosition.cs   // Class for variables used in real-time file monitoring.
弛      戌式式式式式式式 IFileEndPosition.cs   // Interface for variables used in real-time file monitoring.
戌式式 Program.cs       // (*)Class containing the starting point of the application.
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
<br/>
<br/>
<br/>
<br/>
<br/>
<br/>
<br/>
<br/>
<br/>
<br/>
<br/>
<br/>