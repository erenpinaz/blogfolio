using System.Threading.Tasks;

namespace ErenPinaz.Common.Services.Captcha
{
    public interface ICaptchaService
    {
        Task<ReCaptchaResponse> ValidateAsync(string secret, string response);
    }
}