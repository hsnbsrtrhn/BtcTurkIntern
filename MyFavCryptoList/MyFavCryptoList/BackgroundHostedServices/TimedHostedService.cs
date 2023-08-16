using CryptoListApi.Entitiys;
using CryptoListApi.Services;
using CryptoListApi.TicketsService;
using CryptoListApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoListApi.BackgroundHostedServices
{
    public class TimedHostedService : BackgroundService
    {
        private readonly TickerServices _tickerService;
        private readonly IServiceScopeFactory _scopeFactory;

        public TimedHostedService(TickerServices tickerService, IServiceScopeFactory scopeFactory)
        {
            
            _tickerService = tickerService;
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //await WaitForNextMinuteAsync(); // sonraki saat başlama dakikasına kadar beklemek için kullanılır
                using IServiceScope scope = _scopeFactory.CreateScope(); //*****
                var dbContext = scope.ServiceProvider.GetRequiredService<CryptoDbContext>();
                List<Tickers> tickers = await _tickerService.GetSelectedTickerAsync();
                List<CryptoPair> favoriteCryptos = await dbContext.cryptoPairs.ToListAsync();

                foreach (var ticker in tickers)
                {
                    var existingCrypto = favoriteCryptos.FirstOrDefault(d => d.Pair == ticker.Pair);
                    if (existingCrypto != null)
                    {
                        
                        var existingDetails = await dbContext.cryptoDetails.Where(d =>d.Pair == ticker.Pair)
                            .OrderBy(d => d.Date)
                            .Skip(99)
                            .ToListAsync();

                       if (existingDetails.Any())
                        {
                            dbContext.cryptoDetails.RemoveRange(existingDetails);
                        }

                        var newDetail = new CryptoDetail 
                        {
                            Pair = existingCrypto.Pair,
                            Price = ticker.Last,
                            Date = DateTime.Now,
                        };
                        dbContext.cryptoDetails.Add(newDetail); 
                    }
                }
                await dbContext.SaveChangesAsync();
                Thread.Sleep(10000);
            }
        }

        private async Task WaitForNextMinuteAsync() // sonraki saat bekleme 
        {
            DateTime currentTime = DateTime.Now; // şu an olan saat bilgiisni alıyoruz buradan.
            DateTime nextMinute = currentTime.AddMinutes(60 - currentTime.Minute);
            TimeSpan delay = nextMinute - currentTime; // gelecek saatten şimdiki saati çıkarıp gecikmeyi buluyoruz. 
            await Task.Delay(delay); //şimdi ki zaman ile bir sonra ki saat başı arasındaki süreyi beklenir.
        }
    }
}
