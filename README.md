# Arex388.AspNetCore

This is an ASP.NET Core helper library targeting ASP.NET Core 3.1. It contains extensions and utilities I've need to use across my different ASP.NET Core projects. Since it is based on my needs, I do make changes to it from time to time, generally by adding new features or updating the existing ones, but sometimes by removing features. So it is a little volatile that way, but I'd be happy if you gave it a shot anyway.

- [FeaturesViewLocationExpander](#featuresviewlocationexpander)
- [HtmlMinifierMiddleware](#htmlminifiermiddleware)
- [IdentityProvider](#identityprovider)
- [PreCompressedStaticFilesMiddleware](#precompressedstaticfilesmiddleware)
- [SimpleSlugifyParameterTransformer](#simpleslugifyparametertransformer)
- [SitemapMiddleware](#sitemapmiddleware)
- [TokenProvider](#tokenprovider)

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

The `FeaturesViewLocationExpander` is a location expander for the Razor View Engine. It clears all currently registered location expanders, and adds itself. The expander follows the Features folders structure as described by [Jimmy Bogard's Vertical Slices Architecture](https://jimmybogard.com/vertical-slice-architecture/).

#### HtmlMinifierMiddleware

The `HtmlMinifierMiddleware` intercepts the response being returned and minifies the HTML using the [HtmlAgilityPack](https://github.com/zzzprojects/html-agility-pack). I recommend using it right after the call to `UseStaticFiles` and `UseAntiFavicon` (if you use it).

```c#
public void Configure(
	IApplicationBuilder app) {
	app.UseStaticFiles();
	app.UseAntiFavicon();
	app.UseHtmlMinifier();
}
```

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

#### PreCompressedStaticFilesMiddleware

Using Gulp I always had my CSS and JS files bundled into a single minified file. That worked great, but it always felt like it wasn't complete. I really wanted to get Gzip and Brottli versions of the minified file and try to serve those first before falling back to the plain minified version. After a bit of tinkering I got Gulp to generate them as well, but I needed a way to intercept the call to the minified file and return either the Gzip or Brottli versions.

The `PreCompressedStaticFilesMiddleware` was born out of that need. It intercepts requests to CSS and JS files and based on the accept encoding, and existance of the files, will return either the Gzip or Brottli versions or fallback to the minified version.

```c#
public void Configure(
	IApplicationBuilder app) {
	app.UsePreCompressedStaticFiles();
	app.UseStaticFiles();
}
```

#### SimpleSlugifyParameterTransformer

The `SimpleSlugifyParameterTransformer` is, as the name implies, a very simple `IOutboundParameterTransformer` that slugifies the action's name by taking it's camel cased version and kebaberizing it.

#### SitemapMiddleware

The `SitemapMiddleware` is a placeholder middleware for intercepting requests to `/sitemap.xml`. There are two components to it. First you need to have a class that implements `ISitemapServices` and another class that inherits from `SitemapMiddlewareBase` and implements the `InvokeInternalAsync()` method.

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
