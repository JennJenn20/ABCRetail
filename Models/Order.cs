using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;
using ABCRetailWebApplication;

namespace ABCRetailWebApplication.Models
{
    public class Order 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Required]
        public int Quantity { get; set; }

        
    }
}
