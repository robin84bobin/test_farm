using System.Collections.Generic;

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

        public void Init()
        {
            size = App.Instance.catalog.GetSetting<FarmSize>("grid");
            
            ShopInventory = new ShopInventory();
            ShopInventory.Init();
            
            InitCells();
        }

        private void InitCells()
        {
            Cells = new Dictionary<string, Model.FarmCell>();
        }
    }

}