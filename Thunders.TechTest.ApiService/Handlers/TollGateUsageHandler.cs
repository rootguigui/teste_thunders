using Rebus.Handlers;
using Thunders.TechTest.ApiService.Interfaces.Repository;
using Thunders.TechTest.ApiService.Models.Entities;

namespace Thunders.TechTest.ApiService.Handlers;

public class TollGateUsageHandler : IHandleMessages<TollGateUsage>
{
    private readonly ITollGateRepository _repository;
    private readonly ILogger<TollGateUsageHandler> _logger;

    public TollGateUsageHandler(ITollGateRepository repository, ILogger<TollGateUsageHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Handle(TollGateUsage message)
    {
        try
        {
            _logger.LogInformation("Processando uso do pedágio: {TollGate} em {City}", message.TollGate, message.City);
            await _repository.SaveUsageAsync(message);
            _logger.LogInformation("Uso do pedágio processado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao processar uso do pedágio: {ex.Message}");
            throw;
        }
    }
}