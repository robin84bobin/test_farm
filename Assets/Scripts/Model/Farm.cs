using System.Collections.Generic;
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

        public ReactiveParameter<int> Coins;

        public FarmSize size { get; private set; }

        private Dictionary<int,Model.FarmCell> _cells;
        private Dictionary<int,Model.ShopItem> _shopItems;

        public void Init()
        {
            size = App.Instance.catalog.GetSetting<FarmSize>("grid");
            int coinsValue = App.Instance.catalog.Currency["coins"].value;
            Coins = new ReactiveParameter<int>(coinsValue);
            _cells = new Dictionary<int, Model.FarmCell>();
        }



    }

}