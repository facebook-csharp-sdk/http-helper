# HttpHelper

This project has been renamed and was formerly called FluentHttp.Core.

## Overview
HttpHelper is a light weight library aimed to ease the development for your rest client with
consistent api throughout different frameworks whether you are using .net 4.0 or silverlight or
window phone. (It is not aimed to be used directly by the developers, but rather for creating 
a http client wrapper, such as for Facebook, Github, Twitter, Google etc.)

## Supported Frameworks

* .NET 4.5
* .NET 4.0 (client profile supported)
* .NET 3.5 (client profile supported)
* .NET 2.0
* WINRT (Windows Metro Style Applications)
* Silverlight 4.0
* Windows Phone 7
* Mono

## NuGet

```
Install-Package HttpHelper
```

## Samples

### HTTP GET

#### Synchronous Sample

*Synchronous methods are not supported in Silverlight/Windows Phone/WinRT (Windows Metro Apps)*

Make a GET request to https://graph.facebook.com/4 and write the response as string to console.

```csharp
public static void GetSyncSample()
{
    var httpHelper = new HttpHelper("https://graph.facebook.com/4");

    using (var stream = httpHelper.OpenRead())
    {
        using (var reader = new StreamReader(stream))
        {
            var result = reader.ReadToEnd();
            Console.WriteLine(result);
        }
    }
}
```