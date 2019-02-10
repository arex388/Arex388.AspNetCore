using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Arex388.AspNetCore.Mvc.Razor {
	public sealed class FeaturesViewLocationExpander :
		IViewLocationExpander {
		public IEnumerable<string> ExpandViewLocations(
			ViewLocationExpanderContext context,
			IEnumerable<string> viewLocations) {
			//	{0} = action
			//	{1} = controller
			//	{2} = area

			return new[] {
				"/Features/{2}/{1}/{0}.cshtml"
			};
		}

		public void PopulateValues(
			ViewLocationExpanderContext context) {
		}
	}
}