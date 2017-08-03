using BloodDonorsClientLibrary.Models;

namespace BloodDonorsClientLibrary.Commands
{
    class RegisterDonor
    {
        public string Pesel { get; set; }
        public string Name { get; set; }
        public BloodType BloodType { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
    }
}