using System.Collections.Generic;
using Data.User;
using Logic.Parameters;

namespace Model
{
    public class ProductInventory
    {
        public Dictionary<string, Product> Items;

        public void Add(Data.Product product, int amount = 1)
        {
            Items[product.Id].ChangeAmount(amount);
        }

        public void Init()
        {
            Items = new Dictionary<string, Product>();
            foreach (var userProduct in App.Instance.UserRepository.Products.GetAll())
            {
                Items.Add(userProduct.Id, new Product(userProduct));
            }
        }
    }
}