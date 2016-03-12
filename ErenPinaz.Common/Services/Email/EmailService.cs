using System.Net.Mail;
using System.Threading.Tasks;

/* Credits: https://github.com/andrewdavey/postal */

namespace ErenPinaz.Common.Services.Email
{
    /// <summary>
    ///     Implementation that uses <see cref="SmtpClient" />
    /// </summary>
    public class EmailService : IEmailService
    {
        /// <summary>
        ///     Sends a <see cref="MailMessage" />
        /// </summary>
        /// <param name="email"></param>
        public void Send(MailMessage email)
        {
            using (var client = new SmtpClient())
            {
                client.Timeout = 10000;
                client.Send(email);
            }
        }

        /// <summary>
        ///     Asynchronously sends a <see cref="MailMessage" />
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A <see cref="Task" /></returns>
        public Task SendAsync(MailMessage email)
        {
            var client = new SmtpClient();
            try
            {
                var taskCompletionSource = new TaskCompletionSource<object>();

                client.SendCompleted += (o, e) =>
                {
                    client.Dispose();
                    email.Dispose();

                    if (e.Error != null)
                    {
                        taskCompletionSource.TrySetException(e.Error);
                    }
                    else if (e.Cancelled)
                    {
                        taskCompletionSource.TrySetCanceled();
                    }
                    else
                    {
                        taskCompletionSource.TrySetResult(null);
                    }
                };

                client.Timeout = 10000;
                client.SendAsync(email, null);
                return taskCompletionSource.Task;
            }
            catch
            {
                client.Dispose();
                email.Dispose();
                throw;
            }
        }
    }
}