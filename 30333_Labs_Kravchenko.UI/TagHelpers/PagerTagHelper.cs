using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Diagnostics;

namespace _30333_Labs_Kravchenko.UI.TagHelpers
{
    [HtmlTargetElement("pager")]
    public class PagerTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public PagerTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
            Debug.WriteLine("PagerTagHelper initialized");
        }

        [HtmlAttributeName("current-page")]
        public int CurrentPage { get; set; }

        [HtmlAttributeName("total-pages")]
        public int TotalPages { get; set; }

        [HtmlAttributeName("category")]
        public string? Category { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; } = null!;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            Debug.WriteLine($"PagerTagHelper Process called: CurrentPage={CurrentPage}, TotalPages={TotalPages}, Category={Category}");
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "row mt-4 mb-3");

            var nav = new TagBuilder("nav");
            nav.Attributes["aria-label"] = "Page navigation example";
            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination justify-content-center");

            var prevLi = new TagBuilder("li");
            prevLi.AddCssClass("page-item");
            if (CurrentPage == 1)
                prevLi.AddCssClass("disabled");
            var prevA = new TagBuilder("a");
            prevA.AddCssClass("page-link");
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            prevA.Attributes["href"] = urlHelper.Action("Index", "Product", new { category = Category, pageno = CurrentPage == 1 ? 1 : CurrentPage - 1 });
            prevA.Attributes["aria-label"] = "Previous";
            prevA.InnerHtml.AppendHtml("<span aria-hidden=\"true\">«</span>");
            prevLi.InnerHtml.AppendHtml(prevA);
            ul.InnerHtml.AppendHtml(prevLi);

            for (int i = 1; i <= TotalPages; i++)
            {
                var li = new TagBuilder("li");
                li.AddCssClass("page-item");
                if (i == CurrentPage)
                    li.AddCssClass("active");
                var a = new TagBuilder("a");
                a.AddCssClass("page-link");
                a.Attributes["href"] = urlHelper.Action("Index", "Product", new { category = Category, pageno = i });
                a.InnerHtml.Append(i.ToString());
                li.InnerHtml.AppendHtml(a);
                ul.InnerHtml.AppendHtml(li);
            }

            var nextLi = new TagBuilder("li");
            nextLi.AddCssClass("page-item");
            if (CurrentPage == TotalPages)
                nextLi.AddCssClass("disabled");
            var nextA = new TagBuilder("a");
            nextA.AddCssClass("page-link");
            nextA.Attributes["href"] = urlHelper.Action("Index", "Product", new { category = Category, pageno = CurrentPage == TotalPages ? TotalPages : CurrentPage + 1 });
            nextA.Attributes["aria-label"] = "Next";
            nextA.InnerHtml.AppendHtml("<span aria-hidden=\"true\">»</span>");
            nextLi.InnerHtml.AppendHtml(nextA);
            ul.InnerHtml.AppendHtml(nextLi);

            nav.InnerHtml.AppendHtml(ul);
            output.Content.AppendHtml(nav);
            Debug.WriteLine("PagerTagHelper rendered");
        }
    }
}