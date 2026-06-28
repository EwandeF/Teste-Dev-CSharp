namespace TesteDevCSharp.Services
{
    public class ViaCepService : IViaCepService
    {
        private readonly HttpClient _httpClient;

        public ViaCepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string?> BuscarCepAsync(string cep)
        {
            try
            {
                // Remove qualquer caractere não numérico (traço, ponto, espaço)
                var cepLimpo = new string(cep.Where(char.IsDigit).ToArray());

                if (cepLimpo.Length != 8) return null;

                var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cepLimpo}/json/");
                if (!response.IsSuccessStatusCode) return null;
                return await response.Content.ReadAsStringAsync();
            }
            catch
            {
                return null;
            }
        }
    }
}