using System;
using System.Collections.Generic;
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
    public class PersonnelClient : HttpClientWithAuthorization
    {
        internal PersonnelClient(HttpClient client) : base(client)
        {
        }

        /// <summary>
        /// Login into database.
        /// </summary>
        /// <param name="pesel">Personnel pesel</param>
        /// <param name="password">Personnel password</param>
        public async Task LoginAsync(string pesel, string password)
            => await base.LoginAsync(pesel, password, "personnel/login");


        /// <summary>
        /// Returns name of logged personnel.
        /// </summary>
        /// <exception cref="UserNotLoggedInException">
        ///     Thrown when user is not logged in.
        /// </exception>
        public async Task<string> GetNameAsync()
        {
            var response = await Client.AuthenticatedGetAsync("personnel/name", Token);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UserNotLoggedInException();

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Returns all blood taken by logged in personnel.
        /// </summary>
        /// <exception cref="UserNotLoggedInException">
        ///     Thrown when user is not logged in.
        /// </exception>
        public async Task<IEnumerable<BloodDonation>> GetAllBloodTakenByPersonnelAsync()
        {
            var response = await Client.AuthenticatedGetAsync("personnel/alltakenblood", Token);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UserNotLoggedInException();

            var responseJson = await response.Content.ReadAsStringAsync();
            var bloodDonationsTakenByThisPersonnel =
                JsonConvert.DeserializeObject<IEnumerable<BloodDonation>>(responseJson);

            return bloodDonationsTakenByThisPersonnel;
        }

        /// <summary>
        /// Returns DateTime of last blood donation by donor with provided pesel.
        /// </summary>
        /// <param name="donorsPesel">Pesel of donor which info we want to get.</param>
        /// <exception cref="UserNotFoundException">When donor with that pesel couldn't be found.</exception>
        /// <exception cref="Exception">When http code wasn't 2xx or 404</exception>
        /// <exception cref="UserNotLoggedInException">
        ///     Thrown when user is not logged in.
        /// </exception>
        public async Task<DateTime> LastDonationDateByDonorAsync(string donorsPesel)
        {
            var response = await Client.AuthenticatedGetAsync($"personnel/lastDonationBy/{donorsPesel}", Token);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UserNotLoggedInException();

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var dateOfLastDonation = JsonConvert.DeserializeObject<DateTime>(responseJson);
                return dateOfLastDonation;
            }
            if (response.StatusCode.Equals(HttpStatusCode.NotFound))
                throw new UserNotFoundException("Couldn't find donor with that pesel.");
            throw new Exception("server error");
        }

        /// <summary>
        /// Registers new Donor
        /// </summary>
        /// <param name="pesel">Can't be already taken.</param>
        /// <param name="name"></param>
        /// <param name="bloodType"></param>
        /// <param name="mail"></param>
        /// <param name="phone"></param>
        /// <exception cref="ResouceAlreadyExistsException">
        ///     Thrown when donor with that pesel already exists.
        /// </exception>
        /// <exception cref="UserNotLoggedInException">
        ///     Thrown when user is not logged in.
        /// </exception>
        public async Task RegisterDonorAsync(string pesel, string name, BloodType bloodType, string mail, string phone)
        {
            var registerDonor = new RegisterDonor(pesel, name, bloodType, mail, phone);
            var registerDonorJson = JsonConvert.SerializeObject(registerDonor);

            var response =
                await Client.AuthenticatedPostJsonAsync("personnel/newDonor", new StringContent(registerDonorJson),
                    Token);

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UserNotLoggedInException();
                case HttpStatusCode.Conflict:
                    throw new ResouceAlreadyExistsException(response.ReasonPhrase);
            }
        }

        /// <summary>
        /// Add new blood donation
        /// </summary>
        /// <param name="dateOfDonation">When was the blood donated</param>
        /// <param name="volume">Volume of donated blood in mililiters</param>
        /// <param name="bloodType"></param>
        /// <param name="donorPesel"></param>
        /// <exception cref="UserNotLoggedInException">
        ///     Thrown when user is not logged in.
        /// </exception>
        /// <exception cref="UserNotFoundException">
        ///     Thrown when donor with provided pesel has not been found.
        /// </exception>
        public async Task AddDonationAsync(DateTime dateOfDonation, int volume, BloodType bloodType, string donorPesel)
        {
            var personnelPesel = (await GetAccountAsync()).Pesel;

            var newDonation = new AddDonation(dateOfDonation, volume, bloodType, donorPesel, personnelPesel);
            var newDonationJson = JsonConvert.SerializeObject(newDonation);

            var response =
                await Client.AuthenticatedPostJsonAsync("personnel/newDonation", new StringContent(newDonationJson),
                    Token);

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UserNotLoggedInException();
                case HttpStatusCode.BadRequest:
                    throw new UserNotFoundException(response.ReasonPhrase);
            }
        }

        /// <summary>
        /// Get Donor with provided pesel
        /// </summary>
        /// <param name="pesel"></param>
        /// <exception cref="UserNotFoundException">Thrown if user with that pesel was not found.</exception>
        /// <exception cref="UserNotLoggedInException">
        ///     Thrown when user is not logged in.
        /// </exception>
        public async Task<Donor> GetDonorByPeselAsync(string pesel)
        {
            var response = await Client.AuthenticatedGetAsync($"personnel/donor/{pesel}", Token);

            switch (response.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    throw new UserNotFoundException(response.ReasonPhrase);
                case HttpStatusCode.Unauthorized:
                    throw new UserNotLoggedInException();
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var donor = JsonConvert.DeserializeObject<Donor>(responseJson);

            return donor;
        }

        /// <summary>
        /// Returns all blood donations
        /// </summary>
        /// <exception cref="UserNotLoggedInException">
        ///     Thrown when user is not logged in.
        /// </exception>
        public async Task<IEnumerable<BloodDonation>> GetAllBloodAsync()
        {
            var response = await Client.AuthenticatedGetAsync("personnel/allBlood", Token);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UserNotLoggedInException();

            var responseJson = await response.Content.ReadAsStringAsync();

            var allBlood = JsonConvert.DeserializeObject<IEnumerable<BloodDonation>>(responseJson);

            return allBlood;
        }

        /// <summary>
        /// Returns whole personnel account.
        /// </summary>
        /// <exception cref="UserNotLoggedInException">
        ///     Thrown when user is not logged in.
        /// </exception>
        public async Task<Personnel> GetAccountAsync()
        {
            var response = await Client.AuthenticatedGetAsync("personnel", Token);

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UserNotLoggedInException();
                case HttpStatusCode.Gone:
                    throw new UserNotFoundException("Actual user has been deleted?");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var personnel = JsonConvert.DeserializeObject<Personnel>(responseJson);
            return personnel;
        }
    }
}