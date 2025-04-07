using Thunders.TechTest.ApiService.Models.Request;

namespace Thunders.TechTest.ApiService.Interfaces.Service;

public interface ITollGateService
{
    Task ProcessUsageAsync(CreateusageRequest usage);
}
