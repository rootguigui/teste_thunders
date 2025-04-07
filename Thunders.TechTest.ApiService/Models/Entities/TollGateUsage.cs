namespace Thunders.TechTest.ApiService.Models.Entities;

public class TollGateUsage
{
    public int TollGateUsageId { get; set; }
    public DateTime UsageDateTime { get; set; }
    public string TollGate { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public VehicleType VehicleType { get; set; }
}

public enum VehicleType
{
    Motorcycle,
    Car,
    Truck
}