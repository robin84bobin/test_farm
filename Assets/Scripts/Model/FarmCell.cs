using Data.User;

namespace Model
{
    public class FarmCell : BaseModelItem<UserFarmCell>
    {
        public FarmItem Item { get; set; }

        public FarmCell(UserFarmCell userCellData) : base(userCellData)
        {
            Init();
        }

        public void Init(string farmItemId)
        {
            _userData.UserFarmItemId = farmItemId;
            Init();
            SaveData();
        }
        
        private void Init()
        {
            if (!string.IsNullOrEmpty(_userData.UserFarmItemId))
            {
                var userFarmItemData = App.Instance.userRepository.FarmItems[_userData.UserFarmItemId];
                Item = new FarmItem(userFarmItemData);
            }
        }

        protected override void InitData()
        {
            //throw new System.NotImplementedException();
        }

        protected override void SaveData()
        {
            UserRepository.Save();
        }
    }
}