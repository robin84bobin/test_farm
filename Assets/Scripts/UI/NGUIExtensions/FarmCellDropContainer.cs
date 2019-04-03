using System;
using System.Collections;
using Model;
using UnityEngine;
using FarmItem = Data.FarmItem;
using Product = Data.Product;
using ShopItem = Data.ShopItem;

namespace UI.NGUIExtensions
{
    public class FarmCellDropContainer : UIDragDropContainer
    {
        public event Action<Product> OnProductRecieved;
        public event Action<string> OnFarmItemRecieved;
        //private FarmItem _farmItem;
        
        public override void OnDroppedObject(GameObject droppedObject)
        {
            base.OnDroppedObject(droppedObject);
            var dropItem = droppedObject.GetComponent<FarmDragDropItem>();

            if (dropItem == null || dropItem.Data == null)
            {
                Destroy(droppedObject);
                return;
            }

            if (dropItem.Data.Type == "shop")
            {
               RecieveShopItem(dropItem);
            }
            else if (dropItem.Data.Type == "product")
            {
               RecieveProduct(dropItem);
            }
        }

        void OnRecieved(GameObject droppedObject)
        {
            var table = transform.parent.gameObject.GetComponent<UITable>();
            if (table != null)
            {
                StartCoroutine(OnReposition(table));
            }
            
            Destroy(droppedObject);
        }

        void RecieveShopItem(FarmDragDropItem dropItem)
        {
            if (_model.Item == null)
            {
                App.Instance.FarmModel.ShopInventory.Items[dropItem.Data.Id].Spend();
                Destroy(dropItem);

                if (OnFarmItemRecieved != null)
                {
                    ShopItem shopItem = dropItem.Data as ShopItem;
                    OnFarmItemRecieved.Invoke(shopItem.FarmItemId);
                }
            }
            
            OnRecieved(dropItem.gameObject);
        }
        

        void RecieveProduct(FarmDragDropItem dropItem)
        {
            if (_model.Item == null)
               return;  

            if (OnProductRecieved != null)
                OnProductRecieved.Invoke(dropItem.Data as Product);

            OnRecieved(dropItem.gameObject);
        }

        private IEnumerator OnReposition(UITable table)
        {
            yield return null;
            table.repositionNow = true;
            table.Reposition();
        }

        private FarmCell _model;
        public void Init(FarmCell model)
        {
            _model = model;
        }
    }
}