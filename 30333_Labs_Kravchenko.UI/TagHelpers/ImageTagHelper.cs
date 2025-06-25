using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Diagnostics;

namespace _30333_Labs_Kravchenko.UI.TagHelpers
{
    [HtmlTargetElement("img", Attributes = "img-action, img-controller")]
    public class ImageTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public ImageTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory ?? throw new ArgumentNullException(nameof(urlHelperFactory));
            Debug.WriteLine("ImageTagHelper initialized");
        }

        [HtmlAttributeName("img-action")]
        public string? Action { get; set; }

        [HtmlAttributeName("img-controller")]
        public string? Controller { get; set; }

        [HtmlAttributeName("img-route-id")]
        public string? RouteId { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; } = null!;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            try
            {
                Debug.WriteLine($"ImageTagHelper Process called: Controller={Controller}, Action={Action}, RouteId={RouteId}");

                if (string.IsNullOrEmpty(Action) || string.IsNullOrEmpty(Controller))
                {
                    Debug.WriteLine("Missing img-action or img-controller, setting default src");
                    output.Attributes.SetAttribute("src", "/images/no-image.jpg");
                    output.Attributes.RemoveAll("img-action");
                    output.Attributes.RemoveAll("img-controller");
                    output.Attributes.RemoveAll("img-route-id");
                    return;
                }

                if (ViewContext == null)
                {
                    Debug.WriteLine("ViewContext is null, setting default src");
                    output.Attributes.SetAttribute("src", "/images/no-image.jpg");
                    output.Attributes.RemoveAll("img-action");
                    output.Attributes.RemoveAll("img-controller");
                    output.Attributes.RemoveAll("img-route-id");
                    return;
                }

                var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
                string url;
                if (!string.IsNullOrEmpty(RouteId))
                {
                    url = urlHelper.Action(Action, Controller, new { id = RouteId });
                }
                else
                {
                    url = urlHelper.Action(Action, Controller);
                }

                Debug.WriteLine($"Generated URL: {url ?? "null"}");
                output.Attributes.SetAttribute("src", url ?? "/images/no-image.jpg");
                output.Attributes.RemoveAll("img-action");
                output.Attributes.RemoveAll("img-controller");
                output.Attributes.RemoveAll("img-route-id");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ImageTagHelper error: {ex.Message}");
                output.Attributes.SetAttribute("src", "/images/no-image.jpg");
            }
        }
    }
}