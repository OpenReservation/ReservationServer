using System.Net.Http;

namespace ActivityReservation.Common
{
    public class NoProxyHttpClientHandler : HttpClientHandler
    {
        public NoProxyHttpClientHandler()
        {
            Proxy = null;
            UseProxy = false;
        }
    }
}
