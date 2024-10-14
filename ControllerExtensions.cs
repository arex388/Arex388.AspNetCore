using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Mvc {
    public static class ControllerExtensions {
        /// <summary>
        /// Redirects to the Default action of the same controller that invoked the redirect. Clears the id route data value before redirecting.
        /// </summary>
        public static IActionResult RedirectToDefault(
            this Controller controller) {
            controller.RouteData.Values.Remove("id");

            return controller.RedirectToAction("Default");
        }

        /// <summary>
        /// Redirects to the Edit action of the same controller that invoked the redirect.
        /// </summary>
        public static IActionResult RedirectToEdit<T>(
            this Controller controller,
            T id) => controller.RedirectToAction("Edit", new {
                id
            });

        /// <summary>
        /// Redirects to the Gone action in the Default controller. Clears all route data values before redirecting.
        /// </summary>
        public static IActionResult RedirectToGone(
            this Controller controller) {
            controller.RouteData.Values.Clear();

            return controller.RedirectToAction("Gone", "Default");
        }

        /// <summary>
        /// Redirects to the request's referrer.
        /// </summary>
        public static IActionResult RedirectToReferrer(
            this Controller controller) => controller.Redirect(controller.Request.GetReferrer());
    }
}