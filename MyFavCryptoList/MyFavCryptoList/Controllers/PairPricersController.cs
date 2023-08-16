using CryptoListApi.Entitiys;
using Microsoft.AspNetCore.Mvc;

namespace CryptoListApi.Controllers
{
    [Route("api/pair-price-history")]
    [ApiController]
    public class PairPricersController : ControllerBase
    {
        private readonly CryptoDbContext _context;
        public PairPricersController(CryptoDbContext context)

        {
            _context = context;
        }

        [HttpGet]
        [Route("/api/pair/prices")]
        public IActionResult GetHourlyPrices()
        {
            var hourlyPrices = _context.cryptoPairs
                .Join(_context.cryptoDetails,
                    CryptoDetail => CryptoDetail.Pair,
                    CryptoPair => CryptoPair.Pair,
                    (CryptoPair, CryptoDetail) => new
                    {
                        CryptoDetail.Id,
                        CryptoPair.Pair,
                        CryptoDetail.Price,
                        CryptoDetail.Date
                    })
                .ToList();

            return Ok(hourlyPrices);
        }
    }
}
