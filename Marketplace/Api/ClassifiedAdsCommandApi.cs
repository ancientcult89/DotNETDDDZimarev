using Microsoft.AspNetCore.Mvc;
using Serilog;
using static Marketplace.Contracts.ClassifiedAds;

namespace Marketplace.Api
{
    [Route("/ad")]
    [ApiController]
    public class ClassifiedAdsCommandApi : ControllerBase
    {
        private readonly ClassifiedAdsApplicationService _applicationService;
        public ClassifiedAdsCommandApi(ClassifiedAdsApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(V1.Create request) => await HandleRequest(request, _applicationService.Handle);

        [Route("name")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.SetTitle request) => await HandleRequest(request, _applicationService.Handle);

        [Route("text")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.UpdateText request) => await HandleRequest(request, _applicationService.Handle);

        [Route("price")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.UpdatePrice request) => await HandleRequest(request, _applicationService.Handle);

        [Route("publish")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.RequestToPublish request) => await HandleRequest(request, _applicationService.Handle);

        private async Task<IActionResult> HandleRequest<T>(T request, Func<T, Task> handler)
        {
            try
            {
                Log.Debug("Handle HTTP request of type {type}", typeof(T).Name);
                await handler(request);
                return Ok();
            }
            catch (Exception e)
            {
                Log.Error("Error handling the request", e);
                return new BadRequestObjectResult(new { error = e.Message, stackTrace = e.StackTrace });
            }
        }
    }
}
