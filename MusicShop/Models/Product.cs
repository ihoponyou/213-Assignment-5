namespace MusicShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        public required string Title { get; set; }
        
        public required string Genre { get; set; }

        public required string Performer { get; set; }
        
        public int Quantity { get; set; }
    }
}
