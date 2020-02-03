# Arex388.AspNetCore

This is an ASP.NET Core helper library targeting ASP.NET Core 3.1. It contains extensions and utilities I've need to use across my different ASP.NET Core projects. Since it is based on my needs, I do make changes to it from time to time, generally by adding new features or updating the existing ones, but sometimes by removing features. So it is a little volatile that way, but I'd be happy if you gave it a shot anyway.

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

#### FeaturesViewLocationExpander

The `FeaturesViewLocationExpander` is a location expander for the Razor View Engine. It clears all currently registered location expanders, and adds itself. The expander follows the Features folders structure as described by [Jimmy Bogard's Vertical Slices Architecture][0].

#### IdentityProvider

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

#### AntiFaviconMiddleware

The annoying, unsolicited, requests to `/favicon.ico` that browsers love to do has been, well, annoying me. Because it get's processed all the way up the stack in my case it's triggering double the amount of database queries for user details. I've added this middleware to just intercept it and return a 404 as soon as possible.

```C#
public void Configure(
	IApplicationBuilder app) {
	app.UseStaticFiles();
	app.UseAntiFavicon();
}
```

#### HtmlMinifierMiddleware

The `HtmlMinifierMiddleware` intercepts the response being returned and minifies the HTML using the [HtmlAgilityPack][1]. I recommend using it right after the call to `UseStaticFiles` and `UseAntiFavicon` (if you use it).

```c#
public void Configure(
	IApplicationBuilder app) {
	app.UseStaticFiles();
	app.UseAntiFavicon();
	app.UseHtmlMinifier();
}
```

#### SimpleSlugifyParameterTransformer

The `SimpleSlugifyParameterTransformer` is, as the name implies, a very simple `IOutboundParameterTransformer` that slugifies the action's name by taking it's camel cased version and kebaberizing it.

#### SitemapMiddleware

The `SitemapMiddleware` is a placeholder middleware for intercepting requests to "/sitemap.xml". There are two components to it. First you need to have a class that implements `ISitemapServices` and another class that inherits from `SitemapMiddlewareBase` and implements the `InvokeInternalAsync()` method.

I usually pass in my `DbContext` instance and proceed to build out the XML that will be returned based on the needs of the application.

```C#
public void Configure(
	IApplicationBuilder app) {
	app.UseSitemap<SitemapMiddleware>();
}
```

#### TokenProvider

The `TokenProvider` is a small helper class for generating random strings. It is not secure by any means, but works fine for temporary passwords or validation tokens.

```C#
var token = TokenProvider.Create(128);
```

[0]:https://jimmybogard.com/vertical-slice-architecture/
[1]:https://github.com/zzzprojects/html-agility-pack