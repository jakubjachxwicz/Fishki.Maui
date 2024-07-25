using System.Net.Http.Headers;

namespace Fishki.Maui.Utils
{
    public partial class FishkiApiService
    {
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

        public async Task<HttpResponseMessage?> UpdateUsername(StringContent stringContent)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.PatchAsync("update_username", stringContent);
        }

        public async Task<HttpResponseMessage?> UpdateEmail(StringContent stringContent)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.PatchAsync("update_email", stringContent);
        }

        public async Task<HttpResponseMessage?> UpdatePassword(StringContent stringContent)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.PatchAsync("update_password", stringContent);
        }

        public async Task<HttpResponseMessage?> DeleteUser()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.DeleteAsync("delete_user");
        }

        public async Task<HttpResponseMessage?> GetUserData()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.GetAsync("get_user_data");
        }

        public async Task<HttpResponseMessage?> VerifyToken(string token)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetAsync("verify_token");
        }

        public void SetBearerToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
