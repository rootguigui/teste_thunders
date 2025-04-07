using Microsoft.AspNetCore.Mvc;
using Thunders.TechTest.ApiService.Models;
using Thunders.TechTest.ApiService.Interfaces.Service;
using Thunders.TechTest.ApiService.Models.Request;

namespace Thunders.TechTest.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TollGateController : ControllerBase
{
    private readonly ITollGateService _tollGateService;

    public TollGateController(ITollGateService tollGateService)
    {
        _tollGateService = tollGateService;
    }

    [HttpPost("usage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterUsage([FromBody] CreateusageRequest usage)
    {
        if (usage == null) return BadRequest();

        await _tollGateService.ProcessUsageAsync(usage);
        return Ok();
    }
}