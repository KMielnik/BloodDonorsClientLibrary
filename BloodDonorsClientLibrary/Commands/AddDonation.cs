using System;
using BloodDonorsClientLibrary.Models;

namespace BloodDonorsClientLibrary.Commands
{
    class AddDonation
    {
        public DateTime DateOfDonation { get; set; }
        public int Volume { get; set; }
        public BloodType BloodType { get; set; }
        public string DonorPesel { get; set; }
        public string BloodTakerPesel { get; set; }

        public AddDonation()
        {
            
        }

        public AddDonation(DateTime dateOfDonation, int volume, BloodType bloodType, string donorPesel, string bloodTakerPesel)
        {
            DateOfDonation = dateOfDonation;
            Volume = volume;
            BloodType = bloodType;
            DonorPesel = donorPesel;
            BloodTakerPesel = bloodTakerPesel;
        }
    }
}