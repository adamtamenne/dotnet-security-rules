using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

public static class HtmlHelpers
{
    public static string RenderUserComment(string comment)
    {
        // ok: dotnet-xss-html-raw
        return System.Net.WebUtility.HtmlEncode(comment);
    }

    public static IHtmlContent RenderDivider(IHtmlHelper html)
    {
        // ok: dotnet-xss-html-raw (literal markup, no user input)
        return html.Raw("<hr/>");
    }
}
