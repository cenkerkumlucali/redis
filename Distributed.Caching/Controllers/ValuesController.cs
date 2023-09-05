using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class ValuesController : ControllerBase
{
    private readonly IDistributedCache _distributedCache;

    public ValuesController(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
    }

    [HttpPost("set-string")]
    public async Task<IActionResult> SetString([FromBody] string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return BadRequest("Name cannot be null or empty");
        }

        await _distributedCache.SetStringAsync("name", name, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
            SlidingExpiration = TimeSpan.FromSeconds(5)
        });

        return Ok();
    }

    [HttpPost("set-bytes")]
    public async Task<IActionResult> SetBytes([FromBody] string surname)
    {
        if (string.IsNullOrEmpty(surname))
        {
            return BadRequest("Surname cannot be null or empty");
        }

        var surnameBytes = Encoding.UTF8.GetBytes(surname);
        await _distributedCache.SetAsync("surname", surnameBytes, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
            SlidingExpiration = TimeSpan.FromSeconds(5)
        });

        return Ok();
    }

    [HttpGet("get-string")]
    public async Task<IActionResult> GetString()
    {
        var responseName = await _distributedCache.GetStringAsync("name");
        return Ok(responseName);
    }

    [HttpGet("get-bytes")]
    public async Task<IActionResult> GetBytes()
    {
        var surnameBytes = await _distributedCache.GetAsync("surname");
        var responseSurname = surnameBytes != null ? Encoding.UTF8.GetString(surnameBytes) : string.Empty;

        return Ok(responseSurname);
    }

    [HttpDelete("remove-string")]
    public async Task<IActionResult> RemoveString(string key)
    {
        await _distributedCache.RemoveAsync(key);
        return Ok();
    }

    [HttpDelete("remove-bytes")]
    public async Task<IActionResult> RemoveBytes(string key)
    {
        await _distributedCache.RemoveAsync(key);
        return Ok();
    }
}
