namespace Thunders.TechTest.ApiService.Models.Response;

public class HourlyValueReport
{
    public int Hour { get; set; }
    public decimal TotalValue { get; set; }
    public string City { get; set; } = string.Empty;
}

public class TopTollGatesReport
{
    public string TollGate { get; set; } = string.Empty;
    public decimal TotalRevenue { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}

public class VehicleTypeCountReport
{
    public string TollGate { get; set; } = string.Empty;
    public int MotorcycleCount { get; set; }
    public int CarCount { get; set; }
    public int TruckCount { get; set; }
}