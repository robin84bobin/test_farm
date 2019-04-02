using System.Collections.Generic;
using Logic.Parameters;

namespace Model
{
    public class ProductInventory
    {
        public Dictionary<string, Product> Items;

        public void Init()
        {
            Items = new Dictionary<string, Product>();
            foreach (var product in App.Instance.userRepository.Products.GetAll())
            {
                Items.Add(product.ItemId, new Product(product));
            }
        }

    }
}