using System.Collections;
using System.Collections.Generic;
using Data;
using Data.User;
using UnityEngine;

public class ShopInventoryView : MonoBehaviour
{
	[SerializeField] private UIGrid _grid;
	[SerializeField] private UILabel _currency;
	
	public void Init()
	{
		foreach (UserShopItem shopItem in App.Instance.userRepository.ShopItems.GetAll())
		{
			GameObject source = (GameObject)Resources.Load("UI/ShopItem", typeof(GameObject));
			GameObject go = Instantiate(source, _grid.transform);
			go.transform.localPosition = Vector3.zero;
			go.transform.localEulerAngles = Vector3.zero;
			go.transform.localScale = Vector3.one;
			
			ShopItemView item = go.GetComponent<ShopItemView>();
			item.Init(App.Instance.catalog.ShopItems[shopItem.ItemId]);
			item.SetUserData(shopItem);
		}
	}
	

}
