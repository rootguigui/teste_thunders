using Thunders.TechTest.ApiService;
using Thunders.TechTest.ApiService.Data;
using Thunders.TechTest.ApiService.Interfaces.Repository;
using Thunders.TechTest.ApiService.Interfaces.Service;
using Thunders.TechTest.ApiService.Repositories;
using Thunders.TechTest.ApiService.Services;
using Thunders.TechTest.OutOfBox.Database;
using Thunders.TechTest.OutOfBox.Queues;
using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService.Models.Entities;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Thunders.TechTest.OutOfBox.Cache;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();


builder.Services.AddControllers();

var features = Features.BindFromConfiguration(builder.Configuration);

// Add services to the container.
builder.Services.AddProblemDetails();

if (features.UseCache)
{
    builder.Services.AddCache(builder.Configuration);
}

if (features.UseMessageBroker)
{
    var subscriptionBuilder = new SubscriptionBuilder();
    subscriptionBuilder.Add<TollGateUsage>();
    builder.Services.AddBus(builder.Configuration, subscriptionBuilder);
}

if (features.UseEntityFramework)
{
    builder.Services.AddNpgsqlDbContext<TollGateDbContext>(builder.Configuration);
}

if (features.UseSwagger)
{
    builder.Services.AddSwaggerGen();
}

builder.Services.AddTransient<IReportService, ReportService>();
builder.Services.AddTransient<ITollGateRepository, TollGateRepository>();
builder.Services.AddTransient<ITollGateReportRepository, TollGateReportRepository>();
builder.Services.AddTransient<ITollGateReportScheduledRepository, TollGateReportScheduledRepository>();
builder.Services.AddScoped<IMessageSender, RebusMessageSender>();
builder.Services.AddTransient<ITollGateService, TollGateService>();

var app = builder.Build();

app.UseExceptionHandler();
app.MapDefaultEndpoints();

if (features.UseSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

if (features.UseEntityFramework)
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<TollGateDbContext>();
        dbContext.Database.Migrate();
    }
}

app.Run();
