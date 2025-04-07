using Microsoft.AspNetCore.Mvc;
using Thunders.TechTest.ApiService.Interfaces.Service;
using Thunders.TechTest.ApiService.Models.Response;
using Thunders.TechTest.ApiService.Models.Request;
using Thunders.TechTest.ApiService.Models.Entities;

namespace Thunders.TechTest.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TollGateReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public TollGateReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpPost("tollgate-reports/schedule")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ScheduleReport([FromBody] ScheduleReportRequest request)
    {
        try
        {
            if (request == null) return BadRequest();

            await _reportService.ScheduleReportAsync(request.ReportType, request.ScheduledDate, request.Parameters);

            return Accepted();
        }
        catch (Exception)
        {
            return BadRequest("Erro ao agendar relatório");
        }
    }

    [HttpGet("tollgate-reports/report-scheduled")]
    [ProducesResponseType(typeof(List<TollGateReportScheduled>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetReportScheduled()
    {
        var report = await _reportService.GetReportScheduledAsync();
        return Ok(report);
    }

    [HttpGet("tollgate-reports/report-scheduled/{tollGateReportScheduledId}")]
    [ProducesResponseType(typeof(TollGateReport), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetReportScheduled(int tollGateReportScheduledId)
    {
        var report = await _reportService.GetReportAsync(tollGateReportScheduledId);
        return Ok(report);
    }
}
