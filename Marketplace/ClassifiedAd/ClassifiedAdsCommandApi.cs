using Marketplace.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.ClassifiedAd
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
        public async Task<IActionResult> Post(V1.Create request) => await RequestHandler.HandleRequest(request, _applicationService.Handle);

        [Route("name")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.SetTitle request) => await RequestHandler.HandleRequest(request, _applicationService.Handle);

        [Route("text")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.UpdateText request) => await RequestHandler.HandleRequest(request, _applicationService.Handle);

        [Route("price")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.UpdatePrice request) => await RequestHandler.HandleRequest(request, _applicationService.Handle);

        [Route("publish")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.RequestToPublish request) => await RequestHandler.HandleRequest(request, _applicationService.Handle);
    }
}
