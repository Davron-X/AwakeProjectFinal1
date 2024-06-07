namespace Awake_Models.ViewModel
{
    public class UserProductsVM
    {
        public ApplicationUser User { get; set; }
        public IList<Product> Products { get; set; }

        public UserProductsVM()
        {
            Products = new List<Product>();
        }
    }
}
