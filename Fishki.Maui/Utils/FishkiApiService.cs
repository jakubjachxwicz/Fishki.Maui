﻿using Fishki.Maui.Models;
using Fishki.Maui.Vulnerable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Fishki.Maui.Utils
{
    internal class FishkiApiService
    {
        private readonly HttpClient _httpClient;

        public FishkiApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(Constants.URI_BASE);
        }

        public async Task<List<FishkiSet>> GetAllSets()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            return await _httpClient.GetFromJsonAsync<List<FishkiSet>>("get_all_sets");
        }
    }
}
