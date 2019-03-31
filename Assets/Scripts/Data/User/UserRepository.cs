
using Data.Catalog;

namespace Data.User
{
    public class UserRepository : Repository.Repository
    {
        public DataStorage<UserCurrency> Currency;
        public  DataStorage<UserShopItem> ShopItems;
        public  DataStorage<UserFarmItem> FarmItems;
        public  DataStorage<UserProduct> Products;
        public  DataStorage<UserFarmCell> Cells;

        
        public UserRepository(IDataBaseProxy dbProxy) : base(dbProxy)
        {
        }

        public override void Init()
        {
            Currency = CreateStorage<UserCurrency>("currency");
            ShopItems = CreateStorage<UserShopItem>("shop");
            Products = CreateStorage<UserProduct>("products");
            FarmItems = CreateStorage<UserFarmItem>("farmItems");
            Cells = CreateStorage<UserFarmCell>("cells");

            _dbProxy.OnInitialized += OnDbInitComplete;
            _dbProxy.Init();
        }

        public void InitStartValuesFrom(CatalogRepository catalog)
        {
            foreach (Currency currency in catalog.Currency.GetAll())
            {
                this.Currency.Set(new UserCurrency()
                {
                    Id = currency.Id,
                    Value = currency.Value
                });
            }

            foreach (ShopItem shopItem in catalog.ShopItems.GetAll())
            {
                this.ShopItems.Set(new UserShopItem(){ItemId = shopItem.Id}, shopItem.Id);
            }

            foreach (Product product in catalog.Products.GetAll())
            {
                this.Products.Set(new UserProduct(){ItemId = product.Id}, product.Id);
            }
        }
    }
}