using System.Net;
using System.Threading.Tasks;
using ActivityReservation.ViewModels;
using Newtonsoft.Json;
using WeihanLi.Common.Models;
using Xunit;

namespace ActivityReservation.API.Test.Controllers
{
    public class ReservationControllerTest : ControllerTestBase
    {
        public ReservationControllerTest(APITestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetReservationList()
        {
            using (var response = await Client.GetAsync("/api/reservation"))
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var responseString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<PagedListModel<ReservationListViewModel>>(responseString);
                Assert.NotNull(result);
            }
        }
    }
}
