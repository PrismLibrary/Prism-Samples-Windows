namespace AdventureWorks.Shopper.DesignViewModels
{
    public class ShoppingCartTabDesignViewModel
    {
        public ShoppingCartTabDesignViewModel()
        {
            FillWithDummyData();
        }

        public int ItemCount { get; set; }

        private void FillWithDummyData()
        {
            ItemCount = 5;
        }
    }
}
