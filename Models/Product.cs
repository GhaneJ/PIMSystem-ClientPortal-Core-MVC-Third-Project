using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMSystemITEMCRUD.Models
{
    [Index(nameof(ProductName), IsUnique = true)]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Product Name")]
        [Column(TypeName = "nvarchar(50)")]
        public string ProductName { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string ProductLifecycleStatus { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string ProductShortDescription { get; set; }
        public string ProductLongDescription { get; set; }
        public string ProductManager { get; set; }

        // One-To-One with Resource Table
        public ICollection<Resource> Resources { get; set; }
        public ICollection<Item> Items { get; set; }

    }
}
