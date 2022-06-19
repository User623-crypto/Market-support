using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_support10.Models
{
    public class OrderRow
    {

        public int Id { get; set; }

        public int OrderId { get; set; }
        public int? ProductId { get; set; }

        public int? ServiceId { get; set; }

        public int Quantity { get; set; }

        public double Cost { get; set; }

        public int? Warranty { get; set; }
        public double Total { get; set; }

        /**Referencat*/

        public Order Order { get; set; }

        public Product? Product { get; set; }

    }
}
