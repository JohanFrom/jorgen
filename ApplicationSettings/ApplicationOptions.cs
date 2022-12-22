namespace jorgen.ApplicationSettings
{
    public class ApplicationOptions
    {
        public string WeatherUrl { get; set; }
        public string JorgenKeyVaultUrl { get; set; }
        public string JorgenKeyVaultSecretName { get; set; }

        public ApplicationOptions()
        {
            WeatherUrl = "";
            JorgenKeyVaultUrl = "";
            JorgenKeyVaultSecretName = "";
        }
    }
}
