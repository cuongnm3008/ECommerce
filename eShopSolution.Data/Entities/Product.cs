using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        
        public decimal Price { get; set; }

        public decimal OriginalPrice { get; set; }

        public int Stock { get; set; }

        public DateTime? DateCreated { get; set; }

        public string SeoAlias { get; set; }
    }
}
