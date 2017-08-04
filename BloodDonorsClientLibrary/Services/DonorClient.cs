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

        /// <summary>
        /// Login into database.
        /// </summary>
        /// <param name="pesel">Donors pesel</param>
        /// <param name="password">Donors password</param>
        /// <returns></returns>
        public override async Task LoginAsync(string pesel, string password)
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
                AddAuthorizationToClient();
                AutomaticLogout(jwt.Expires);
                IsLoggedIn = true;
            }
        }

        /// <summary>
        /// Returns name of logged donor.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetNameAsync()
        {
            var response = await Client.GetStringAsync("donor/name");
            return response;
        }

        /// <summary>
        /// Returns amount of blood donated by logged donor in mililiters.
        /// </summary>
        /// <returns></returns>
        public async Task<int> HowMuchDonatedAsync()
        {
            var response = await Client.GetStringAsync("donor/donations/volume");
            var mililitersDonated = int.Parse(response);
            return mililitersDonated;
        }

        /// <summary>
        /// Returns time when donor will be able to donate blood again.
        /// </summary>
        /// <returns></returns>
        public async Task<DateTime> WhenAbleToDonateAgainAsync()
        {
            var response = await Client.GetStringAsync("donor/donations/whenabletodonate");
            var date = JsonConvert.DeserializeObject<DateTime>(response);
            return date;
        }

        /// <summary>
        /// Returns whole donor account.
        /// </summary>
        /// <returns></returns>
        public async Task<Donor> GetAccountAsync()
        {
            var response = await Client.GetStringAsync("donor");
            var donor = JsonConvert.DeserializeObject<Donor>(response);
            return donor;
        }
    }
}