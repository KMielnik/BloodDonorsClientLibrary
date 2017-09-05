using System;

namespace BloodDonorsClientLibrary.Models
{
    public class Donor
    {
        public string Pesel { get; set; }
        public string Name { get; set; }
        public DateTime LastDonated { get; set; }
        public BloodType BloodType { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }

        public Donor()
        {

        }

        public Donor(string pesel, string name, DateTime lastDonated, BloodType bloodType, string mail,
            string phone)
        {
            Pesel = pesel;
            Name = name;
            LastDonated = lastDonated;
            BloodType = bloodType;
            Mail = mail;
            Phone = phone;
        }

        public override string ToString()
        {
            return Pesel;
        }
    }
}