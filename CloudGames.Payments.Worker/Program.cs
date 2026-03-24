using CloudGames.Payments.Worker;
using Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<SqsService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
