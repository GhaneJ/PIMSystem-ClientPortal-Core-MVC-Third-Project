using PIM_Dashboard.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIM_Dashboard.ViewModels
{
    public class ProductViewModel
    {
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
        public ICollection<Item> Items { get; set; }

        // Resources

        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Image Name")]
        public string ResourceFileName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string ResourceImageTitle { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ResourceImageFile { get; set; }
    }
}
