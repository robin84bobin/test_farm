﻿using Data.User;

namespace Model
{
    public class FarmCell 
    {
        public FarmItem Item { get; set; }
        public Data.User.UserFarmCell userCellData;
        private Data.User.UserFarmItem _data;

        public FarmCell(UserFarmCell userCellData)
        {
            this.userCellData = userCellData;
            Init();
        }

        public void Init(string farmItemId)
        {
            userCellData.UserFarmItemId = farmItemId;
            Init();
        }
        
        private void Init()
        {
            if (!string.IsNullOrEmpty(userCellData.UserFarmItemId))
            {
                _data = App.Instance.userRepository.FarmItems[this.userCellData.UserFarmItemId];
                Item = new FarmItem(_data);
            }
        }
    }
}