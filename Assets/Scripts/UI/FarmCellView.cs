using Data.User;
using Model;
using UI.NGUIExtensions;
using UnityEngine;
using FarmItem = Data.FarmItem;
using Product = Data.Product;

namespace UI
{
    public class FarmCellView : MonoBehaviour
    {
        [SerializeField] private Transform _itemPlaceholder;
        [SerializeField] private FarmCellDropContainer _dropContainer;
        private FarmCell _model;

        public void Init(FarmCell cellModel)
        {
            _model = cellModel;
        }
        
        private void Start()
        {
            _dropContainer.OnProductRecieved += OnProductDrop;
            _dropContainer.OnFarmItemRecieved += OnFarmItemDrop;
        }

        private void OnFarmItemDrop(FarmItem farmItem)
        {
            
        }

        private void OnProductDrop(Product obj)
        {
            //
        }

    }
}