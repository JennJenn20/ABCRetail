using Azure;
using Azure.Data.Tables;
using Microsoft.Azure.Cosmos;
using System.ComponentModel.DataAnnotations;

namespace ABCRetailWebApplication.Models
{
    public class Customer : ITableEntity
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        // Properties required by ITableEntity
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Default constructor
        public Customer() { }

        // Optional constructor for convenience
        public Customer(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }
    }

   
}
