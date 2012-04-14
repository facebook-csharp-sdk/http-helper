# HttpHelper

This project has been renamed and was formerly called FluentHttp.Core.

## Overview
HttpHelper is a light weight library aimed to ease the development for your rest client with
consistent api throughout different frameworks whether you are using .net 2.0+ or silverlight or
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

**Note:**

* **The developer is reponsible for manually disposing request (write) and response (read) streams.**
* **Always create new instance of `HttpHelper` for each new requests.**

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

#### Asynchronous Sample with Async/Await

*Available only in .NET 4.5+, and WinRT (Windows Metro Apps)*

Make an asynchrounous GET request to https://graph.facebook.com/4 and return the response as task of string.

`HTTPHELPER_TPL` conditional compilation symbol must be defined in order to use XTaskAsync methods.

```csharp
public static async Task<string> GetAsyncAwaitSample(CancellationToken cancellationToken = default(CancellationToken))
{
    var httpHelper = new HttpHelper("https://graph.facebook.com/4");

    using (var stream = await httpHelper.OpenReadTaskAsync(cancellationToken))
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

`HTTPHELPER_TPL` conditional compilation symbol must be defined in order to use XTaskAsync methods.

```csharp
public static Task<string> GetAsyncTaskSample(CancellationToken cancellationToken = default(CancellationToken))
{
    var httpHelper = new HttpHelper("https://graph.facebook.com/4");

    return httpHelper
        .OpenReadTaskAsync(cancellationToken)
        .ContinueWith(t =>
                          {
                              // propagate previous task exceptions correctly.
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

#### Asynchronous Sample with Event Programming Model (EPM)

*Available in all .NET plaforms.*

Make an asynchrounous GET request to https://graph.facebook.com/4 and call the callback on completion.
`Action<string, object, bool, Exception>` is mapped to response string, userState, isCancelled and Exception. 

```csharp
public static void GetAsyncSample(Action<string, object, bool, Exception> callback = null, object userState = null)
{
    var httpHelper = new HttpHelper("https://graph.facebook.com/4");

    httpHelper.OpenReadCompleted +=
        (o, e) =>
        {
            if (callback == null)
            {
                // make sure to dispose the response stream
                if(!e.Cancelled && e.Error != null)
                    using (var stream = e.Result) { }
                return;
            }

            if (e.Cancelled)
                callback(null, e.UserState, true, null);
            else if (e.Error != null)
                callback(null, e.UserState, false, e.Error);
            else
            {
                try
                {
                    using (var stream = e.Result)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            callback(reader.ReadToEnd(), e.UserState, false, null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    callback(null, e.UserState, false, ex);
                }

            }
        };

    httpHelper.OpenReadAsync(userState);
}
```

You can cancel the asynchronous requests for EPM using `httpHelper.CancelAsync()` method.

### HTTP POST

#### Synchronous Sample

*Synchronous methods are not supported in Silverlight/Windows Phone/WinRT (Windows Metro Apps)*

Make a POST request to https://graph.facebook.com/me/feed and return the response as string. 
For the below sample you can get the `access_token` from http://developers.facebook.com/tools/explorer/ 
Make sure you have `publish_stream` extended permission which is required to post to your facebook wall.

`HTTPHELPER_URLENCODING` conditional compilation symbol must be defined in order to use 
`HttpHelper.UrlEncode` or `HttpHelper.UrlDecode` methods.

Windows Phone and WinRT (Windows Metro Apps) do not have the property ContentLength for HttpWebRequest. In order to reduce
the `#if (WINDOWS_PHONE || NETFX_CORE)` you can use the `TrySetContentLength` method.

```csharp
public static string PostSyncSample(IEnumerable<KeyValuePair<string, string>> parameters)
{
    var httpHelper = new HttpHelper("https://graph.facebook.com/me/feed");
    var request = httpHelper.HttpWebRequest;
    request.Method = "POST";
    request.ContentType = "application/form-url-encoded";

    var bodyString = new StringBuilder();
    foreach (var kvp in parameters)
        bodyString.AppendFormat("{0}={1}&", HttpHelper.UrlEncode(kvp.Key), HttpHelper.UrlEncode(kvp.Value));
    if (bodyString.Length > 0)
        bodyString.Length -= 1;

    var bodyByteArray = Encoding.UTF8.GetBytes(bodyString.ToString());
    request.TrySetContentLength(bodyByteArray.Length);

    // write to request body only if we have data, otherwise directly open the read stream
    if (bodyByteArray.Length > 0)
    {
        using (var stream = httpHelper.OpenWrite())
        {
            stream.Write(bodyByteArray, 0, bodyByteArray.Length);
        }
    }

    using (var stream = httpHelper.OpenRead())
    {
        using (var reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }
}

var parameters = new Dictionary<string, string>();
parameters["message"] = "Hi from HttpHelper";
parameters["access_token"] = "";
var result = PostSyncSample(parameters);
Console.WriteLine(result);
```