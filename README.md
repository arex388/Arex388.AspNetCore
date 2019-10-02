# Arex388.AspNetCore

This is a small library with ASP.NET Core extensions that I use across my different projects. Below is a list of the available extensions and how to use them. I'd be happy if you found a use for the library, but be aware that I change it often to suit my needs, generally by adding new functionality, but sometimes with breaking changes.

With the release of .NET Core 3.0, I've decided to upgrade the library to it, and to change the version to 3.0.0 as well to match.

#### Basic Configuration

There is a extension method for `IServiceCollection` called `AddArex388()` which you can use for basic configuration to enable any of the following:

- `UseFeatures` to enable the `FeaturesViewLocationExpander`
- `UseIdentityProvider` to configure the `IdentityProvider` to be injectable
- `UseSimpleSlugifyParameterTransformer` to enable the `SimpleSlugifyParameterTransformer`

Here's the `Startup` code:

    public void ConfigureServices(
        IServiceCollection services) {
        services.AddArex388(
            o => {
                o.UseFeatures = true;
                o.UseIdentityProvider = true;
                o.UseSimpleSlugifyParameterTransformer = true;
            });
    }

#### AuthenticatedStaticFilesMiddleware

I am going to revisit this in the future because of the changes to authentication and authorization in ASP.NET Core 3.0.

<strike>

The `AuthenticatedStaticFilesMiddleware` is a "security" middleware to intercept requests to static files that should be used after a user is successfully authenticated. If the user is not authenticated, then it returns a 404 response.

I use it to block un-authenticated access to JavaScript and CSS files that may "bleed" available functionality to curious, but un-authenticated, users.

	public void ConfigureServices(
		IServiceCollection services) {
		services.Configure<AuthenticatedStaticFileOptions>(
			o => {
				o.Paths = new[] {
					"admin.min.js",
					"admin.min.css"
				};
			});
	}

	public void Configure(
		IApplicationBuilder app) {
		//  ...

		app.UseAuthentication();
		app.UseAuthenticatedStaticFiles();
		app.UseStaticFiles();

		//  ...
	}

</strike>

#### FeaturesViewLocationExpander

The `FeaturesViewLocationExpander` is a location expander for the Razor View Engine. It clears all currently registered location expanders, and adds itself. The expander follows the Features folders structure as described by Jimmy Bogard's Vertical Slices Architecture.

#### IdentityProvider

The `IdentityProvider` is a small helper class to access identity information for a currently authenticated user. By default it only provides access to the `UserId` (as an int) and `IsAuthenticated` properties.

You can extend it by inheriting from it, which gives you access to the `IHttpContextAccessor` from which you can then extract identity claims using the `GetValue()` extension method from `ClaimsPrincipal`.

    public sealed class MyIdentityProvider :
        IdentityProvider {
        public MyIdentityProvider(
            IHttpContextAccessor accessor) : base(accessor) {
        }

        public string UserLovesArex388 => Accessor.HttpContext.User.GetValue("LovesArex388");
    }

#### AntiFaviconMiddleware

The annoying, unsolicited, requests to `/favicon.ico` that browsers love to do has been, well, annoying me. Because it get's processed all the way up the stack in my case it's triggering double the amount of database queries for user details. I've added this middleware to just intercept it and return a 404 as soon as possible.

	public void Configure(
		IApplicationBuilder app) {
		app.UseStaticFiles();
		app.UseAntiFavicon();
	}

#### HtmlMinifierMiddleware

The `HtmlMinifierMiddleware` intercepts the response being returned and minifies the HTML using the HtmlAgilityPack. <strike>Ideally you should use it right before using Mvc.
</strike> The `UseMvc` extension is no longer relevant in ASP.NET Core 3.0, so I would recommend using it right after the call to `UseStaticFiles` and `UseAntiFavicon`.

	public void Configure(
		IApplicationBuilder app) {
		app.UseStaticFiles();
		app.UseAntiFavicon();
		app.UseHtmlMinifier();
	}

#### OneOffMiddleware

I'm starting to have misgivings about this, so until I can sit down and think about it clearly, I am removing it.

<strike>

The `OneOffMiddleware` is a "placeholder" middleware for triggering "one-off" tasks using the middleware pipeline.

I use it to trigger one or more Hangfire recurring jobs using the memory storage. This way I can avoid configuring full persistent storage such as SQL Server until I really need it. My simplest task is to run a self ping every five minutes.

There are two components. First you need to have a class that implements `IOneOffServices` and another class that inherits from `OneOffMiddlewareBase` and implements the `OneOffInvokeAsync()` method.

    public sealed class OneOffServices :
        IOneOffServices {
        public IMediator Mediator { get; }

        public OneOffServices(
            IMediator mediator) => Mediator = mediator;
    }

    public sealed class OneOffMiddleware :
        OneOffMiddlewareBase {
        public OneOffMiddleware(
            RequestDelegate next)
            : base(next) {
        }

        protected override async Task InvokeInternalAsync(
            IOneOffServices services) {
            if (!(services is OneOffServices oneOffServices)) {
                return;
            }

            var mediator = oneOffServices.Mediator;

            await mediator.Send(new KeepAlive.Command());
        }
    }

Then in the `Startup` class you need to register the `OneOffServices` as scoped, and register the middleware.

	public void ConfigureServices(
		IServiceCollection services) {
		services.AddScoped<IOneOffServices, OneOffServices>();
	}

    public void Configure(
		IApplicationBuilder app) {
		app.UseOneOff<OneOffMiddleware>();
	}

</strike>

#### SimpleSlugifyParameterTransformer

The `SimpleSlugifyParameterTransformer` is, as the name implies, a very simple `IOutboundParameterTransformer` that slugifies the action's name by taking it's camel cased version and kebaberizing it.

#### SitemapMiddleware

The `SitemapMiddleware` is a "placeholder" middleware for intercepting requests to "/sitemap.xml". It follows the same pattern as the `OneOffMiddleware`, just replace the `IOneOffServices` with `ISitemapServices`.

I usually pass in my `DbContext` instance and proceed to build out the XML that will be returned based on the needs of the application.

	public void Configure(
		IApplicationBuilder app) {
		app.UseSitemap<SitemapMiddleware>();
	}

#### TokenProvider

The `TokenProvider` is a small helper class for generating random strings. It is not secure by any means, but works fine for temporary passwords or validation tokens. I've also decided to make it a static class and add helper methods for the 3.0.0 update.

    var token = TokenProvider.Create(128);