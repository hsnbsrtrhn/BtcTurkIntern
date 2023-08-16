using Microsoft.AspNetCore.Mvc;
using CryptoListApi.Entitiys;
using CryptoListApi.Models.Requests;

namespace CryptoListApi.Controllers
{
    [Route("api/pair")]
    [ApiController]
    public class PairController : ControllerBase
    {
        private readonly CryptoDbContext _context;

        public PairController(CryptoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            List<CryptoPair> cryptoPairs = _context.cryptoPairs.ToList();
            return Ok(cryptoPairs);
        }

        [HttpPost]
        [Route("")]
        public IActionResult CreateFavorite([FromBody] CreateCryptoRequest request)
        {
            //bool isDuplicate = _context.cryptoEntities.Any(x => x.Pair == request.Name); bunu gidip if içinde kullanabilirsin 

            if (_context.cryptoPairs.Any(x => x.Pair == request.Name))
            {
                return BadRequest("Bu kayıt daha önceden eklenmiş!");
            }

            if (_context.cryptoPairs.Count() >= 10)
            {
                return BadRequest("Maksimum kayıt sayısına ulaşıldı!");

            }


            CryptoPair newCrypto = new CryptoPair
            {
                Pair = request.Name
            };

            _context.cryptoPairs.Add(newCrypto);
            _context.SaveChanges();
            return Ok("Kayıtlar başarıyla eklendi.");
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateByPair(int id, [FromBody] CryptoPair updatedCrypto)
        {
            var existingCrypto = _context.cryptoPairs.FirstOrDefault(x => x.Id == id);
            if (existingCrypto == null)
            {
                return NotFound("Böyle bir kayıt bulunamadı!");
            }

            existingCrypto.Pair = updatedCrypto.Pair;

            _context.SaveChanges();

            return Ok("Kayıt güncellendi.");

        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteByPair(int id)
        {
            var existingCrypto = _context.cryptoPairs.FirstOrDefault(c => c.Id == id);
            if (existingCrypto == null)
            {
                return NotFound("Böyle bir kayıt bulunamadı!");
            }

            _context.cryptoPairs.Remove(existingCrypto);
            _context.SaveChanges();

            return Ok("Kayıt başarılı bir şekilde silindi.");
        }
    }
}
