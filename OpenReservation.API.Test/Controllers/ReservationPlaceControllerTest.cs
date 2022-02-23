using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenReservation.Database;
using OpenReservation.Models;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace OpenReservation.API.Test.Controllers;

public class ReservationPlaceControllerTest : ControllerTestBase
{
    public ReservationPlaceControllerTest(APITestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task GetReservationPlaceList()
    {
        var result = await Client.GetFromJsonAsync<ReservationPlace[]>("/api/reservationPlaces");
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetReservationPlacePeriods()
    {
        using var scope = Services.CreateScope();
        var place = await scope.ServiceProvider.GetRequiredService<ReservationDbContext>()
            .ReservationPlaces.AsNoTracking()
            .FirstOrDefaultAsync();
        Assert.NotNull(place);

        var result = await Client.GetFromJsonAsync<ReservationPeriod[]>($"/api/reservationPlaces/{place.PlaceId:N}/periods?dt={DateTime.Today:yyyy-MM-dd}");
        Assert.NotNull(result);
    }
}
