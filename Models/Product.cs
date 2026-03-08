using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleApiCrud.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required, Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative number"),] [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required, Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative integer")]
        public int Stock { get; set; }
    }
}