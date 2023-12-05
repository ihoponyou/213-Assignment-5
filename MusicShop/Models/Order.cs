using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicShop.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int CardNumber { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        [ForeignKey("ShippingInfo")]
        public int ShippingInfoId { get; set; }

        public virtual ShippingInfo ShippingInfo { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
