﻿using Microsoft.AspNetCore.WebUtilities;

namespace Marketplace.Infrastructure
{
    public class PurgomalumClient
    {
        private readonly HttpClient _httpClient;
        public PurgomalumClient() : this (new HttpClient()){ }
        public PurgomalumClient(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<bool> CheckForProfanity(string text)
        {
            var result = await _httpClient.GetAsync(QueryHelpers.AddQueryString("https://www.purgomalum.com/service/containsprofanity", "text", text));
            var value = await result.Content.ReadAsStringAsync();
            return bool.Parse(value);
        }
    }
}
