using Data.User;
using Zenject;

namespace Model
{
    public class FarmCell : BaseModelItem<UserFarmCell>
    {
        private FarmItem.Factory _farmItemFactory;

        public class Factory : PlaceholderFactory<UserFarmCell,FarmCell>
        {
            
        }
        
        public FarmItem Item { get; set; }

        public FarmCell(UserFarmCell userCellData, FarmItem.Factory farmItemFactory) 
           //base(userCellData)
        {
            _farmItemFactory = farmItemFactory;
            _userData = userCellData;
            InitData();
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
            Item = _farmItemFactory.Create(userFarmItemData);
        }


        protected override void InitData()
        {
            if (!string.IsNullOrEmpty(_userData.UserFarmItemId))
            {
                UserFarmItem userFarmItemData = App.Instance.userRepository.FarmItems[_userData.UserFarmItemId];
                Item = _farmItemFactory.Create(userFarmItemData);
            }
        }

        protected override void SaveData()
        {
            UserRepository.Save();
        }

        public void Init(UserFarmCell userData)
        {
            _userData = userData;
            InitData();
        }
    }
}