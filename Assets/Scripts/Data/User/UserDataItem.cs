namespace Data.User
{
    public class UserDataItem<T>: DataItem where T: DataItem
    {
        public T CatalogData { get; private set; }
        public string CatalogDataId;

        public override void Init()
        {
            base.Init();

            CatalogData = App.Instance.CatalogRepository.Storages[Type].GetObject(CatalogDataId) as T;
        }
    }
}