using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        public required string Title { get; set; }
        
        public required string Genre { get; set; }

        public required string Performer { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public required decimal Price { get; set; }
        
        public required int Quantity { get; set; }
    }
}
