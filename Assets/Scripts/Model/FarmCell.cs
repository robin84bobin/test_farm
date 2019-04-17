using Data.User;

namespace Model
{
    public class FarmCell : BaseModelItem<UserFarmCell>
    {
        public FarmItem Item { get; set; }

        public FarmCell(UserFarmCell userCellData) : base(userCellData)
        {
        }

        public void Init(string farmItemId)
        {
            CreateFarmItem(farmItemId);
            SaveData();
        }

        private void CreateFarmItem(string farmItemId)
        {
            _userData.UserFarmItemId = _userData.Id;
            var userFarmItemData = new UserFarmItem();
            userFarmItemData.Id = _userData.UserFarmItemId;
            userFarmItemData.CatalogDataId = farmItemId;
            userFarmItemData.Type = App.Instance.userRepository.FarmItems.CollectionName;
            userFarmItemData.Init(); 
            App.Instance.userRepository.FarmItems.Set(userFarmItemData, this._userData.Id);
            Item = new FarmItem(userFarmItemData);
        }


        protected override void InitData()
        {
            if (!string.IsNullOrEmpty(_userData.UserFarmItemId))
            {
                UserFarmItem userFarmItemData = App.Instance.userRepository.FarmItems[_userData.UserFarmItemId];
                Item = new FarmItem(userFarmItemData);
            }
        }

        protected override void SaveData()
        {
            UserRepository.Save();
        }
    }
}