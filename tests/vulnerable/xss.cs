using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

public static class HtmlHelpers
{
    public static IHtmlContent RenderUserComment(IHtmlHelper html, string comment)
    {
        // ruleid: dotnet-xss-html-raw
        return html.Raw(comment);
    }

    public static IHtmlContent RenderUserBio(IHtmlHelper Html, string bio)
    {
        // ruleid: dotnet-xss-html-raw
        return Html.Raw("<p>" + bio + "</p>");
    }
}
