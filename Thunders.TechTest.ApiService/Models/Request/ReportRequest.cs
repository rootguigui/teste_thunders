namespace Thunders.TechTest.ApiService.Models.Request;

public class ReportRequest
{
    public int TollGateReportScheduledId { get; set; }
    public string ReportType { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new();
}