using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class AdminController : Controller
{
    [AllowAnonymous]
    public IActionResult GetConfig()
    {
        return Ok(new { connectionString = "Server=db;Password=secret" });
    }
}
