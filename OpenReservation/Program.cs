using OpenReservation;
using WeihanLi.Common.Event;

var builder = WebApplication.CreateSlimBuilder();
builder.Configuration.AddEnvironmentVariables("Reservation_");

builder.Logging.AddJsonConsole(options =>
{
    options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss]";
    options.JsonWriterOptions = new System.Text.Json.JsonWriterOptions()
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
});
var startup = new Startup(builder.Configuration, builder.Environment);

builder.AddServiceDefaults();
startup.ConfigureServices(builder.Services);

var app = builder.Build();

app.MapDefaultEndpoints();
startup.Configure(
    app, app.Services.GetRequiredService<ILoggerFactory>(), 
    app.Services.GetRequiredService<IEventBus>()
    );

await app.RunAsync();
