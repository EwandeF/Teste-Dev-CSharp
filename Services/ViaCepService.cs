namespace TesteDevCSharp.Services
{
    public class ViaCepService : IViaCepService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ViaCepService> _logger;

        public ViaCepService(HttpClient httpClient, ILogger<ViaCepService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string?> BuscarCepAsync(string cep)
        {
            try
            {
                // Limpeza feita aqui — controller não precisa repetir
                var cepLimpo = new string(cep.Where(char.IsDigit).ToArray());

                if (cepLimpo.Length != 8) return null;

                var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cepLimpo}/json/");
                if (!response.IsSuccessStatusCode) return null;
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar CEP {Cep}", cep);
                return null;
            }
        }
    }
}
