using OpenReservation;

Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(builder =>
    {
        builder.AddEnvironmentVariables("Reservation_");
    })
    .ConfigureLogging(builder => builder.AddJsonConsole(options =>
    {
        options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss]";
        options.JsonWriterOptions = new System.Text.Json.JsonWriterOptions()
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }))
    .ConfigureWebHostDefaults(webHostBuilder =>
    {
        webHostBuilder.UseStartup<Startup>();
    })
    .Build()
    .Run()
    ;
