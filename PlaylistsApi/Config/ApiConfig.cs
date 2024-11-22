namespace PlaylistsApi.Config
{
    public class ApiConfig
    {
        private static readonly Lazy<ApiConfig> _instance = new(() => new ApiConfig());

        public static ApiConfig Instance => _instance.Value;

        public string BaseUrl { get; private set; }
        public string LoginEndpoint { get; private set; }
        public string ConteudosEndpoint { get; private set; }

        private ApiConfig()
        {
            BaseUrl = "http://localhost:8000/api/v1";
            LoginEndpoint = $"{BaseUrl}/auth/login/criador";
            ConteudosEndpoint = $"{BaseUrl}/conteudos";
        }

        public void SetBaseUrl(string baseUrl)
        {
            BaseUrl = baseUrl;
            LoginEndpoint = $"{BaseUrl}/auth/login/criador";
            ConteudosEndpoint = $"{BaseUrl}/conteudos";
        }

        public string GetLoginUrl()
        {
            return LoginEndpoint;
        }

        public string GetConteudosUrl(int? conteudoId = null)
        {
            if (conteudoId.HasValue)
            {
                return $"{ConteudosEndpoint}/{conteudoId}";
            }

            return ConteudosEndpoint;
        }

        public string GetCriadorUrl()
        {
            return $"{BaseUrl}/criadores";
        }
    }
}
