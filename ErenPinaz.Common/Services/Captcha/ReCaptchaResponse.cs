using System.Collections.Generic;
using Newtonsoft.Json;

namespace ErenPinaz.Common.Services.Captcha
{
    public class ReCaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}