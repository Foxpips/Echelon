using System;
using System.Threading.Tasks;
using System.Web.Http;
using Echelon.Core.Infrastructure.Services.Rest;

namespace Echelon.Controllers.Api
{
    public class EmbedController : ApiController
    {
        private readonly IRestService _restService;

        public EmbedController(IRestService restService)
        {
            _restService = restService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetVideo(VideoTypeEnum videoType, string url)
        {
            switch (videoType)
            {
                case VideoTypeEnum.Vimeo:
                    return
                        Json(
                            await
                                _restService.MakeGenericRequest<object>(
                                    new Uri($"https://vimeo.com/api/oembed.json?url={url}")));
                case VideoTypeEnum.Dailymotion:
                    return
                        Json(
                            await
                                _restService.MakeGenericRequest<object>(
                                    new Uri($"https://www.dailymotion.com/services/oembed?url={url}")));
                case VideoTypeEnum.Youtube:
                    break;
                case VideoTypeEnum.Twitch:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(videoType), videoType, null);
            }

            return Json(string.Empty);
        }
    }

    public enum VideoTypeEnum
    {
        Vimeo,
        Youtube,
        Dailymotion,
        Twitch
    }
}