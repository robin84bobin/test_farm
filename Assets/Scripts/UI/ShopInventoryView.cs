using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

public class ShopInventoryView : MonoBehaviour
{
	[SerializeField] private UIGrid _grid;
	[SerializeField] private UILabel _currency;

	private ShopInventory _shopModel;

	public void Init(ShopInventory model)
	{
		_shopModel = model;
		_shopModel.Coins.OnValueChange += OnCoinsChange;
		InitItems(_shopModel.Items);
	}

	private void OnCoinsChange(int oldvalue, int newvalue)
	{
		_currency.text = newvalue.ToString();
	}

	void InitItems(Dictionary<string, Model.ShopItem> modelShopItems)
	{
		foreach (Model.ShopItem shopItem in modelShopItems.Values)
		{
			GameObject source = (GameObject)Resources.Load("UI/ShopItem", typeof(GameObject));
			GameObject go = Instantiate(source, _grid.transform);
			go.transform.localPosition = Vector3.zero;
			go.transform.localEulerAngles = Vector3.zero;
			go.transform.localScale = Vector3.one;
			
			ShopItemView item = go.GetComponent<ShopItemView>();
			item.Init(shopItem);
		}

		_grid.repositionNow = true;
		_grid.Reposition();
	}
	

}
