namespace BloodDonorsClientLibrary.Commands
{
    class LoginCredentials
    {
        public string Pesel;
        public string Password;

        public LoginCredentials()
        {
        }

        public LoginCredentials(string pesel, string password)
        {
            Pesel = pesel;
            Password = password;
        }
    }
}