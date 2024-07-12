using Fishki.Maui.Models;
using Fishki.Maui.Vulnerable;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public async Task<HttpResponseMessage> DeleteSet(int setId)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.DeleteAsync($"delete_set?set_id={setId}");
        }

        public async Task<HttpResponseMessage> UpdateSet(StringContent stringContent, int setId)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.PatchAsync($"update_set?set_id={setId}", stringContent);
        }

        public async Task<HttpResponseMessage> AddWords(int setId, StringContent stringContent)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.PostAsync($"add_words?set_id={setId}", stringContent);
        }

        public async Task<HttpResponseMessage> DeleteWords(int setId, int wordsId)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.DeleteAsync($"delete_words?set_id={setId}&words_id={wordsId}");
        }

        public async Task<HttpResponseMessage> UpdateWords(int setId, int wordsId, StringContent stringContent)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.PatchAsync($"update_words?set_id={setId}&words_id={wordsId}", stringContent);
        }
    }
}
