using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService.Data;
using Thunders.TechTest.ApiService.Interfaces.Repository;
using Thunders.TechTest.ApiService.Models.Entities;

namespace Thunders.TechTest.ApiService.Repositories;

public class TollGateReportScheduledRepository : ITollGateReportScheduledRepository
{
    private readonly TollGateDbContext _context;

    public TollGateReportScheduledRepository(TollGateDbContext context)
    {
        _context = context;
    }

    public async Task<TollGateReportScheduled> CreateReportScheduledAsync(TollGateReportScheduled reportScheduled)
    {
        await _context.TollGateReportScheduled.AddAsync(reportScheduled);
        await _context.SaveChangesAsync();
        return reportScheduled;
    }

    public async Task<List<TollGateReportScheduled>> GetReportScheduledAsync()
    {
        return await _context.TollGateReportScheduled.ToListAsync();
    }

    public async Task<TollGateReportScheduled> UpdateProgressAsync(int tollGateReportScheduledId)
    {
        var reportScheduled = await _context.TollGateReportScheduled.FindAsync(tollGateReportScheduledId);

        if (reportScheduled == null)
        {
            throw new Exception("Report scheduled not found");
        }

        reportScheduled.IsProcessed = true;
        reportScheduled.ProcessedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return reportScheduled;
    }
}