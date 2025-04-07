var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache", 6379);

var rabbitMqPassword = builder.AddParameter("RabbitMqPassword", true);
var rabbitMq = builder.AddRabbitMQ("RabbitMq", password: rabbitMqPassword)
    .WithDataVolume()
    .WithVolume("/etc/rabbitmq")
    .WithManagementPlugin();

var postgresPassword = builder.AddParameter("PostgresPassword", true);
var postgres = builder.AddPostgres("postgres", password: postgresPassword)
    .WithDataVolume()
    .AddDatabase("ThundersTechTestDb");

var apiService = builder.AddProject<Projects.Thunders_TechTest_ApiService>("apiservice")
    .WithReference(cache)
    .WithReference(rabbitMq)
    .WaitFor(rabbitMq)
    .WithReference(postgres)
    .WaitFor(postgres);

builder.Build().Run();
