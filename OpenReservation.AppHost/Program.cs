var builder = DistributedApplication.CreateBuilder(args);
var reservation = builder.AddSqlServer("db")
    .WithDataVolume("reservation-volume")
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("Reservation")
    ;
var redis = builder.AddRedis("redis");
builder.AddProject<Projects.OpenReservation>("reservation-app")
    .WithReference(reservation)
    .WaitFor(reservation)
    .WithReference(redis)
    .WaitFor(redis)
    ;
await builder.Build().RunAsync();
