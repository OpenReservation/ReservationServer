using System;
using System.Net.Http;

namespace OpenReservation.API.Test.Controllers
{
    public abstract class ControllerTestBase
    {
        protected HttpClient Client { get; }

        protected IServiceProvider Services { get; }

        protected ControllerTestBase(APITestFixture fixture)
        {
            Client = fixture.Client;
            Services = fixture.Services;
        }
    }
}
