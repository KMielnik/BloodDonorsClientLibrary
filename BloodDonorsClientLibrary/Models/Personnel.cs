namespace BloodDonorsClientLibrary.Models
{
    public class Personnel
    {
        public string Pesel { get; set; }
        public string Name { get; set; }

        public Personnel()
        {

        }

        public Personnel(string pesel, string name)
        {
            Pesel = pesel;
            Name = name;
        }

        public override string ToString()
        {
            return Pesel;
        }
    }
}