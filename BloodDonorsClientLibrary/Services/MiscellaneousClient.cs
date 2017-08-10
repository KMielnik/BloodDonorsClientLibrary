using System;
using System.Collections.Generic;
using System.Net;
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

        /// <summary>
        /// Checks if server is online by sending a request to it.
        /// </summary>
        public async Task<bool> IsServerOnline()
        {
            try
            {
                var response = await Client.GetAsync("whatever");
            }
            catch (HttpRequestException)
            {
                return false;
            }
            return true;
        }
    }
}