using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Diagnostics;

namespace Marketplace.Infrastructure
{
    public static class RequestHandler
    {
        public static async Task<IActionResult> HandleRequest<T>(T request, Func<T, Task> handler)
        {
            try
            {
                Log.Debug("Handle HTTP request of type {type}", typeof(T).Name);
                await handler(request);
                return new OkResult();
            }
            catch (Exception e)
            {
                Log.Error("Error handling the request", e);
                return new BadRequestObjectResult(new { error = e.Message, stackTrace = e.StackTrace });
            }
        }

        public static async Task<IActionResult> HandleQuery<TModel>(Func<Task<TModel>> query, ILogger log)
        {
            try
            {
                return new OkObjectResult(await query());
            }
            catch (Exception e)
            {
                log.Error(e, "Error handling the query");
                return new BadRequestObjectResult(new
                {
                    error = e.Message,
                    StackTrace = e.StackTrace
                });
            }
        }
    }
}
