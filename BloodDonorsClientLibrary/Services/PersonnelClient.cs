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
        public PersonnelClient(HttpClient client) : base(client)
        {
        }

        /// <summary>
        /// Login into database.
        /// </summary>
        /// <param name="pesel">Personnel pesel</param>
        /// <param name="password">Personnel password</param>
        /// <returns></returns>
        public override async Task LoginAsync(string pesel, string password)
        {
            var loginCredentials = new LoginCredentials
            {
                Pesel = pesel,
                Password = password
            };

            var loginJson = JsonConvert.SerializeObject(loginCredentials);
            var response = await Client.PostJsonAsync("personnel/login", new StringContent(loginJson));

            if (response.IsSuccessStatusCode)
            {
                var jwtJson = await response.Content.ReadAsStringAsync();
                var jwt = JsonConvert.DeserializeObject<Jwt>(jwtJson);

                Token = jwt.Token;
                AddAuthorizationToClient();
                AutomaticLogout(jwt.Expires);
            }
        }

        /// <summary>
        /// Returns name of logged personnel.
        /// </summary>
        public async Task<string> GetNameAsync()
        {
            var response = await Client.GetStringAsync("personnel/name");
            return response;
        }

        /// <summary>
        /// Returns all blood taken by logged in personnel.
        /// </summary>
        public async Task<IEnumerable<BloodDonation>> GetAllBloodTakenByPersonnelAsync()
        {
            var response = await Client.GetAsync("personnel/alltakenblood");
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
        public async Task<DateTime> LastDonationDateByDonorAsync(string donorsPesel)
        {
            var response = await Client.GetAsync($"personnel/lastDonationBy/{donorsPesel}");

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
        public async Task RegisterDonorAsync(string pesel, string name, BloodType bloodType, string mail, string phone)
        {
            var registerDonor = new RegisterDonor(pesel, name, bloodType, mail, phone);
            var registerDonorJson = JsonConvert.SerializeObject(registerDonor);

            var response = await Client.PostJsonAsync("personnel/newDonor", new StringContent(registerDonorJson));

            if (response.StatusCode.Equals(HttpStatusCode.Conflict))
                throw new ResouceAlreadyExistsException(response.ReasonPhrase);
        }

        /// <summary>
        /// Add new blood donation
        /// </summary>
        /// <param name="dateOfDonation">When was the blood donated</param>
        /// <param name="volume">Volume of donated blood in mililiters</param>
        /// <param name="bloodType"></param>
        /// <param name="donorPesel"></param>
        /// <exception cref="UserNotFoundException">
        ///     Thrown when donor with provided pesel has not been found.
        /// </exception>
        public async Task AddDonationAsync(DateTime dateOfDonation, int volume, BloodType bloodType, string donorPesel)
        {
            var personnelPesel = (await GetAccountAsync()).Pesel;

            var newDonation = new AddDonation(dateOfDonation, volume, bloodType, donorPesel, personnelPesel);
            var newDonationJson = JsonConvert.SerializeObject(newDonation);

            var response = await Client.PostJsonAsync("personnel/newDonation", new StringContent(newDonationJson));

            if (response.StatusCode.Equals(HttpStatusCode.BadRequest))
                throw new UserNotFoundException(response.ReasonPhrase);
        }

        /// <summary>
        /// Get Donor with provided pesel
        /// </summary>
        /// <param name="pesel"></param>
        /// <exception cref="UserNotFoundException">Thrown if user with that pesel was not found.</exception>
        public async Task<Donor> GetDonorByPeselAsync(string pesel)
        {
            var response = await Client.GetAsync($"personnel/donor/{pesel}");

            if (response.StatusCode.Equals(HttpStatusCode.NotFound))
                throw new UserNotFoundException(response.ReasonPhrase);

            var responseJson = await response.Content.ReadAsStringAsync();
            var donor = JsonConvert.DeserializeObject<Donor>(responseJson);

            return donor;
        }

        /// <summary>
        /// Returns all blood donations
        /// </summary>
        public async Task<IEnumerable<BloodDonation>> GetAllBloodAsync()
        {
            var responseJson = await Client.GetStringAsync("allBlood");
            var allBlood = JsonConvert.DeserializeObject<IEnumerable<BloodDonation>>(responseJson);

            return allBlood;
        }

        /// <summary>
        /// Returns whole personnel account.
        /// </summary>
        public async Task<Personnel> GetAccountAsync()
        {
            var response = await Client.GetAsync("personnel");

            if (response.StatusCode.Equals(HttpStatusCode.Gone))
                throw new UserNotFoundException("Actual user has been deleted?");

            var responseJson = await response.Content.ReadAsStringAsync();
            var personnel = JsonConvert.DeserializeObject<Personnel>(responseJson);
            return personnel;
        }
    }
}