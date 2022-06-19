using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace e_support10.Models
{
    public class Order
    {

        public int Id { get; set; }
        public string CustomerId { get; set; }

  

        [DataType(DataType.DateTime)]
       public DateTime Date { get; set; }

        public double Total { get; set; }

        /**Referencat**/

        public List<OrderRow> OrderRows { get; set; }
        
    }
}
