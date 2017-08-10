namespace BloodDonorsClientLibrary.Models
{
    public class DonorScore
    {
        public string Name { get; set; }
        public int Volume { get; set; }

        public DonorScore(string name, int volume)
        {
            Name = name;
            Volume = volume;
        }

        public DonorScore()
        {

        }
    }
}