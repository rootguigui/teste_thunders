namespace Thunders.TechTest.ApiService.Models.Entities;

public class TollGateReport
{
    public int TollGateReportId { get; set; }
    public int TollGateReportScheduledId { get; set; }
    public string Content { get; set; } = string.Empty;

    public virtual TollGateReportScheduled TollGateReportScheduled { get; set; } = default!;
}
