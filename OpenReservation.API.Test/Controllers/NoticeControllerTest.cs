using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenReservation.Models;
using WeihanLi.Common.Models;
using Xunit;

namespace OpenReservation.API.Test.Controllers
{
    public class NoticeControllerTest : ControllerTestBase
    {
        public NoticeControllerTest(APITestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetNoticeList()
        {
            using var response = await Client.GetAsync("/api/notice");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PagedListResult<Notice>>(responseString);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetNoticeDetails()
        {
            var path = "test-notice";
            using var response = await Client.GetAsync($"/api/notice/{path}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Notice>(responseString);
            Assert.NotNull(result);
            Assert.Equal(path, result.NoticeCustomPath);
        }

        [Fact]
        public async Task GetNoticeDetails_NotFound()
        {
            using var response = await Client.GetAsync("/api/notice/test-notice1212");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
