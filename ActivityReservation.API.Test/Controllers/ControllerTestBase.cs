using System.Net.Http;
using Xunit;

namespace ActivityReservation.API.Test.Controllers
{
    [Collection("APITestCollection")]
    public class ControllerTestBase
    {
        public HttpClient Client { get; }

        public ControllerTestBase(APITestFixture fixture)
        {
            Client = fixture.Client;
        }
    }
}
