using System;

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
        }
    }
}