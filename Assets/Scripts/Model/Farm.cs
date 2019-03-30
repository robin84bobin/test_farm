using System.Collections.Generic;
using UnityEngine;

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
        public class FarmSize
        {
            public int height;
            public int width;
        }

        public FarmSize size { get; private set; }

        private Dictionary<int,Model.FarmCell> _cells;

        public void Init()
        {
            size = App.Instance.catalog.GetSetting<FarmSize>("grid");
            _cells = new Dictionary<int, Model.FarmCell>();
            var cellDatas = App.Instance.userRepository.Cells;
            
        }



    }

}