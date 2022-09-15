using Microsoft.EntityFrameworkCore;
using PIMSystemITEMCRUD.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMSystemITEMCRUD.ViewModels
{
    [Index(nameof(ItemName), IsUnique = true)]
    public class ItemViewModel
    {
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Item Name")]
        [Column(TypeName = "nvarchar(50)")]
        public string ItemName { get; set; }
        public string ItemStatus { get; set; }
        public double ItemRetailPrice { get; set; }
        public string ItemPackageType { get; set; }
        public string ItemPackageQuantity { get; set; }
        public string ItemEngineType { get; set; }
        public string ItemServiceInterval { get; set; }
        public string ItemBrandColor { get; set; }
        public string ItemBaseColor { get; set; }
        public string ItemNutritionalFacts { get; set; }
        public string ItemFoodGroup { get; set; }
        public string ItemSize { get; set; }
        public string ItemCategory { get; set; }
        public string ItemForceSend { get; set; }

        [DisplayName("Item Created")]
        public DateTime ItemCreated { get; set; }
        public Product Product { get; set; }
        public ICollection<Resource> Resources { get; set; }
    }
}
