using System;
using System.Net.Http;
using System.Threading.Tasks;
using BloodDonorsClientLibrary.Commands;
using BloodDonorsClientLibrary.Extensions;
using BloodDonorsClientLibrary.Models;
using Newtonsoft.Json;

namespace BloodDonorsClientLibrary.Services
{
    public class DonorClient : HttpClientWithAuthorization
    {
        public DonorClient(HttpClient client) : base(client)
        {
        }

        public override async Task Login(string pesel, string password)
        {
            var loginCredentials = new LoginCredentials
            {
                Pesel = pesel,
                Password = password
            };

            var loginJson = JsonConvert.SerializeObject(loginCredentials);
            var response =  await Client.PostJsonAsync("donor/login", new StringContent(loginJson));

            if (response.IsSuccessStatusCode)
            {
                var jwtJson = await response.Content.ReadAsStringAsync();
                var jwt = JsonConvert.DeserializeObject<Jwt>(jwtJson);

                Token = jwt.Token;
                AutomaticLogout(jwt.Expires);
            }
        }
    }
}