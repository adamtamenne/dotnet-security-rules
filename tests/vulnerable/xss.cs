using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

public static class HtmlHelpers
{
    public static IHtmlContent RenderUserComment(IHtmlHelper html, string comment)
    {
        return html.Raw(comment);
    }
}
