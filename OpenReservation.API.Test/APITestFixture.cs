using System;
using System.Net.Http;
using OpenReservation.Database;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeihanLi.Common.Helpers;
using Xunit;

namespace OpenReservation.API.Test
{
    /// <summary>
    /// Shared Context https://xunit.github.io/docs/shared-context.html
    /// </summary>
    public class APITestFixture : IDisposable
    {
        private readonly IWebHost _server;
        public IServiceProvider Services { get; }

        public HttpClient Client { get; }

        public APITestFixture()
        {
            var baseUrl = $"http://localhost:{NetHelper.GetRandomPort()}";
            _server = WebHost.CreateDefaultBuilder()
                .UseUrls(baseUrl)
                .UseStartup<TestStartup>()
                .Build();
            _server.Start();

            Services = _server.Services;

            Client = new HttpClient(new WeihanLi.Common.Http.NoProxyHttpClientHandler())
            {
                BaseAddress = new Uri($"{baseUrl}")
            };
            // Add Api-Version Header
            // Client.DefaultRequestHeaders.TryAddWithoutValidation("Api-Version", "1.2");

            Initialize();

            Console.WriteLine("test begin");
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

            Console.WriteLine("test end");
        }
    }

    [CollectionDefinition("APITestCollection")]
    public class APITestCollection : ICollectionFixture<APITestFixture>
    {
    }
}
