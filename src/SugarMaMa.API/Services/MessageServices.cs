using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SugarMaMa.API.Helpers;

namespace SugarMaMa.API.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private readonly SmsSettings _smsSettings;

        public AuthMessageSender(IOptions<SmsSettings> smsSettings)
        {
            _smsSettings = smsSettings.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }

        public async Task SendSmsAsync(string number, string message)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(_smsSettings.BaseUri) })
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_smsSettings.Sid}:{_smsSettings.Token}")));

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("To",$"+{number}"),
                    new KeyValuePair<string, string>("From", _smsSettings.From),
                    new KeyValuePair<string, string>("Body", message)
                });

                var a = await client.PostAsync(_smsSettings.RequestUri, content).ConfigureAwait(false);
            }
        }
    }
}
