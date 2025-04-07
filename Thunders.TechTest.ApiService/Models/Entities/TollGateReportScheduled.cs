namespace Thunders.TechTest.ApiService.Models.Entities;

public class TollGateReportScheduled
{
    public int TollGateReportScheduledId { get; set; }
    public string ReportType { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public string Parameters { get; set; } = string.Empty;
    public bool IsProcessed { get; set; }
    public DateTime? ProcessedDate { get; set; }
}
