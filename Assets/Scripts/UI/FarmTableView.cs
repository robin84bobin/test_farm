using UnityEngine;

namespace UI
{
    public class FarmTableView : MonoBehaviour
    {
        [SerializeField] private UITable _table;

        private int _cellAmount;
        
        public void Init(int width, int height)
        {
            _cellAmount = width * height;
            _table.columns = width;

            for (int i = 0; i < _cellAmount; i++)
            {
                GameObject source = (GameObject)Resources.Load("UI/FarmCell", typeof(GameObject));
                GameObject go = Instantiate(source, _table.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localEulerAngles = Vector3.zero;
                go.transform.localScale = Vector3.one;
                FarmCellView cell = go.GetComponent<FarmCellView>();
            }
        }
    }
}