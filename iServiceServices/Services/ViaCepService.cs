using iServiceServices.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace iServiceServices.Services
{
    public class ViaCepService
    {
        private readonly IConfiguration _configuration;

        public ViaCepService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ViaCep> Search(string cep)
        {
            var cepFormat = UtilService.CleanString(cep);

            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://viacep.com.br/ws/{cepFormat}/json");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode == false)
            {
                return new ViaCep();
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var viaCep = JsonSerializer.Deserialize<ViaCep>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return viaCep;
        }
    }
}
