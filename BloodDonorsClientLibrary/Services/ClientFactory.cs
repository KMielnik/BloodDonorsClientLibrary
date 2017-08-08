using System;
using System.Net.Http;

namespace BloodDonorsClientLibrary.Services
{
    public class ClientFactory
    {
        private DonorClient donorClient;
        private PersonnelClient personnelClient;
        private MiscellaneousClient miscellaneousClient;

        private readonly HttpClient client;

        /// <summary>
        /// Create factory which returns single instances of particuliar clients, you can use all of them at once.
        /// </summary>
        /// <param name="apiServerAdress">
        /// Adress of API server which you would like to use
        /// like "localhost","74.12.156.11","www.apiadrress.com"
        /// </param>
        public ClientFactory(string apiServerAdress)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri($"http://{apiServerAdress}/api/");
        }

        /// <summary>
        /// Get instance of DonorClient
        /// </summary>
        public DonorClient GetDonorClient()
            => donorClient ?? (donorClient = new DonorClient(client));

        /// <summary>
        /// Get instance of PersonnelClient
        /// </summary>
        public PersonnelClient GetPersonnelClient()
            => personnelClient ?? (personnelClient = new PersonnelClient(client));

        /// <summary>
        /// Get instance of MiscellaneousClient
        /// </summary>
        public MiscellaneousClient GetMiscellaneousClient()
            => miscellaneousClient ?? (miscellaneousClient = new MiscellaneousClient(client));
    }
}