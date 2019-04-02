using Data.User;

namespace Model
{
    public class FarmCell 
    {
        public FarmItem Item { get; private set; }
        public Data.User.UserFarmCell userData;
        private Data.FarmItem _data;

        public FarmCell(UserFarmCell userData)
        {
            this.userData = userData;
            if (string.IsNullOrEmpty(userData.UserFarmItemId))
            {
                _data = App.Instance.catalog.FarmItems[this.userData.UserFarmItemId];
                Init(new FarmItem(_data));
            }
        }

        private void Init(FarmItem farmItem)
        {
            Item = farmItem;
        }
    }
}