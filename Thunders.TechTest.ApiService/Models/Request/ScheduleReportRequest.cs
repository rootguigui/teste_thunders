namespace Thunders.TechTest.ApiService.Models.Request;

public class ScheduleReportRequest
{
    public string ReportType { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new();
}