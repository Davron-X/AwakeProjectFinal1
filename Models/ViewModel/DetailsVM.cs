namespace Awake_Models.ViewModel
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            Product = new();
        }
        public  Product Product { get; set; }
        public bool ExistInCar { get; set; }
    }
}
