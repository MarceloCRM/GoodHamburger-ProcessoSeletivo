using GoodHamburger.Data;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ItemsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetItems()
    {
        var items = _context.Items.ToList();
        return Ok(items);
    }
}