
using Data.Catalog;
using Model;

namespace Data.User
{
    public class UserRepository : Repository.Repository
    {
        public  DataStorage<UserCurrency> Currency;
        public  DataStorage<UserShopItem> ShopItems;
        public  DataStorage<UserFarmItem> FarmItems;
        public  DataStorage<UserProduct> Products;
        public  DataStorage<UserFarmCell> Cells;

        
        public UserRepository(IDataBaseProxy dbProxy) : base(dbProxy)
        {
            EnableTick();
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
                UserCurrency c = new UserCurrency(){Type = currency.Type,CatalogDataId = currency.Id, Value = currency.Value};
                c.Init();
                this.Currency.Set(c, currency.Id, true);
            }

            foreach (ShopItem shopItem in catalog.ShopItems.GetAll())
            {
                UserShopItem s = new UserShopItem(){Type = shopItem.Type,CatalogDataId = shopItem.Id};
                s.Init();
                this.ShopItems.Set(s, shopItem.Id, true);
            }

            foreach (Product product in catalog.Products.GetAll())
            {
                UserProduct p = new UserProduct(){Type = product.Type,CatalogDataId = product.Id};
                p.Init();
                this.Products.Set(p, product.Id, true);
            }


            var size = catalog.GetSetting<FarmSize>("grid");
            for (int index = 0; index < (size.height*size.width); index++)
            {
                var cell = new UserFarmCell() {Index = index, Id = index.ToString()};
                this.Cells.Set(cell);
            }
            
            SaveAll();
        }

        private static bool _needSave;
        public static void Save()
        {
            _needSave = true;
        }
        protected override void OnTick(int deltaTime)
        {
            base.OnTick(deltaTime);
            if (_needSave)
            {
                _needSave = false;
                SaveAll();
            }
        }
        
        protected void SaveAll()
        {
            Currency.SaveData();
            ShopItems.SaveData();
            FarmItems.SaveData();
            Products.SaveData();
            Cells.SaveData();
        }
    }
}