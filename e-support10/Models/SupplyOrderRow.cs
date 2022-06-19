using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace e_support10.Models
{
    public class SupplyOrderRow
    {
        public int Id { get; set; }

        public int SupplyOrderId { get; set; }
        public int? ProductId { get; set; }


        public int Quantity { get; set; }

        public double Cost { get; set; }

        public int? Warranty { get; set; }
        public double Total { get; set; }

        /**Referencat*/

        public Order Order { get; set; }

        public Product Product { get; set; }
    }
}
