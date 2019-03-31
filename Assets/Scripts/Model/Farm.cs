﻿using System.Collections.Generic;
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

        public FarmSize size { get; private set; }

        public ShopInventory ShopInventory { get; private set; }
        
        public Dictionary<string,Model.FarmCell> Cells{ get; private set; }
        public Dictionary<string,Model.ShopItem> ShopItems{ get; private set; }
        public Dictionary<string,Model.Product> Products{ get; private set; }

        public void Init()
        {
            size = App.Instance.catalog.GetSetting<FarmSize>("grid");
            
            ShopInventory = new ShopInventory();
            ShopInventory.Init();
            
            InitCells();
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


        private void InitCells()
        {
            Cells = new Dictionary<string, Model.FarmCell>();
        }
    }

}