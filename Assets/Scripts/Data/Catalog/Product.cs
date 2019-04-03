namespace Data
{
    public class Product : DataItem, ISellable
    {
        public int SellPrice { get; set; }
    }

    public interface ISellable
    {
        int SellPrice { get; }
    }
}