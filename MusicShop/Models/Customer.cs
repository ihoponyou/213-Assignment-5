namespace MusicShop.Models
{
    public class Customer : User
    {
        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Address { get; set; }

        public required string Email { get; set; }

        public ICollection<Product> ShoppingCart { get; set; }
    }
}
