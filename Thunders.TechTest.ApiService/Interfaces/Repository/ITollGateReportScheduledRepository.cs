using Thunders.TechTest.ApiService.Models.Entities;

namespace Thunders.TechTest.ApiService.Interfaces.Repository;

public interface ITollGateReportScheduledRepository
{
    Task<TollGateReportScheduled> CreateReportScheduledAsync(TollGateReportScheduled reportScheduled);
    Task<List<TollGateReportScheduled>> GetReportScheduledAsync();
    Task<TollGateReportScheduled> UpdateProgressAsync(int tollGateReportScheduledId);
}
