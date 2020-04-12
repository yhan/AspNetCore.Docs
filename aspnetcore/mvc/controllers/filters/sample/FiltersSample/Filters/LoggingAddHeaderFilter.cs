using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace FiltersSample.Filters
{
    /// <summary>
    /// https://github.com/aspnet/Mvc/blob/master/test/WebSites/FiltersWebSite/Filters/AddHeaderAttribute.cs
    /// </summary>
    #region snippet_ResultFilter
    public class AddHeaderResultServiceFilter : IResultFilter
    {
        private ILogger _logger;
        public AddHeaderResultServiceFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AddHeaderResultServiceFilter>();
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            var headerName = "OnResultExecuting";
            context.HttpContext.Response.Headers.Add(headerName, new string[] { "ResultExecutingSuccessfully" });
            _logger.LogInformation("Header added: {HeaderName}", headerName);
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            // Can't add to headers here because response has started.
        }
    }
    #endregion

    public class AddHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AddHeaderMiddleware> _logger;

        public AddHeaderMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<AddHeaderMiddleware>();
        }
        public Task Invoke(HttpContext context)
        {
            _logger.LogDebug($"[Pipeline] {nameof(AddHeaderMiddleware)} Invoked");

            var headerName = "OnResultExecuting";
            context.Response.Headers.Add(headerName, new[] { "ResultExecutingSuccessfully" });

            var task = _next.Invoke(context);

            _logger.LogDebug($"[Pipeline] {nameof(AddHeaderMiddleware)} Returned");

            return task;
        }
    }
}