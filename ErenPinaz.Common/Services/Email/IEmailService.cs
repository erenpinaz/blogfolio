using System.Net.Mail;
using System.Threading.Tasks;

/* Credits: https://github.com/andrewdavey/postal */

namespace ErenPinaz.Common.Services.Email
{
    public interface IEmailService
    {
        /// <summary>
        ///     Sends a <see cref="MailMessage" />
        /// </summary>
        /// <param name="email"></param>
        void Send(MailMessage email);

        /// <summary>
        ///     Asynchronously sends a <see cref="MailMessage" />
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A <see cref="Task" /></returns>
        Task SendAsync(MailMessage email);
    }
}