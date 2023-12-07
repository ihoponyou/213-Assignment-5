using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MusicShop.Models
{
    public class ProductPerformerViewModel
    {
        public List<Product> Products { get; set; }
        public SelectList? Performer { get; set; }
        public string? ProductPerformer { get; set; }
        public string? SearchString { get; set; }
    }
}
