using System;

namespace Data.User
{
    public class UserRepository : Repository.Repository
    {
        public  DataStorage<UserShopItem> ShopItems;
        public  DataStorage<UserProduct> Products;
        public  DataStorage<UserFarmCell> Cells;
        
        public UserRepository(IDataBaseProxy dbProxy) : base(dbProxy)
        {
        }

        public override void Init()
        {
            ShopItems = CreateStorage<UserShopItem>("shop");
            Products = CreateStorage<UserProduct>("products");
            Cells = CreateStorage<UserFarmCell>("cells");
        }
    }
}