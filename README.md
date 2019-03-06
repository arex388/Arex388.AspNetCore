# Arex388.AspNetCore

This is a small library with ASP.NET Core extensions that I use across my different projects. Below is a list of the available extensions and how to use them. I'd be happy if you found a use for the library, but be aware that I change it often to suit my needs, generally by adding new functionality.

#### AuthenticatedStaticFilesMiddleware

The AutheticatedStaticFiles middleware is a "security" middleware that intersepts requests to static files and checks if the user has been authorized to access the app. If the user is not authorized, then return a 404 response.

	public void ConfigureServices(
		IServiceCollection services) {
		services.Configure<AuthenticatedStaticFileOptions>(
			o => {
				o.Paths = new List<string> {
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

#### FeaturesViewLocationExpander

A view location expander that follows the Vertical slices architecture style by Jimmy Bogard.

	public void ConfigureServices(
		IServiceCollection services) {
		services.AddFeatures();
	}

#### HtmlMinifierMiddleware

A middleware to minify HTML after the Razor view has been rendered using the HtmlAgilityPack.

	public void Configure(
		IApplicationBuilder app) {
		//  ...

		app.UseHtmlMinifier();
		app.UseMvc(...);

		//  ...
	}

#### SimpleSlugifyParameterTransformer

A parameter transformer to slugify a route value. It is very simple in that it assumes that action names are camel cased, which it then kebaberizes.

	public void ConfigureServices(
		IServiceCollection services) {
		services.Configure<RouteOptions>()
			.ConfigureSimpleSlugifyParameterTransformer();
	}

#### SitemapMiddleware

A placeholder middleware to intercept a request for a sitemap and generate one. You will have to implement your own concrete `SitemapMiddleware` with your own logic for generating the sitemap. Every app is different so your way of generating the sitemap will vary depending on your data sources.

	public void Configure(
		IApplicationBuilder app) {
		//  ...

		app.UseSitemap<SitemapMiddleware>();

		//  ...
	}