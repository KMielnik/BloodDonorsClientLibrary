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
        public async Task LoginAsync(string pesel, string password)
            => await base.LoginAsync(pesel, password, "donor/login");


        /// <summary>
        /// Returns name of logged donor.
        /// </summary>
        /// <exception cref="UserNotLoggedInException">
        ///     Thrown when user is not logged in.
        /// </exception>
        public async Task<string> GetNameAsync()
        {
            var response = await Client.GetAsync("donor/name");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UserNotLoggedInException();
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        /// <summary>
        /// Returns amount of blood donated by logged donor in mililiters.
        /// </summary>
        /// <exception cref="UserNotLoggedInException">
        ///     Thrown when user is not logged in.
        /// </exception>
        public async Task<int> HowMuchDonatedAsync()
        {
            var response = await Client.GetAsync("donor/donations/volume");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UserNotLoggedInException();

            var responseBody = await response.Content.ReadAsStringAsync();
            var mililitersDonated = int.Parse(responseBody);
            return mililitersDonated;
        }

        /// <summary>
        /// Returns time when donor will be able to donate blood again.
        /// </summary>
        /// <exception cref="UserNotLoggedInException">
        ///     Thrown when user is not logged in.
        /// </exception>
        public async Task<DateTime> WhenAbleToDonateAgainAsync()
        {
            var response = await Client.GetAsync("donor/donations/whenabletodonate");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UserNotLoggedInException();

            var responseBody = await response.Content.ReadAsStringAsync();
            var date = JsonConvert.DeserializeObject<DateTime>(responseBody);
            return date;
        }

        /// <summary>
        /// Returns whole donor account.
        /// </summary>
        /// <exception cref="UserNotLoggedInException">
        ///     Thrown when user is not logged in.
        /// </exception>
        public async Task<Donor> GetAccountAsync()
        {
            var response = await Client.GetAsync("donor");

            switch (response.StatusCode)
            {
                case HttpStatusCode.Gone:
                    throw new UserNotFoundException("Actual user has been deleted?");
                case HttpStatusCode.Unauthorized:
                    throw new UserNotLoggedInException();
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var donor = JsonConvert.DeserializeObject<Donor>(responseJson);
            return donor;
        }
    }
}