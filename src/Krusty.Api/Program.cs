using Krusty.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentationServices();

var app = builder.Build();

app.UsePresentationServices();

app.Run();
