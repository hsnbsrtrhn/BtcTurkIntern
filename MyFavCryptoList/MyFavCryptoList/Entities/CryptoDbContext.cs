using Microsoft.EntityFrameworkCore;
using CryptoListApi.Entities;

namespace CryptoListApi.Entitiys
{
    public class CryptoDbContext : DbContext 
    {
           public CryptoDbContext(DbContextOptions<CryptoDbContext>options)
            : base(options) 
            {
            }
            public DbSet<CryptoPair> cryptoPairs { get; set; } 
            public DbSet<CryptoDetail> cryptoDetails { get; set; }    //isimlendirmeleri tekrar düşün !!!!

    }
}
