using Thunders.TechTest.ApiService.Models.Entities;
using Thunders.TechTest.ApiService.Models.Response;

namespace Thunders.TechTest.ApiService.Interfaces.Repository;

public interface ITollGateRepository
{
    Task SaveUsageAsync(TollGateUsage usage);
    Task<List<HourlyValueReport>> GetHourlyValuesByCityAsync(string city, DateTime date);
    Task<List<TopTollGatesReport>> GetTopTollGatesByMonthAsync(int month, int year, int count);
    Task<VehicleTypeCountReport> GetVehicleTypeCountByTollGateAsync(string tollGate);
}