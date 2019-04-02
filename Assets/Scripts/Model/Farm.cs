using System.Collections.Generic;
using Data.User;

namespace Model
{
    public class Farm 
    {
        public FarmSize size { get; private set; }

        public ShopInventory ShopInventory { get; private set; }
        public ProductInventory ProductInventory { get; private set; }
        
        public Dictionary<string,Model.FarmCell> Cells{ get; private set; }

        public void Init()
        {
            size = App.Instance.catalog.GetSetting<FarmSize>("grid");
            
            ShopInventory = new ShopInventory();
            ShopInventory.Init();
            
            ProductInventory = new ProductInventory();
            ProductInventory.Init();
            
            InitCells();
        }

        
        private void InitCells()
        {
            Cells = new Dictionary<string, Model.FarmCell>();
            foreach (var cell in App.Instance.userRepository.Cells)
            {
                Cells.Add(cell.Id,  new FarmCell(cell));
            }
        }
    }

}