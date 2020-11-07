using System;
using System.Net.Http;

namespace OpenReservation.API.Test
{
    public class APITestFixture
    {
        public IServiceProvider Services { get; }

        public HttpClient Client { get; }

        public APITestFixture(IServiceProvider serviceProvider, HttpClient httpClient)
        {
            Services = serviceProvider;

            Client = httpClient;
        }
    }
}
