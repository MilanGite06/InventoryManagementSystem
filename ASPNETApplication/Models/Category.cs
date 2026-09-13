using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Web.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; } = string.Empty;
    }
}