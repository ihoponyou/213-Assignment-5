namespace MusicShop.Models
{
    public class ShippingInfo
    {
        public int Id { get; set; }

        public int Cost { get; set; }

        public string Location { get; set; }

        public bool IsDownload {  get; set; }
    }
}
