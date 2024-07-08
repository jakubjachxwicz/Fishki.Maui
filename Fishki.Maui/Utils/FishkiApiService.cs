using Fishki.Maui.Models;
using Fishki.Maui.Vulnerable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Fishki.Maui.Utils
{
    public class FishkiApiService
    {
        private readonly HttpClient _httpClient;

        public FishkiApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(Constants.URI_BASE);
        }

        public async Task<FishkiSet> GetSet(int setId)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.GetFromJsonAsync<FishkiSet>($"get_set?set_id={setId}");
        }

        public async Task<List<Words>> GetWords(int setId)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.GetFromJsonAsync<List<Words>>($"get_words?set_id={setId}");
        }

        public async Task<List<FishkiSet>> GetAllSets()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.GetFromJsonAsync<List<FishkiSet>>("get_all_sets");
        }

        public async Task<HttpResponseMessage> AddSet(StringContent stringContent)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.PostAsync("create_set", stringContent);
        }
    }
}
