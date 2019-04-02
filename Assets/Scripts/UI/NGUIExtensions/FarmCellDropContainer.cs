using System;
using System.Collections;
using Data;
using UnityEngine;

namespace UI.NGUIExtensions
{
    public class FarmCellDropContainer : UIDragDropContainer
    {
        public event Action<Product> OnProductRecieved;
        public event Action<FarmItem> OnFarmItemRecieved;
        private FarmItem _farmItem;
        
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
                if (_farmItem == null)
                {
                    ShopItem shopItem = dropItem.Data as ShopItem;
                    _farmItem = App.Instance.catalog.FarmItems[shopItem.FarmItemId];
                    
                    App.Instance.FarmModel.ShopInventory.Items[shopItem.Id].Spend();
                    Destroy(dropItem);

                    var table = transform.parent.gameObject.GetComponent<UITable>();
                    if (table != null)
                    {
                        StartCoroutine(OnReposition(table));
                    }

                    if (OnFarmItemRecieved != null) 
                        OnFarmItemRecieved.Invoke(_farmItem);
                    return;
                }
            }
            else
            if (dropItem.Data.Type == "product")
            {
                if (OnProductRecieved != null) 
                    OnProductRecieved.Invoke(dropItem.Data as Product);
            }
            
            Destroy(droppedObject);
        }

        private IEnumerator OnReposition(UITable table)
        {
            yield return null;
            table.repositionNow = true;
            table.Reposition();
        }
    }
}