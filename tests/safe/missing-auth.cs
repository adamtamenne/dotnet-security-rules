using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class AdminController : Controller
{
    [Authorize(Roles = "Admin")]
    public IActionResult GetConfig()
    {
        return Ok(new { connectionString = "Server=db;Password=secret" });
    }
}
