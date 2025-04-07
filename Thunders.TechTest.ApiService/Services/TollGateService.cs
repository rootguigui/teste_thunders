using Thunders.TechTest.ApiService.Interfaces.Service;
using Thunders.TechTest.ApiService.Models.Request;
using Thunders.TechTest.ApiService.Models.Entities;
using Thunders.TechTest.ApiService.Interfaces.Repository;
using Thunders.TechTest.OutOfBox.Queues;

namespace Thunders.TechTest.ApiService.Services;

public class TollGateService : ITollGateService
{
    private readonly IMessageSender _messageSender;

    public TollGateService(IMessageSender messageSender)
    {
        _messageSender = messageSender;
    }

    public async Task ProcessUsageAsync(CreateusageRequest usage)
    {
        try
        {
            var message = new TollGateUsage
            {
                TollGate = usage.TollGateUsage,
                City = usage.City,
                VehicleType = usage.VehicleType,
                Amount = usage.Amount,
                UsageDateTime = DateTime.UtcNow.AddHours(-3),
            };

            await _messageSender.SendLocal(message);
        }
        catch (Exception)
        {
            throw new Exception("Erro ao processar uso");
        }
    }

}
