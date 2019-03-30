using System.Collections.Generic;

namespace Model
{
    public class FarmCell 
    {
        Data.User.UserFarmCell _data;

        public void Init(Data.User.UserFarmCell userData)
        {
            _data = userData;
        }
    }

    public class Farm 
    {
        private Dictionary<int,Model.FarmCell> _cells;

        public void Init()
        {
            _cells = new Dictionary<int, Model.FarmCell>();
            var cellDatas = App.Instance.userRepository.Cells;
            
        }



    }
}