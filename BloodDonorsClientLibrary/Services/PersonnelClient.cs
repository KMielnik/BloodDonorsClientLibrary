using System.Net.Http;
using System.Threading.Tasks;
using BloodDonorsClientLibrary.Commands;
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
        /// <returns></returns>
        public async Task<string> GetNameAsync()
        {
            var response = await Client.GetStringAsync("personnel/name");
            return response;
        }
    }
}