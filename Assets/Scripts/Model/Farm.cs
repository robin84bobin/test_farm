using System.Collections.Generic;
using Data.User;
using Zenject;

namespace Model
{
    public class Farm 
    {
        private FarmCell.Factory _cellFactory;
        public FarmSize size { get; private set; }

        public ShopInventory ShopInventory { get; private set; }
        public ProductInventory ProductInventory { get; private set; }
        
        public Dictionary<string,Model.FarmCell> Cells{ get; private set; }


        public Farm(ShopInventory shop, ProductInventory products, FarmCell.Factory cellFactory)
        {
            ShopInventory = shop;
            ProductInventory = products;
            _cellFactory = cellFactory;
        }

        public void Init()
        {
            size = App.Instance.catalog.GetSetting<FarmSize>("grid");
            ShopInventory.Init();
            ProductInventory.Init();
            InitCells();
        }
        
        private void InitCells()
        {
            Cells = new Dictionary<string, Model.FarmCell>();
            foreach (var cell in App.Instance.userRepository.Cells)
            {
                var cellModel = _cellFactory.Create(cell);
                Cells.Add(cell.Id, cellModel);
            }
        }

       
    }

}