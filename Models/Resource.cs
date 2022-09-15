using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMSystemITEMCRUD.Models
{
    public class Resource
    {
        [Key]
        public int ResourceId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Image Name")]
        public string ResourceFileName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string ResourceImageTitle { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ResourceImageFile { get; set; }

        // One-To-One with product table
        public Product Product { get; set; }

        // One-To-One with Item Table
        public Item Item { get; set; }
    }
}
