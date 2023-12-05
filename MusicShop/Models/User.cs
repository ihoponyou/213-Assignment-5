namespace MusicShop.Models
{
    public abstract class User
    {
        public int Id { get; set; }

        public required string UserName { get; set; }

        public required string Password { get; set; }

        public required string UserType { get; set; }
    }
}
