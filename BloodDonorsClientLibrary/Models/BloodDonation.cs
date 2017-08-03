using System;

namespace BloodDonorsClientLibrary.Models
{
    public class BloodDonation
    {
        public DateTime DateOfDonation { get; set; }
        public int Volume { get; set; }
        public BloodType BloodType { get; set; }
        public Donor Donor { get; set; }
        public Personnel BloodTaker { get; set; }

        public BloodDonation()
        {

        }

        public BloodDonation(DateTime dateOfDonation, int volume, BloodType bloodType, Donor donor, Personnel bloodTaker)
        {
            DateOfDonation = dateOfDonation;
            Volume = volume;
            BloodType = bloodType;
            Donor = donor;
            BloodTaker = bloodTaker;
        }
    }
}