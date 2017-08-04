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
            WhenLogout = OnLogout;
            Token = "";
        }

        public bool IsLoggedIn { get; protected set; }
        protected string Token;
        public event EventHandler WhenLogout;

        public abstract Task LoginAsync(string pesel, string password);

        public void Logout()
        {
            WhenLogout?.Invoke(this,EventArgs.Empty);
        }

        protected void OnLogout(object sender,EventArgs e)
        {
            Token = "";
            AddAuthorizationToClient();
            IsLoggedIn = false;
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