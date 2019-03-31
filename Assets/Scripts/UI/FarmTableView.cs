using System.Collections.Generic;
using Data.User;
using UnityEngine;

namespace UI
{
    public class FarmTableView : MonoBehaviour
    {
        [SerializeField] private UITable _table;

        private int _cellAmount;
        private List<FarmCellView> _cells;
        
        public void Init(int width, int height)
        {
            _cellAmount = width * height;
            _table.columns = width;

            _cells = new List<FarmCellView>();
            for (int i = 0; i < _cellAmount; i++)
            {
                GameObject source = (GameObject)Resources.Load("UI/FarmCell", typeof(GameObject));
                GameObject go = Instantiate(source, _table.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localEulerAngles = Vector3.zero;
                go.transform.localScale = Vector3.one;
                FarmCellView cell = go.GetComponent<FarmCellView>();
                _cells.Add(cell);
            }
            
            _table.repositionNow = true;
            _table.Reposition();
        }


        public void SetData(List<UserFarmCell> userRepositoryCells)
        {
            //
            foreach (var cell in userRepositoryCells)
            {
                _cells[cell.Index].SetData(cell);
            }
        }
    }
}