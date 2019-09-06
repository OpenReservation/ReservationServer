using System;
using System.Net.Http;
using Xunit;

namespace ActivityReservation.API.Test.Controllers
{
    [Collection("APITestCollection")]
    public class ControllerTestBase
    {
        protected HttpClient Client { get; }

        protected IServiceProvider Services { get; }

        public ControllerTestBase(APITestFixture fixture)
        {
            Client = fixture.Client;
            Services = fixture.Services;
        }
    }
}
