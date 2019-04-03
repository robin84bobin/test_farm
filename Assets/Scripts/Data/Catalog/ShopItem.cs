namespace Data
{
    public class ShopItem : DataItem, IBuyable
    {
        public string FarmItemId;
        public string Currency { get; set; }
        public int BuyPrice { get; set; }
        public string BuyCurrencyId;
    }
}