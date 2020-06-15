using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OpenReservation.ViewModels;
using Newtonsoft.Json;
using WeihanLi.Common;
using WeihanLi.Common.Models;
using Xunit;

namespace OpenReservation.API.Test.Controllers
{
    public class ReservationControllerTest : ControllerTestBase
    {
        public ReservationControllerTest(APITestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetReservationList()
        {
            using var response = await Client.GetAsync("/api/reservations");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PagedListModel<ReservationListViewModel>>(responseString);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task MakeReservationWithInvalidRequest()
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "/api/reservations");
            request.Headers.TryAddWithoutValidation("UserId", GuidIdGenerator.Instance.NewId());
            request.Headers.TryAddWithoutValidation("UserName", Environment.UserName);

            request.Content = new StringContent("{}", Encoding.UTF8, "application/json");

            using var response = await Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task MakeReservationWithUserInfo()
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "/api/reservations");
            request.Headers.TryAddWithoutValidation("UserId", GuidIdGenerator.Instance.NewId());
            request.Headers.TryAddWithoutValidation("UserName", Environment.UserName);
            request.Headers.TryAddWithoutValidation("UserRoles", "User,ReservationManager");

            request.Content = new StringContent($@"{{""reservationUnit"":""nnnnn"",""reservationActivityContent"":""13211112222"",""reservationPersonName"":""谢谢谢"",""reservationPersonPhone"":""13211112222"",""reservationPlaceId"":""f9833d13-a57f-4bc0-9197-232113667ece"",""reservationPlaceName"":""第一多功能厅"",""reservationForDate"":""2020-06-13"",""reservationForTime"":""10:00~12:00"",""reservationForTimeIds"":""1""}}", Encoding.UTF8, "application/json");

            using var response = await Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task MakeReservationWithInvalidUserInfo()
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "/api/reservations");

            request.Headers.TryAddWithoutValidation("UserName", Environment.UserName);

            request.Content = new StringContent($@"{{""reservationUnit"":""nnnnn"",""reservationActivityContent"":""13211112222"",""reservationPersonName"":""谢谢谢"",""reservationPersonPhone"":""13211112222"",""reservationPlaceId"":""f9833d13-a57f-4bc0-9197-232113667ece"",""reservationPlaceName"":""第一多功能厅"",""reservationForDate"":""2020-06-13"",""reservationForTime"":""10:00~12:00"",""reservationForTimeIds"":""1""}}", Encoding.UTF8, "application/json");

            using var response = await Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task MakeReservationWithoutUserInfo()
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "/api/reservations")
            {
                Content = new StringContent(
                    @"{""reservationUnit"":""nnnnn"",""reservationActivityContent"":""13211112222"",""reservationPersonName"":""谢谢谢"",""reservationPersonPhone"":""13211112222"",""reservationPlaceId"":""f9833d13-a57f-4bc0-9197-232113667ece"",""reservationPlaceName"":""第一多功能厅"",""reservationForDate"":""2020-06-13"",""reservationForTime"":""10:00~12:00"",""reservationForTimeIds"":""1""}",
                    Encoding.UTF8, "application/json")
            };

            using var response = await Client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
