using Lyrics.API;
using Lyrics.API.Controllers;
using Lyrics.Common.Interfaces;
using Lyrics.Logic.Services;
using Lyrics.Lyricsovh.Services;
using Lyrics.MusicBrainz.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<ILyricsService, LyricsService>();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();

// Add a HttpClient for MusicBrainz which has specific requirements for User-Agent to avoid throttling
// Request JSON rather than XML
builder.Services.AddHttpClient<IArtistService, ArtistService>((client) =>
{
    client.BaseAddress = new Uri(builder.Configuration["MusicBrainz:APIEndpoint"]);
    client.DefaultRequestHeaders.Add("User-Agent", $"LyricsAnalyser/{Assembly.GetAssembly(typeof(LyricsController)).GetName().Version} (${builder.Configuration["MusicBrainz:ContactEmail"]})");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Add a HttpClient for Lyricovh API. This service has a rate limiter on it but unsure of limits so you can configure these in appsettings.json
builder.Services.AddHttpClient<ILyricsService, LyricsService>((client) =>
{
    client.BaseAddress = new Uri(builder.Configuration["Lyricsovh:APIEndpoint"]);
})
.AddHttpMessageHandler(() => new OutboundHttpRequestLimiter(Convert.ToInt32(builder.Configuration["Lyricsovh:MaxConcurrentRequests"])));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
