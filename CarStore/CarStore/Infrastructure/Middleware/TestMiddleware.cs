using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Infrastructure.Middleware
{
    public class TestMiddleware
    {
        private RequestDelegate nextDelegate;
        public TestMiddleware(RequestDelegate next) => nextDelegate = next;

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext), "Missing httpContext");
            if (httpContext.Request.Path.ToString().ToUpperInvariant() == "/MIDDLEWARE")
            {
                await httpContext.Response.WriteAsync("Middleware is here", Encoding.UTF8).ConfigureAwait(true);
            }
            else
            {
                await nextDelegate.Invoke(httpContext).ConfigureAwait(true);
            }
        }
    }
}
