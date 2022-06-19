using System.ComponentModel.DataAnnotations;

namespace e_support10.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        [Range(0, 99999)]
        public double Price { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        [Range(0,999999)]
        public int Quantity { get; set; }
    }
}
