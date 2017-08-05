using System.Net.Http;

namespace BloodDonorsClientLibrary.Services
{
    public class ClientFactory
    {
        private readonly HttpClient client;

        public ClientFactory()
        {
            client = new HttpClient();
        }

        public DonorClient GetDonorClient()
            => new DonorClient(client);

        public PersonnelClient GetPersonnelClient()
            => new PersonnelClient(client);

        public MiscellaneousClient GetMiscellaneousClient()
            => new MiscellaneousClient(client);
    }
}