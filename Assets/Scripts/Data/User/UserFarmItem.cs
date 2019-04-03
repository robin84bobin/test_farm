namespace Data.User
{
    public class UserFarmItem: UserDataItem<FarmItem>
    {
        public string ItemId;
        public float Progress;
        public int ResourceTime;
        public int PendingCount;
    }
}