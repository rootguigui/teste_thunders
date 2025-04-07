using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService.Data;
using Thunders.TechTest.ApiService.Interfaces.Repository;
using Thunders.TechTest.ApiService.Models.Entities;
using Thunders.TechTest.ApiService.Models.Response;

namespace Thunders.TechTest.ApiService.Repositories;

public class TollGateRepository : ITollGateRepository
{
    private readonly TollGateDbContext _context;

    public TollGateRepository(TollGateDbContext context)
    {
        _context = context;
    }

    public async Task SaveUsageAsync(TollGateUsage usage)
    {
        await _context.TollGateUsages.AddAsync(usage);
        await _context.SaveChangesAsync();
    }

    public async Task<List<HourlyValueReport>> GetHourlyValuesByCityAsync(string city, DateTime date)
    {
        var utcDate = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);
        var nextUtcDate = utcDate.AddDays(1);

        return await _context.TollGateUsages
            .Where(u => u.City == city && u.UsageDateTime >= utcDate && u.UsageDateTime < nextUtcDate)
            .GroupBy(u => u.UsageDateTime.Hour)
            .Select(g => new HourlyValueReport
            {
                Hour = g.Key,
                TotalValue = g.Sum(u => u.Amount),
                City = city
            })
            .OrderBy(r => r.Hour)
            .ToListAsync();
    }

    public async Task<List<TopTollGatesReport>> GetTopTollGatesByMonthAsync(int month, int year, int count)
    {
        return await _context.TollGateUsages
            .Where(u => u.UsageDateTime.Month == month && u.UsageDateTime.Year == year)
            .GroupBy(u => u.TollGate)
            .Select(g => new TopTollGatesReport
            {
                TollGate = g.Key,
                TotalRevenue = g.Sum(u => u.Amount),
                Month = month,
                Year = year
            })
            .OrderByDescending(r => r.TotalRevenue)
            .Take(count)
            .ToListAsync();
    }

    public async Task<VehicleTypeCountReport> GetVehicleTypeCountByTollGateAsync(string tollGate)
    {
        var usages = await _context.TollGateUsages
            .Where(u => u.TollGate == tollGate)
            .ToListAsync();

        return new VehicleTypeCountReport
        {
            TollGate = tollGate,
            MotorcycleCount = usages.Count(u => u.VehicleType == VehicleType.Motorcycle),
            CarCount = usages.Count(u => u.VehicleType == VehicleType.Car),
            TruckCount = usages.Count(u => u.VehicleType == VehicleType.Truck)
        };
    }
}