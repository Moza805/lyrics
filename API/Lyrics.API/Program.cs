using Lyrics.API.Controllers;
using Lyrics.Common.Interfaces;
using Lyrics.Lyricsovh.Services;
using Lyrics.MusicBrainz.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddScoped<IArtistService, ArtistService>((serviceProvider) =>
{
    var version = Assembly.GetAssembly(typeof(LyricsController)).GetName().Version;
    var contactEmail = builder.Configuration["MusicBrainz:ContactEmail"];

    return new ArtistService(
        "LyricsAnalyser",
        version,
        contactEmail,
        serviceProvider.GetService<HttpClient>()
    );
});
builder.Services.AddScoped<ILyricsService, LyricsService>();

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
