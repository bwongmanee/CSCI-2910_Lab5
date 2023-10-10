using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WongmaneeB_ConsumingAPIsII
{
    public class SCPDataAPI
    {
        private readonly HttpClient _httpClient;

        // ======================== FIELDS ======================== //
        public SCPDataAPI()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://scp-data.tedivm.com/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add
                (new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        // ======================== GET DATA ASYNC ======================== //
        public async Task<string> SCPCall(string endpoint)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode(); // Throws an exception on non-success status codes.

                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

    }
}
