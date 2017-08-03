using System;
using BloodDonorsClientLibrary.Models;

namespace BloodDonorsClientLibrary.Commands
{
    public class AddDonation
    {
        public DateTime DateOfDonation { get; set; }
        public int Volume { get; set; }
        public BloodType BloodType { get; set; }
        public string DonorPesel { get; set; }
        public string BloodTakerPesel { get; set; }
    }
}