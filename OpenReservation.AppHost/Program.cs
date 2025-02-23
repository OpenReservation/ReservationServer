var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis")
    .WithRedisInsight()
    ;
builder.AddProject<Projects.OpenReservation>("reservation")
    .WithReference(redis);

await builder.Build().RunAsync();
