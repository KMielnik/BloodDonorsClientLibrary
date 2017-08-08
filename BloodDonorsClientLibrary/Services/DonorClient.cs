using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BloodDonorsClientLibrary.Commands;
using BloodDonorsClientLibrary.Exceptions;
using BloodDonorsClientLibrary.Extensions;
using BloodDonorsClientLibrary.Models;
using Newtonsoft.Json;

namespace BloodDonorsClientLibrary.Services
{
    public class DonorClient : HttpClientWithAuthorization
    {
        internal DonorClient(HttpClient client) : base(client)
        {
        }

        /// <summary>
        /// Login into database.
        /// </summary>
        /// <param name="pesel">Donors pesel</param>
        /// <param name="password">Donors password</param>
        /// <exception cref="InvalidLoginCredentialsException">
        ///     Thrown when user couldn't be found/password was incorrect.
        /// </exception>
        public override async Task LoginAsync(string pesel, string password)
        {
            var loginCredentials = new LoginCredentials(pesel, password);

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
                return;
            }

            if(response.StatusCode.Equals(HttpStatusCode.BadRequest))
                throw new InvalidLoginCredentialsException();
        }

        /// <summary>
        /// Returns name of logged donor.
        /// </summary>
        public async Task<string> GetNameAsync()
        {
            var response = await Client.GetStringAsync("donor/name");
            return response;
        }

        /// <summary>
        /// Returns amount of blood donated by logged donor in mililiters.
        /// </summary>
        public async Task<int> HowMuchDonatedAsync()
        {
            var response = await Client.GetStringAsync("donor/donations/volume");
            var mililitersDonated = int.Parse(response);
            return mililitersDonated;
        }

        /// <summary>
        /// Returns time when donor will be able to donate blood again.
        /// </summary>
        public async Task<DateTime> WhenAbleToDonateAgainAsync()
        {
            var response = await Client.GetStringAsync("donor/donations/whenabletodonate");
            var date = JsonConvert.DeserializeObject<DateTime>(response);
            return date;
        }

        /// <summary>
        /// Returns whole donor account.
        /// </summary>
        public async Task<Donor> GetAccountAsync()
        {
            var response = await Client.GetAsync("donor");

            if(response.StatusCode.Equals(HttpStatusCode.Gone))
                throw new UserNotFoundException("Actual user has been deleted?");

            var responseJson = await response.Content.ReadAsStringAsync();
            var donor = JsonConvert.DeserializeObject<Donor>(responseJson);
            return donor;
        }
    }
}