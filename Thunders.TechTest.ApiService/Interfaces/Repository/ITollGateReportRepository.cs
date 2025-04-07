using Thunders.TechTest.ApiService.Models.Entities;

namespace Thunders.TechTest.ApiService.Interfaces.Repository;

public interface ITollGateReportRepository
{
    Task<TollGateReport> CreateReportAsync(TollGateReport report);
    Task<TollGateReport?> GetReportAsync(int tollGateReportScheduledId);
}
