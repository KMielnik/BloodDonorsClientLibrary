﻿namespace BloodDonorsClientLibrary.Models
{
    public class BloodType
    {
        public string AboType { get; set; }
        public string RhType { get; set; }

        public BloodType()
        {

        }

        public BloodType(string aboType, string rhType)
        {
            AboType = aboType;
            RhType = rhType;
        }

        public override string ToString()
        {
            return $"{AboType,-2} Rh{RhType}";
        }
    }
}