using Fishki.Maui.Models;
using Fishki.Maui.Vulnerable;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Fishki.Maui.Utils
{
    public class FishkiApiService
    {
        private readonly HttpClient _httpClient;

        public FishkiApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(Constants.URI_BASE);
            _httpClient.Timeout = new TimeSpan(0, 0, 15);
        }

        public async Task<HttpResponseMessage?> GetSet(int setId)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.GetAsync($"get_set?set_id={setId}");
        }

        public async Task<HttpResponseMessage?> GetWords(int setId)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.GetAsync($"get_words?set_id={setId}");
        }

        public async Task<HttpResponseMessage?> GetAllSets()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.GetAsync("get_all_sets");
        }

        public async Task<HttpResponseMessage?> AddSet(StringContent stringContent)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.PostAsync("create_set", stringContent);
        }

        public async Task<HttpResponseMessage?> DeleteSet(int setId)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.DeleteAsync($"delete_set?set_id={setId}");
        }

        public async Task<HttpResponseMessage?> UpdateSet(StringContent stringContent, int setId)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.PatchAsync($"update_set?set_id={setId}", stringContent);
        }

        public async Task<HttpResponseMessage?> AddWords(int setId, StringContent stringContent)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.PostAsync($"add_words?set_id={setId}", stringContent);
        }

        public async Task<HttpResponseMessage?> DeleteWords(int setId, int wordsId)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.DeleteAsync($"delete_words?set_id={setId}&words_id={wordsId}");
        }

        public async Task<HttpResponseMessage?> UpdateWords(int setId, int wordsId, StringContent stringContent)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.PatchAsync($"update_words?set_id={setId}&words_id={wordsId}", stringContent);
        }

        public async Task<HttpResponseMessage?> RegisterUser(StringContent stringContent)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.PostAsync("register", stringContent);
        }

        public async Task<HttpResponseMessage?> LoginUser(StringContent stringContent)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.PostAsync("login", stringContent);
        }

        public async Task<HttpResponseMessage?> VerifyToken(string token)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetAsync("verify_token");
        }
    }
}
