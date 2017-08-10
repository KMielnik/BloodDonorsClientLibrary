using System;

namespace BloodDonorsClientLibrary.Models
{
    public class Jwt
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}