using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CryptoListApi.Entities
{
    public class CryptoDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Pair { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }  
    }
}
