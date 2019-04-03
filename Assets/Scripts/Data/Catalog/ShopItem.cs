namespace Data
{
    public class ShopItem : DataItem, IBuyable
    {
        public string FarmItemId;
        public int BuyPrice { get; set; }
        public string BuyCurrencyId;

    }
}