using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemory.Caching.Controllers;

[ApiController]
[Route("[controller]")]
public class ValuesController : ControllerBase
{
    private readonly IMemoryCache _memoryCache;

    public ValuesController(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    [HttpPost]
    public IActionResult SetName([FromBody] string name)
    {
        _memoryCache.Set("name", name);
        return Ok();
    }

    [HttpGet]
    public IActionResult GetName()
    {
        if (_memoryCache.TryGetValue("name", out string? name))
        {
            return Ok(name);
        }

        return BadRequest();
    }

    [HttpPost("[action]")]
    public IActionResult SetDate()
    {
        _memoryCache.Set<DateTime>("date", DateTime.Now, options: new()
        {
            AbsoluteExpiration = DateTime.Now.AddSeconds(30),
            SlidingExpiration = TimeSpan.FromSeconds(5)
        });
        return Ok();
    }

    [HttpGet("[action]")]
    public IActionResult GetDate()
    {
        if (_memoryCache.TryGetValue("date", out string? date))
        {
            return Ok(date);
        }

        return BadRequest();
    }
}