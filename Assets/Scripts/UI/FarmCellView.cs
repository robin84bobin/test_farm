using Data.User;
using Model;
using UI.NGUIExtensions;
using UnityEngine;
using Product = Data.Product;

namespace UI
{
    public class FarmCellView : MonoBehaviour
    {
        [SerializeField] private Transform _itemPlaceholder;
        [SerializeField] private FarmCellDropContainer _dropContainer;
        private FarmCell _model;

        private void Start()
        {
            _dropContainer.OnProductRecieved += OnProductDrop;
        }

        public void Init(FarmCell cellModel)
        {
            _model = cellModel;
        }

        private void OnProductDrop(Product obj)
        {
            throw new System.NotImplementedException();
        }

        public void SetData(UserFarmCell data)
        {
            var go =_itemPlaceholder.transform.GetComponentInChildren<FarmItemView>().gameObject;
            Destroy(go);
        }
    }
}