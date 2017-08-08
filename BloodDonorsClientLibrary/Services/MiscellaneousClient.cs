using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BloodDonorsClientLibrary.Models;
using Newtonsoft.Json;

namespace BloodDonorsClientLibrary.Services
{
    public class MiscellaneousClient : HttpClientBase
    {
        internal MiscellaneousClient(HttpClient client) : base(client)
        {
        }

        /// <summary>
        /// Return list of honorary(donated over 20 liters of blood) donors.
        /// </summary>
        public async Task<IEnumerable<DonorScore>> GetHonoraryDonorsAsync()
        {
            var responseJson = await Client.GetStringAsync("donations/honorary");
            var honoraryDonors = JsonConvert.DeserializeObject<IEnumerable<DonorScore>>(responseJson);

            return honoraryDonors;
        }

        /// <summary>
        /// Returns volume of all blood donated ever.
        /// </summary>
        public async Task<int> GetAllBloodDonatedVolumeAsync()
        {
            var responseJson = await Client.GetStringAsync("donations/allbloodvolume");
            var allBloodVolume = JsonConvert.DeserializeObject<int>(responseJson);

            return allBloodVolume;
        }
    }
}