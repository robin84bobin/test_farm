using System;
using System.Collections;
using Data.User;
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
        private FarmCell _model;
        
        public void OnDroppedObject(IDroppableData dropItem)
        {
            if (dropItem == null || dropItem.Data == null)
            {
                return;
            }

            if (dropItem.Data.Type == UserRepository.SHOP)
            {
               RecieveShopItem(dropItem);
            }
            else if (dropItem.Data.Type == UserRepository.PRODUCTS)
            {
               RecieveProduct(dropItem);
            }
        }

        void OnRecieved()
        {
            var table = transform.parent.gameObject.GetComponent<UITable>();
            if (table != null)
            {
                StartCoroutine(OnReposition(table));
            }
        }

        void RecieveShopItem(IDroppableData dropItem)
        {
            if (_model.Item == null)
            {
                App.Instance.FarmModel.ShopInventory.Items[dropItem.Data.Id].Spend();
                if (OnFarmItemRecieved != null)
                {
                    ShopItem shopItem = dropItem.Data as ShopItem;
                    OnFarmItemRecieved.Invoke(shopItem.FarmItemId);
                }
            }
            OnRecieved();
        }
        

        void RecieveProduct(IDroppableData dropItem)
        {
            if (_model.Item == null)
               return;  
           
            if (OnProductRecieved != null)
                OnProductRecieved.Invoke(dropItem.Data as Product);

            OnRecieved();
        }

        private IEnumerator OnReposition(UITable table)
        {
            yield return null;
            table.repositionNow = true;
            table.Reposition();
        }


        public void Init(FarmCell model)
        {
            _model = model;
        }
    }
}