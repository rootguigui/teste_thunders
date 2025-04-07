using System.Text.Json;
using Rebus.Handlers;
using Thunders.TechTest.ApiService.Interfaces.Repository;
using Thunders.TechTest.ApiService.Interfaces.Service;
using Thunders.TechTest.ApiService.Models.Entities;
using Thunders.TechTest.ApiService.Models.Request;

namespace Thunders.TechTest.ApiService.Handlers;

public class ReportRequestHandler : IHandleMessages<ReportRequest>
{
    private readonly IReportService _reportService;
    private readonly ITollGateReportRepository _tollGateReportRepository;
    private readonly ITollGateReportScheduledRepository _tollGateReportScheduledRepository;
    private readonly ILogger<ReportRequestHandler> _logger;

    public ReportRequestHandler(IReportService reportService, ITollGateReportRepository tollGateReportRepository, ITollGateReportScheduledRepository tollGateReportScheduledRepository, ILogger<ReportRequestHandler> logger)
    {
        _reportService = reportService;
        _tollGateReportRepository = tollGateReportRepository;
        _tollGateReportScheduledRepository = tollGateReportScheduledRepository;
        _logger = logger;
    }

    public async Task Handle(ReportRequest message)
    {
        try
        {
            _logger.LogInformation("Processando relatório do tipo {ReportType} agendado para {ScheduledDate}",
                message.ReportType, message.ScheduledDate);

            switch (message.ReportType.ToLower())
            {
                case "hourly-value":
                    message.Parameters.TryGetValue("city", out var city);
                    message.Parameters.TryGetValue("date", out var date);

                    _logger.LogInformation("Cidade: {City}, Data: {Date}", city, date);

                    if (city != null && date != null)
                    {
                        var content = await _reportService.GenerateHourlyValueReportAsync(
                            city.ToString()!,
                            DateTime.Parse(date.ToString()!));

                        _logger.LogInformation("Conteúdo: {Content}", content);

                        await _tollGateReportScheduledRepository.UpdateProgressAsync(message.TollGateReportScheduledId);

                        await _tollGateReportRepository.CreateReportAsync(new TollGateReport
                        {
                            TollGateReportScheduledId = message.TollGateReportScheduledId,
                            Content = JsonSerializer.Serialize(content),
                        });
                    }

                    break;

                case "top-tollgates":
                    message.Parameters.TryGetValue("month", out var month);
                    message.Parameters.TryGetValue("year", out var year);
                    message.Parameters.TryGetValue("count", out var count);

                    _logger.LogInformation("Mês: {Month}, Ano: {Year}, Quantidade: {Count}", month, year, count);

                    if (month != null && year != null && count != null)
                    {
                        var content = await _reportService.GenerateTopTollGatesReportAsync(
                            int.Parse(month.ToString()!),
                            int.Parse(year.ToString()!),
                            int.Parse(count.ToString()!));

                        _logger.LogInformation("Conteúdo: {Content}", content);

                        await _tollGateReportScheduledRepository.UpdateProgressAsync(message.TollGateReportScheduledId);

                        await _tollGateReportRepository.CreateReportAsync(new TollGateReport
                        {
                            TollGateReportScheduledId = message.TollGateReportScheduledId,
                            Content = JsonSerializer.Serialize(content)
                        });
                    }
                    break;

                case "vehicle-types":
                    message.Parameters.TryGetValue("tollGate", out var tollGate);

                    _logger.LogInformation("TollGate: {TollGate}", tollGate);

                    if (tollGate != null)
                    {
                        var content = await _reportService.GenerateVehicleTypeCountReportAsync(
                            tollGate.ToString()!);

                        _logger.LogInformation("Conteúdo: {Content}", content);

                        await _tollGateReportScheduledRepository.UpdateProgressAsync(message.TollGateReportScheduledId);

                        await _tollGateReportRepository.CreateReportAsync(new TollGateReport
                        {
                            TollGateReportScheduledId = message.TollGateReportScheduledId,
                            Content = JsonSerializer.Serialize(content)
                        });
                    }
                    break;

                default:
                    throw new ArgumentException($"Tipo de relatório desconhecido: {message.ReportType}");
            }

            _logger.LogInformation("Relatório processado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar relatório: {Message}", ex.Message);
            throw;
        }
    }
}