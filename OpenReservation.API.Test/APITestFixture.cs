using System;
using System.Net.Http;

namespace OpenReservation.API.Test;

public class APITestFixture(IServiceProvider serviceProvider, HttpClient httpClient)
{
    public IServiceProvider Services { get; } = serviceProvider;

    public HttpClient Client { get; } = httpClient;
}