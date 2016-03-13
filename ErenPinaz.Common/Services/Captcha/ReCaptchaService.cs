using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ErenPinaz.Common.Services.Captcha
{
    public class ReCaptchaService : ICaptchaService
    {
        public async Task<ReCaptchaResponse> ValidateAsync(string secret, string response)
        {
            using (var client = new WebClient())
            {
                var clientResponse = await client.DownloadStringTaskAsync(
                    new Uri(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response)));

                return JsonConvert.DeserializeObject<ReCaptchaResponse>(clientResponse);
            }
        }
    }
}
