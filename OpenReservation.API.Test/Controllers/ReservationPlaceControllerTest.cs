using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OpenReservation.Database;
using OpenReservation.Models;
using Xunit;

namespace OpenReservation.API.Test.Controllers
{
    public class ReservationPlaceControllerTest : ControllerTestBase
    {
        public ReservationPlaceControllerTest(APITestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetReservationPlaceList()
        {
            using var response = await Client.GetAsync("/api/reservationPlaces");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ReservationPlace[]>(responseString);
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
            using var response = await Client.GetAsync($"/api/reservationPlaces/{place.PlaceId:N}/periods?dt={DateTime.Today:yyyy-MM-dd}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ReservationPeriod[]>(responseString);
            Assert.NotNull(result);
        }
    }
}
