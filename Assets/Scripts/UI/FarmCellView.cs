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
        private GameObject _item;
        private FarmCell _model;

        public void Init(FarmCell cellModel)
        {
            _model = cellModel;
            if (_model.Item != null)
                CreateFarmItem();
        }

        private void CreateFarmItem()
        {
            GameObject source = (GameObject)Resources.Load("UI/FarmItems/"+_model.Item.UserData.CatalogDataId, typeof(GameObject));
            _item = Instantiate(source, _itemPlaceholder);
            _item.transform.localPosition = Vector3.zero;
            _item.transform.localEulerAngles = Vector3.zero;
            _item.transform.localScale = Vector3.one;

            _item.gameObject.GetComponent<FarmItemView>().Init(_model.Item);
            
            _model.Item.OnInitInCell();
        }

        private void Start()
        {
            _dropContainer.OnProductRecieved += OnProductDrop;
            _dropContainer.OnFarmItemRecieved += OnFarmItemDrop;
            _dropContainer.Init(_model);
        }

        private void OnFarmItemDrop(string farmItemId)
        {
            _model.Init(farmItemId);
            CreateFarmItem();
        }

        private void OnProductDrop(Product product)
        {
            if (_model.Item == null) return;
            _model.Item.Eat(product);
        }

    }
}