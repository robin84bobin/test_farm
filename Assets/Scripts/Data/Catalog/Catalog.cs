using Data.Repository;

namespace Data.Catalog
{
    public class Catalog : Repository.Repository
    {
        public  DataStorage<ShopItem> ShopItems;
        public  DataStorage<Product> Products;
        
        public Catalog(IDataBaseProxy dbProxy) : base(dbProxy)
        {
        }
        
        public override void Init()
        {
            ShopItems = CreateStorage<ShopItem>(Collections.SHOP);
            Products = CreateStorage<Product>(Collections.PRODUCTS);

            _dbProxy.OnInitialized += OnDbInitComplete;
            _dbProxy.Init();
        }
    }
    
    

    public class Collections
    {
        public const string SHOP = "shop";
        public const string PRODUCTS = "products";
    }
}