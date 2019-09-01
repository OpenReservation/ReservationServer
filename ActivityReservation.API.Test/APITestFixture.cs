using System;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using ActivityReservation.Database;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ActivityReservation.API.Test
{
    /// <summary>
    /// Shared Context https://xunit.github.io/docs/shared-context.html
    /// </summary>
    public class APITestFixture : IDisposable
    {
        private readonly IWebHost _server;
        private IServiceProvider Services { get; }

        public HttpClient Client { get; }

        public APITestFixture()
        {
            var baseUrl = $"http://localhost:{GetRandomPort()}";
            var builder = WebHost.CreateDefaultBuilder()
                .UseUrls(baseUrl)
                .UseStartup<TestStartup>();

            _server = builder.Build();
            _server.Start();

            Services = _server.Services;

            Client = new HttpClient(new WeihanLi.Common.Http.NoProxyHttpClientHandler())
            {
                BaseAddress = new Uri($"{baseUrl}")
            };
            // Add Api-Version Header
            // Client.DefaultRequestHeaders.TryAddWithoutValidation("Api-Version", "1.2");

            Initialize();
        }

        /// <summary>
        /// TestDataInitialize
        /// </summary>
        private void Initialize()
        {
        }

        public void Dispose()
        {
            using (var dbContext = Services.GetRequiredService<ReservationDbContext>())
            {
                if (dbContext.Database.IsInMemory())
                {
                    dbContext.Database.EnsureDeleted();
                }
            }

            Client.Dispose();
            _server.Dispose();
        }

        private static int GetRandomPort()
        {
            var activePorts = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().Select(_ => _.Port).ToList();

            var random = new Random();
            var randomPort = random.Next(10000, 65535);

            while (activePorts.Contains(randomPort))
            {
                randomPort = random.Next(10000, 65535);
            }

            return randomPort;
        }
    }

    [CollectionDefinition("APITestCollection")]
    public class APITestCollection : ICollectionFixture<APITestFixture>
    {
    }
}
