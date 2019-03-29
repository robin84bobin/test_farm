using InternalNewtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace Data.User
{
    public class GameState
    {
        const string PREF_KEY = "GameState";

        public int Money;

        public List<FarmCell> Cells = new List<FarmCell>();
        public Dictionary<string, Data.User.ShopInventoryItem> ShopItems = new Dictionary<string, ShopInventoryItem>();
        public Dictionary<string, Data.User.ProductInventoryItem> Products = new Dictionary<string, ProductInventoryItem>();


        public void Save()
        {
            PlayerPrefs.SetString(PREF_KEY, JsonConvert.SerializeObject(this));
        }

        public void Load()
        {
            GameState gs = JsonConvert.DeserializeObject<GameState>(PlayerPrefs.GetString(PREF_KEY));
            Money = gs.Money;
            Cells = gs.Cells;
            ShopItems = gs.ShopItems;
            Products = gs.Products;
        }
    }
}