namespace BloodDonorsClientLibrary.Models
{
    public class DonorScore
    {
        public string Name;
        public int Volume;

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