using PIM_Dashboard.Models;

namespace PIM_Dashboard.ViewModels
{
    public class Product_Item_ViewModel
    {
        public ICollection<Product> Products { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
