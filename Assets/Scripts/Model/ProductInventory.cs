using System.Collections.Generic;
using Logic.Parameters;

namespace Model
{
    public class ProductInventory
    {
        public Dictionary<string, Model.Product> Items;

        public void Init()
        {
            Items = new Dictionary<string, Model.Product>();
            foreach (var product in App.Instance.userRepository.Products.GetAll())
            {
                Items.Add(product.ItemId, new Product(product));
            }
        }

    }
}