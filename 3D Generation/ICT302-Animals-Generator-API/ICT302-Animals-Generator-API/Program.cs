// Main program file and entry point for the Generator API application

using ICT302_Animals_Generator_API.Util;
using NReco.Logging.File;

var builder = WebApplication.CreateBuilder(args);

// Add allowed CORS origins
var cors = "_localCORSOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(cors,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "https://localhost:3000", "http://10.51.33.50",
                "http://localhost:*", "http://17.19.0.1", "https://api.wildvision.co", "https://wildvision.co",
                "https://*.vectorpixel.net");
        });
});

// Set fixed port 5000
builder.WebHost.UseUrls("http://0.0.0.0:5000");

// Registering file logger
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddFile("it01-backend-api.log", true);
});

// Registering API endpoint controllers
builder.Services.AddControllers();

// Registering Swagger API Explorer
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registering the Security Master Controller for auth requests between the backend and generation API applications
builder.Services.AddSingleton<SecurityMaster>();

// Building App
var app = builder.Build();

// Flag check to allow or block access to the Swagger API explorer
var enableSwagger = builder.Configuration.GetValue<bool>("EnableSwagger");
Console.WriteLine("Swagger Enabled? : {0}", enableSwagger);
if (enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Allow HTTPS Redirect
app.UseHttpsRedirection();

// Map controller endpoints to url's
app.MapControllers();
app.MapDefaultControllerRoute();

app.Run();