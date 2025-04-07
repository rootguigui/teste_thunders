using Thunders.TechTest.ApiService.Interfaces.Repository;
using Thunders.TechTest.ApiService.Interfaces.Service;
using Thunders.TechTest.ApiService.Models.Response;
using Thunders.TechTest.ApiService.Models.Request;
using Thunders.TechTest.OutOfBox.Queues;
using Thunders.TechTest.ApiService.Models.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Thunders.TechTest.ApiService.Services;

public class ReportService : IReportService
{
    private readonly ITollGateRepository _repository;
    private readonly IMessageSender _messageSender;
    private readonly ITollGateReportRepository _tollGateReportRepository;
    private readonly ITollGateReportScheduledRepository _tollGateReportScheduledRepository;
    private readonly IDistributedCache _cache;

    public ReportService
    (
        ITollGateRepository repository,
        IMessageSender messageSender,
        ITollGateReportRepository tollGateReportRepository,
        ITollGateReportScheduledRepository tollGateReportScheduledRepository,
        IDistributedCache cache
    )
    {
        _repository = repository;
        _messageSender = messageSender;
        _tollGateReportRepository = tollGateReportRepository;
        _tollGateReportScheduledRepository = tollGateReportScheduledRepository;
        _cache = cache;
    }

    public async Task<List<HourlyValueReport>> GenerateHourlyValueReportAsync(string city, DateTime date)
    {
        return await _repository.GetHourlyValuesByCityAsync(city, date);
    }

    public async Task<List<TopTollGatesReport>> GenerateTopTollGatesReportAsync(int month, int year, int count)
    {
        return await _repository.GetTopTollGatesByMonthAsync(month, year, count);
    }

    public async Task<VehicleTypeCountReport> GenerateVehicleTypeCountReportAsync(string tollGate)
    {
        return await _repository.GetVehicleTypeCountByTollGateAsync(tollGate);
    }

    public async Task<TollGateReport?> GetReportAsync(int tollGateReportScheduledId)
    {
        return await _tollGateReportRepository.GetReportAsync(tollGateReportScheduledId);
    }

    public async Task<List<TollGateReportScheduled>> GetReportScheduledAsync()
    {
        var cachedReports = await _cache.GetStringAsync("scheduled-reports");

        if (cachedReports != null)
        {
            return JsonSerializer.Deserialize<List<TollGateReportScheduled>>(cachedReports);
        }

        var scheduledReports = await _tollGateReportScheduledRepository.GetReportScheduledAsync();

        await _cache.SetStringAsync("scheduled-reports", JsonSerializer.Serialize(scheduledReports), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
        });

        return scheduledReports;
    }

    public async Task ScheduleReportAsync(string reportType, DateTime scheduledDate, Dictionary<string, object> parameters)
    {
        try
        {
            var reportScheduled = await _tollGateReportScheduledRepository.CreateReportScheduledAsync(new TollGateReportScheduled
            {
                ReportType = reportType,
                ScheduledDate = scheduledDate,
                Parameters = JsonSerializer.Serialize(parameters)
            });

            var cachedReports = await _cache.GetStringAsync("scheduled-reports");

            if (cachedReports != null)
            {
                var scheduledReports = JsonSerializer.Deserialize<List<TollGateReportScheduled>>(cachedReports) ?? new List<TollGateReportScheduled>();
                scheduledReports.Add(reportScheduled);

                await _cache.SetStringAsync("scheduled-reports", JsonSerializer.Serialize(scheduledReports), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
                });
            }

            await _messageSender.SendLocal(new ReportRequest
            {
                TollGateReportScheduledId = reportScheduled.TollGateReportScheduledId,
                ReportType = reportType,
                ScheduledDate = scheduledDate,
                Parameters = parameters
            });
        }
        catch (Exception ex)
        {
            throw new Exception("Error scheduling report", ex);
        }
    }
}