# FluentHttp

## Overview
FluentHttp is a light weight library aimed to ease the development for your rest client with
consistent api throughout different frameworks whether you are using .net 4.0 or silverlight or
window phone. (It is not aimed to be used directly by the developers, but rather for creating 
a rest client wrapper, such as for Facebook, Github, Twitter, Google etc.)

## Supported Frameworks

* .NET 4.0 (client profile supported) ~50kb
* .NET 3.5 (client profile supported) ~50kb
* .NET 2.0 ~50kb
* Silverlight 4.0  ~30kb
* Windows Phone 7  ~30kb
* Mono

## Getting Started
Reference appropriate FluentHttp.dll depending on your framework.

## Samples
Here are some samples to get started with on FluentHttp.

### Using async callbacks

	private static void GetAsync()
	{
		// Stream to save the response to
		var responseSaveStream = new MemoryStream();

		// Prepare the request.
		var request = new FluentHttpRequest()
			.BaseUrl("https://graph.facebook.com")
			.ResourcePath("/4")
			.Method("GET")
			.Headers(h => h.Add("User-Agent", "FluentHttp"))
			.QueryStrings(q => q
									.Add("fields", "name,first_name,last_name")
									.Add("format", "json"))
			.Proxy(WebRequest.DefaultWebProxy)
			.OnResponseHeadersReceived((o, e) => e.SaveResponseIn(responseSaveStream));

		request.ExecuteAsync(ar =>
                                {
                                    var response = ar.Response;

                                    // seek the save stream to beginning.
                                    response.SaveStream.Seek(0, SeekOrigin.Begin);

                                    // Print the response
                                    Console.WriteLine("GetAsync: ");
                                    Console.WriteLine(FluentHttpRequest.ToString(response.SaveStream));
                                });
	}

### Using with Task Parallel Library (TPL)
Note: supported only in .NET 4.0

	private static void GetAsyncWithTask()
	{
		// Stream to save the response to
		var responseSaveStream = new MemoryStream();

		// Prepare the request.
		var request = new FluentHttpRequest()
			.BaseUrl("https://graph.facebook.com")
			.ResourcePath("/4")
			.Method("GET")
			.Headers(h => h.Add("User-Agent", "FluentHttp"))
			.QueryStrings(q => q
									.Add("fields", "name,first_name,last_name")
									.Add("format", "json"))
			.Proxy(WebRequest.DefaultWebProxy)
			.OnResponseHeadersReceived((o, e) => e.SaveResponseIn(responseSaveStream));

		var task = request.ExecuteTaskAsync();

		task.ContinueWith(
			t =>
			{
				var ar = t.Result;
				var response = ar.Response;

				// seek the save stream to beginning.
				response.SaveStream.Seek(0, SeekOrigin.Begin);

				// Print the response
				Console.WriteLine("GetAsyncWithTask: ");
				Console.WriteLine(FluentHttpRequest.ToString(response.SaveStream));
			});
	}

### Making synchronous requests
Unlike most of the rest libraries, FluentHttp only supports async web requests. 
But there is a fake synchronous method which allows you to make async method synchronous.

    var ar = request.Execute();

## Using Authenticators
To simplify authentication for http web requests, FluentHttp source includes some
authenticators. (These authenticators are not part of the core FluentHttp.dll and must
be included manually which can be found [here](https://github.com/prabirshrestha/FluentHttp/tree/master/src/FluentHttp.Tests/FluentAuthenticators).)

	var request = new FluentHttpRequest()
		.BaseUrl("https://graph.facebook.com")
		.ResourcePath("/me")
		.Method("GET")
		.AuthenticateUsing(new OAuth2UriQueryPrameterAuthenticator(AccessToken))
		.OnResponseHeadersReceived((o, e) => e.SaveResponseIn(responseSaveStream));

## License
FluentHttp is intended to be used in both open-source and commercial environments.

FluentHttp is licensed under Apache License 2.0.
