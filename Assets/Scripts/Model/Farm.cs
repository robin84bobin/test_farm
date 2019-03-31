using System.Collections.Generic;
using Logic.Parameters;

namespace Model
{
    public class Farm 
    {
        public class FarmSize
        {
            public int height;
            public int width;
        }

        public ReactiveParameter<int> Coins;

        public FarmSize size { get; private set; }

        public Dictionary<string,Model.FarmCell> Cells{ get; private set; }
        public Dictionary<string,Model.ShopItem> ShopItems{ get; private set; }
        public Dictionary<string,Model.Product> Products{ get; private set; }

        public void Init()
        {
            size = App.Instance.catalog.GetSetting<FarmSize>("grid");
            
            int coinsValue = App.Instance.catalog.Currency["coins"].value;
            Coins = new ReactiveParameter<int>(coinsValue);
            
            InitCells();
            InitShop();
            InitProducts();
        }

        private void InitProducts()
        {
            Products = new Dictionary<string, Product>();
            foreach (var product in App.Instance.userRepository.Products)
            {
                Products.Add(product.ItemId, new Product(product));
                
            }
        }

        private void InitShop()
        {
            ShopItems = new Dictionary<string, ShopItem>();
        }

        private void InitCells()
        {
            Cells = new Dictionary<string, Model.FarmCell>();
        }
    }

}