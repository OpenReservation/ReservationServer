using System;
using System.Net.Http;

namespace OpenReservation.API.Test.Controllers;

public abstract class ControllerTestBase(APITestFixture fixture)
{
    protected HttpClient Client { get; } = fixture.Client;

    protected IServiceProvider Services { get; } = fixture.Services;
}