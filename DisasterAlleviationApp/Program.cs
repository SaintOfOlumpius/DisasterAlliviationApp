using DisasterAlleviationApp;
using DisasterAlleviationApp.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure MongoDB
var databaseSettings = builder.Configuration
    .GetSection("DisasterAlleviationDatabaseSettings")
    .Get<DisasterAlleviationDatabaseSettings>();

builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    return new MongoClient(databaseSettings?.ConnectionString ?? "mongodb://localhost:27017");
});

builder.Services.AddSingleton<IMongoDatabase>(serviceProvider =>
{
    var client = serviceProvider.GetRequiredService<IMongoClient>();
    var dbName = databaseSettings?.DatabaseName ?? "DisasterAlleviationDB";
    return client.GetDatabase(dbName);
});

// Register services
builder.Services.AddScoped<IDonationService, DonationService>();
builder.Services.AddScoped<IBeneficiaryService, BeneficiaryService>();
builder.Services.AddScoped<IVolunteerService, VolunteerService>();
builder.Services.AddScoped<IDisasterService, DisasterService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Disaster Alleviation API v1");
        c.RoutePrefix = string.Empty; // ⭐ makes Swagger the homepage
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

// ⭐ Add a default route for / (optional safety)
app.MapGet("/", () => Results.Ok("🏥 Disaster Alleviation API is running... Go to /swagger to test endpoints."));

app.Run();
