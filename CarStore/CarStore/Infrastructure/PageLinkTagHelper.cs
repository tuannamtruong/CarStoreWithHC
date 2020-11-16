using CarStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace CarStore.Infrastructure
{
    // Looking for a div tag with page-model="..."
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;
        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        // page-model="..."
        public PagingInfo PageModel { get; set; }
        // page-action="..."
        public string PageAction { get; set; }
        public bool PageClassesEnabled { get; set; } = false;
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }
        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder divTag = new TagBuilder(tagName: "div");
            for (int i = 1; i <= PageModel.AmountPages; ++i)
            {
                TagBuilder hyperlinkTag = BuildAnchorTag(urlHelper, i);
                SetVisualForPageNumbers(i, hyperlinkTag);
                divTag.InnerHtml.AppendHtml(hyperlinkTag);
            }
            output.Content.AppendHtml(divTag.InnerHtml);
        }

        private TagBuilder BuildAnchorTag(IUrlHelper urlHelper, int i)
        {
            TagBuilder hyperlinkTag = new TagBuilder("a");
            PageUrlValues["currentPage"] = i;
            hyperlinkTag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
            hyperlinkTag.InnerHtml.Append(i.ToString());
            return hyperlinkTag;
        }
        private void SetVisualForPageNumbers(int pageNumber, TagBuilder hyperlinkTag)
        {
            if (PageClassesEnabled)
            {
                hyperlinkTag.AddCssClass(PageClass);
                hyperlinkTag.AddCssClass(pageNumber == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
            }
        }
    }
}
