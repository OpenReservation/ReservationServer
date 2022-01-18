using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using OpenReservation.Common;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeihanLi.Common.Http;
using Xunit;

namespace OpenReservation.UnitTest.Common;

public class TencentCaptchaHelperTest
{
    private readonly TencentCaptchaHelper _captchaHelper;
    private readonly MockHttpHandler _mockHttpHandler = new();

    public TencentCaptchaHelperTest()
    {
        _captchaHelper = new TencentCaptchaHelper(Options.Create(new TencentCaptchaOptions()
        {
            AppId = "test",
            AppSecret = "test"
        }), NullLogger<TencentCaptchaHelper>.Instance, new HttpClient(_mockHttpHandler));
    }

    [Fact]
    public async Task IsValidRequestAsync_Valid()
    {
        _mockHttpHandler.SetResponseFactory(req => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new TencentCaptchaHelper.TencentCaptchaResponse()
            {
                Code = 1
            })
        });
        var result = await _captchaHelper.IsValidRequestAsync(new TencentCaptchaRequest() { Nonce = "1234", Ticket = "test" });
        Assert.True(result);
    }

    [Fact]
    public async Task IsValidRequestAsync_Invalid()
    {
        _mockHttpHandler.SetResponseFactory(req => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new TencentCaptchaHelper.TencentCaptchaResponse()
            {
                Code = 0
            })
        });
        var result = await _captchaHelper.IsValidRequestAsync(new TencentCaptchaRequest() { Nonce = "1234", Ticket = "test" });
        Assert.False(result);
    }
}
