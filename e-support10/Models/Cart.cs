using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace e_support10.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public string CustomerId { get; set; }

        public int Quantity { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }
        public double Price { get; set; }

        /**Referencat*/

        public Product Product { get; set; }

    }
}
