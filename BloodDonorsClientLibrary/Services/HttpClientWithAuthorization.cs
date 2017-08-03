using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BloodDonorsClientLibrary.Services
{
    public abstract class HttpClientWithAuthorization : HttpClientBase
    {
        protected HttpClientWithAuthorization(HttpClient client) : base(client)
        {
            Token = "";
        }

        protected string Token;

        public abstract Task Login(string pesel, string password);

        public async void Logout()
        {
            Token = "";
            AddAuthorizationToClient();
        }

        protected void AddAuthorizationToClient()
        {
            Client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {Token}");
        }

        protected async void AutomaticLogout(DateTime dateTime)
        {
            var timeSpan = dateTime - DateTime.UtcNow;
            await Task.Delay(timeSpan);
            Logout();
        }
    }
}