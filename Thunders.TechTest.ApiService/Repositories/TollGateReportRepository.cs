using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService.Data;
using Thunders.TechTest.ApiService.Interfaces.Repository;
using Thunders.TechTest.ApiService.Models.Entities;

namespace Thunders.TechTest.ApiService.Repositories;

public class TollGateReportRepository : ITollGateReportRepository
{
    private readonly TollGateDbContext _context;

    public TollGateReportRepository(TollGateDbContext context)
    {
        _context = context;
    }

    public async Task<TollGateReport> CreateReportAsync(TollGateReport report)
    {
        await _context.TollGateReports.AddAsync(report);
        await _context.SaveChangesAsync();
        return report;
    }

    public async Task<TollGateReport?> GetReportAsync(int tollGateReportScheduledId)
    {
        return await _context.TollGateReports
            .Include(t => t.TollGateReportScheduled)
            .Where(t => t.TollGateReportScheduledId == tollGateReportScheduledId).FirstOrDefaultAsync();
    }
}
