using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;

namespace Arex388.AspNetCore {
    public sealed class FeaturesViewLocationExpander :
		IViewLocationExpander {
		//	{0} = action
		//	{1} = controller
		//	{2} = area
		public IEnumerable<string> ExpandViewLocations(
			ViewLocationExpanderContext context,
			IEnumerable<string> viewLocations) => new[] {
			"/Features/{2}/{1}/{0}.cshtml"
		};

		public void PopulateValues(
			ViewLocationExpanderContext context) {
		}
	}
}