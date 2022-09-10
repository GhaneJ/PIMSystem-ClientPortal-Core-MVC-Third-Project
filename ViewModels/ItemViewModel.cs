using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMSystemITEMCRUD.ViewModels
{
    public class ItemViewModel
    {
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Item Name")]
        [Column(TypeName = "nvarchar(50)")]
        public string ItemName { get; set; }

        [DisplayName("Item Created")]
        public DateTime ItemCreated { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string ItemImageTitle { get; set; }
        public string ItemPackageType { get; set; }

        [Precision(18, 2)]
        public decimal ItemPrice { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Image Name")]
        public string ItemImageName { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ItemImageFile { get; set; }
    }
}
