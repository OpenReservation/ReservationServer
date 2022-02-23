using OpenReservation.Models;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeihanLi.Common.Models;
using Xunit;

namespace OpenReservation.API.Test.Controllers;

public class NoticeControllerTest : ControllerTestBase
{
    public NoticeControllerTest(APITestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task GetNoticeList()
    {
        var result = await Client.GetFromJsonAsync<PagedListResult<Notice>>("/api/notice");
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetNoticeDetails()
    {
        var path = "test-notice";
        var result = await Client.GetFromJsonAsync<Notice>($"/api/notice/{path}");
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
