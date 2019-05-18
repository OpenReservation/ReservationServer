namespace Microsoft.AspNetCore.Http
{
    public static class RequestExtensions
    {
        public static string GetUserIP(this HttpContext httpContext, string realIPHeader = "X-Real-IP")
        {
            return httpContext.Request.Headers.TryGetValue(realIPHeader, out var ip) ? ip.ToString() : httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
