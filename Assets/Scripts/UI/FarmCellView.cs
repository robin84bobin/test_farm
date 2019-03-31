using Data.User;
using UnityEngine;

namespace UI
{
    public class FarmCellView : MonoBehaviour
    {
        [SerializeField] private Transform _itemPlaceholder;
        
        public void SetData(UserFarmCell data)
        {
            var go =_itemPlaceholder.transform.GetComponentInChildren<FarmItemView>().gameObject;
            Destroy(go);
        }
    }
}