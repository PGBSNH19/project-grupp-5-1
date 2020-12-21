using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int Stock { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public string ImageUrl // Temp solution until we add this property to the API model
        { 
            get => "https://promoboxx.com/wp-content/uploads/2013/01/promoboxx_icon__white_200x2002.png"; 
        } 
        public int ProductCategoryId { get; set; }
    }
}