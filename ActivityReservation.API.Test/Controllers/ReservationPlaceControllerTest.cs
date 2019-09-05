using System.Net;
using System.Threading.Tasks;
using ActivityReservation.Models;
using Newtonsoft.Json;
using Xunit;

namespace ActivityReservation.API.Test.Controllers
{
    public class ReservationPlaceControllerTest : ControllerTestBase
    {
        public ReservationPlaceControllerTest(APITestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetReservationPlaceList()
        {
            using (var response = await Client.GetAsync("/api/reservationPlace"))
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var responseString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ReservationPlace>(responseString);
                Assert.NotNull(result);
            }
        }
    }
}
