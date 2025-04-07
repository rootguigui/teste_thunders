using Thunders.TechTest.ApiService.Models.Entities;
using Thunders.TechTest.ApiService.Models.Response;

namespace Thunders.TechTest.ApiService.Interfaces.Service;

public interface IReportService
{
    Task<List<HourlyValueReport>> GenerateHourlyValueReportAsync(string city, DateTime date);
    Task<List<TopTollGatesReport>> GenerateTopTollGatesReportAsync(int month, int year, int count);
    Task<VehicleTypeCountReport> GenerateVehicleTypeCountReportAsync(string tollGate);
    Task ScheduleReportAsync(string reportType, DateTime scheduledDate, Dictionary<string, object> parameters);
    Task<List<TollGateReportScheduled>> GetReportScheduledAsync();
    Task<TollGateReport?> GetReportAsync(int tollGateReportScheduledId);
}
