using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Mvc {
    public static class ControllerExtensions {
        public static IActionResult RedirectToDefault(
            this Controller controller) => controller.RedirectToAction("Default");

        public static IActionResult RedirectToEdit<T>(
            this Controller controller,
            T id) => controller.RedirectToAction("Edit", new {
                id
            });

        public static IActionResult RedirectToGone(
            this Controller controller) => controller.RedirectToAction("Gone", "Default");

        public static IActionResult RedirectToReferrer(
            this Controller controller) => controller.Redirect(controller.Request.GetReferrer());
    }
}