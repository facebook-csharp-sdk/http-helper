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

Make a GET request to https://graph.facebook.com/4 and return the response as string.

```csharp
public static string GetSyncSample()
{
    var httpHelper = new HttpHelper("https://graph.facebook.com/4");

    using (var stream = httpHelper.OpenRead())
    {
        using (var reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }
}
```

#### Asynchronous Sample with Task Parallel Library (TPL)

*Available only in .NET 4.0+, SL5 and WinRT (Windows Metro Apps)*

Make an asynchrounous GET request to https://graph.facebook.com/4 and return the response as task of string.

`HTTPHELPER_TPL` conditional compilation symbol must be deinfed inorder to use XTaskAsync methods.

```csharp
public static Task<string> GetAsyncTaskSample(CancellationToken cancellationToken = default(CancellationToken))
{
    var httpHelper = new HttpHelper("https://graph.facebook.com/4");

    return httpHelper
        .OpenReadTaskAsync(cancellationToken)
        .ContinueWith(t =>
                          {
                              if (t.IsFaulted || t.IsCanceled) t.Wait();

                              using (var stream = t.Result)
                              {
                                  using (var reader = new StreamReader(stream))
                                  {
                                      return reader.ReadToEnd();
                                  }
                              }
                          });
}
```