using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

public static class HtmlHelpers
{
    public static string RenderUserComment(string comment)
    {
        return System.Net.WebUtility.HtmlEncode(comment);
    }
}
