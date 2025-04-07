using Thunders.TechTest.ApiService.Models.Entities;

namespace Thunders.TechTest.ApiService.Models.Request;

public class CreateusageRequest
{
    public string TollGateUsage { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public VehicleType VehicleType { get; set; }
    public decimal Amount { get; set; }
}
