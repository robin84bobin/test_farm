using System.Collections.Generic;

namespace Model
{
    public class FarmCell 
    {
        Data.User.FarmCell _data;

        public void Init(Data.User.FarmCell userData)
        {
            _data = userData;
        }
    }

    public class Farm 
    {
        private List<Model.FarmCell> _cells;

        public void Init()
        {
            _cells = new List<FarmCell>();
            var cellDatas = App.Instance.Repository.Cells;
            for (int i = 0; i < cellDatas.Count; i++)
            {
                FarmCell cell = new FarmCell();
                cell.Init(cellDatas[i]);

                _cells.Add(cell);
            }
        }



    }
}