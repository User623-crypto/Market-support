using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace e_support10.Models
{
    public class SupplyOrder
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }



        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        public double Total { get; set; }

        /**Referencat**/

        public List<SupplyOrderRow> SupplyOrderRows { get; set; }
    }
}
