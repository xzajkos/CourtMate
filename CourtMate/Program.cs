using CourtMate.Endpoints;
using CourtMate.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<LeagueStandingService>();
builder.Services.AddSingleton<LeagueService>();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapLeagueEndpoints();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<LeagueService>().SeedDemoData();
}

app.Run();
