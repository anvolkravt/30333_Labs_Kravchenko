using Microsoft.AspNetCore.Razor.TagHelpers;

namespace _30333_Labs_Kravchenko.UI.TagHelpers
{
    //[HtmlTargetElement("img", Attributes = "img-action,img-controller")]
    //public class ImageTagHelper : TagHelper
    //{
    //    private readonly LinkGenerator _linkGenerator;

    //    [HtmlAttributeName("img-controller")]
    //    public string ImgController { get; set; } = null!;

    //    [HtmlAttributeName("img-action")]
    //    public string ImgAction { get; set; } = null!;

    //    public ImageTagHelper(LinkGenerator linkGenerator)
    //    {
    //        _linkGenerator = linkGenerator;
    //    }
    //    public override void Process(TagHelperContext context, TagHelperOutput output)
    //    {
    //        output.Attributes.SetAttribute("src", _linkGenerator.GetPathByAction(ImgAction, ImgController) ?? "#");
    //    }
    //}
    [HtmlTargetElement("img", Attributes = "img-action, img-controller")]
    public class ImageTagHelper(LinkGenerator linkGenerator) : TagHelper
    {
        public string ImgController { get; set; }
        public string ImgAction { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("src", linkGenerator.GetPathByAction(ImgAction, ImgController));
        }
    }
}