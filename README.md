# Arex388.AspNetCore

This is an ASP.NET Core helper library targeting ASP.NET Core 5.0. It contains extensions and utilities I've needed to use across my different ASP.NET Core projects. Since it is based on my needs it is opinionated and I do make changes to it from time to time, generally by adding new features or updating the existing ones, but sometimes by removing features. So it is a little volatile that way, but I'd be happy if you gave it a shot anyway.

- Extensions
  - [ClaimsPrincipal](#claimsprincipal)
  - [Controller](#controller)
  - [Enumerable](#enumeralb)
  - [HttpRequest](#httprequest)
  - [ModelStateDictionary](#modelstatedictionary)
  - [String](#string)
- Middlewares
  - [Html Minifier](#html-minifier)
  - [Pre-Compressed Static Files](#pre-compressed-staticf-iles)
  - [Sitemap](#sitemap)
- Miscellaneous
  - [Features ViewLocationExpander](#features-viewlocationexpander)
  - [Identity Provider](#identity-provider)
  - [Simple Slugify ParameterTransformer](#simple-slugify-parametertransformer)
  - [Token Provider](#token-provider)



#### Basic Configuration

There is a extension method for `IServiceCollection` called `AddArex388()` which you can use for basic configuration to enable any of the following:

- `UseFeatures` to enable the `FeaturesViewLocationExpander`
- `UseIdentityProvider` to configure the `IdentityProvider` to be injectable
- `UseSimpleSlugifyParameterTransformer` to enable the `SimpleSlugifyParameterTransformer`

Here's the `Startup` code:

```C#
public void ConfigureServices(
    IServiceCollection services) {
    services.AddArex388(
        o => {
            o.UseFeatures = true;
            o.UseIdentityProvider = true;
            o.UseSimpleSlugifyParameterTransformer = true;
        });
}
```



## Extensions

The `Controller` and `HttpRequest` classes have been extended with the following extensions:



#### ClaimsPrincipal

- `GetUserId()` - returns the user's id claim value as an `int`.
- `GetValue(string claimType)` - returns a claim value as a string.



#### Controller

> **NOTE:** These extensions are highly opinionated about the controller and action structure of the app.

- `RedirectToDefault()` - returns a `RedirectToAction()` result for the *Default* view.
- `RedirectToEdit<T>(T id)` - returns a `RedirectToAction()` result for the *Edit* view passing in the `id`.
- `RedirectToGone()` - returns a `RedirectToAction()` result for the *Gone* view.
- `RedirectToReferrer()` - returns a `Redirect()` result with the request's referrer.



#### Enumerable

- `StringJoin<T>(string? separator)` - returns a concatenated string of an enumerable with the specified separator.



#### HttpRequest

- `GetReferrer()` - returns the request's referrer from the referrer header, if any.



#### ModelStateDictionary

- `GetDictionary()` - returns an `IDictionary<string, string>` containing any model errors.



#### String

- `HasValue()` - returns true or false if the string **not** null or empty.



## Middlewares

The following middlewares are available.



#### Html Minifier

The `HtmlMinifierMiddleware` intercepts the response being returned and minifies the HTML using the [HtmlAgilityPack](https://github.com/zzzprojects/html-agility-pack). I recommend using it right after the call to `UseStaticFiles`.

```c#
public void Configure(
	IApplicationBuilder app) {
	app.UseStaticFiles();
	app.UseHtmlMinifier();
}
```



#### Pre-Compressed Static Files

Using Gulp I always have my CSS and JS files bundled into a single minified file. That works great, but it always felt like it wasn't complete. I really wanted to get Gzip and Brottli versions of the minified file and try to serve those first before falling back to the plain minified version. After a bit of tinkering I got Gulp to generate those as well, but I needed a way to intercept the call to the minified file and return either the Gzip or Brottli versions.

The `PreCompressedStaticFilesMiddleware` was born out of that need. It intercepts requests to CSS and JS files and, based on the accept encoding and existence of the files, will return either the Gzip or Brottli versions or fallback to the minified version.

```c#
public void Configure(
	IApplicationBuilder app) {
	app.UsePreCompressedStaticFiles();
	app.UseStaticFiles();
}
```

You must also add the following to your `web.config` to make it all work.

```xml

<system.webServer>
	<httpCompression>
		<staticTypes>
			<add mimeType="text/css" enabled="false"/>
			<add mimeType="text/javascript" enabled="false"/>
		</staticTypes>
	</httpCompression>
</system.webServer>
```



#### Sitemap

The `SitemapMiddleware` is a placeholder middleware for intercepting requests to `/sitemap.xml`. There are two components to it. First you need to have a class that implements `ISitemapServices` and another class that inherits from `SitemapMiddlewareBase` and implements the `InvokeInternalAsync()` method.

I usually pass in my `DbContext` instance and proceed to build out the XML that will be returned based on the needs of the application.

```C#
public void Configure(
	IApplicationBuilder app) {
	app.UseSitemap<SitemapMiddleware>();
}
```



## Miscellaneous

The following miscellaneous features are available.



#### Features ViewLocationExpander

The `FeaturesViewLocationExpander` is a location expander for the Razor View Engine. It clears all currently registered location expanders, and adds itself. The expander follows the Features folders structure as described by [Jimmy Bogard's Vertical Slices Architecture](https://jimmybogard.com/vertical-slice-architecture/).



#### Identity Provider

The `IdentityProvider` is a small helper class to access identity information for a currently authenticated user. By default it only provides access to the `UserId` (as an `int`) and `IsAuthenticated` properties.

You can extend it by inheriting from it, which gives you access to the `IHttpContextAccessor` from which you can then extract identity claims using the `GetValue()` extension method from `ClaimsPrincipal`:

```C#
public sealed class MyIdentityProvider :
    IdentityProvider {
    public MyIdentityProvider(
        IHttpContextAccessor accessor) : base(accessor) {
    }

    public string UserLovesArex388 => Accessor.HttpContext.User.GetValue("LovesArex388");
}
```



#### Simple Slugify ParameterTransformer

The `SimpleSlugifyParameterTransformer` is, as the name implies, a very simple `IOutboundParameterTransformer` that slugifies the action's name by taking it's camel cased version and kebaberizing it.



#### Token Provider

The `TokenProvider` is a small helper class for generating random strings. It is not secure by any means, but works fine for temporary passwords or validation tokens.

```C#
var token = TokenProvider.Create(128);
```
