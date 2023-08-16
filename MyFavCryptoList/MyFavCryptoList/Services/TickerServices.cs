using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CryptoListApi.TicketsService;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace CryptoListApi.Services
{
    public class TickerServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<TickerServices> _logger;
        private ILogger<TickerServices> logger;
        public TickerServices(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<List<Tickers>> GetSelectedTickerAsync()
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                var apiUrl = "https://api.btcturk.com/api/v2/ticker";
                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);
                    List<Tickers> selectedTickers = new List<Tickers>();

                    foreach (var data in apiResponse.Data)
                    {
                        Tickers ticker = new Tickers()
                        {
                            Pair = data.Pair,

                            Last = data.Last,

                        };
                        selectedTickers.Add(ticker);
                    }
                    return selectedTickers;

                }

                else
                {
                    _logger.LogError("API'den veriler alınamıyor! ");
                    return null;
                }
            }
        }

        public class ApiResponse
        {
            public List<ApiData> Data { get; set; }
            public bool Success { get; set; }
            public string Message { get; set; }
            public int Code { get; set; }
        }

        public class ApiData
        {
            public string Pair { get; set; }
            public long Timestamp { get; set; }
            public decimal Last { get; set; }


        }

    }
}
