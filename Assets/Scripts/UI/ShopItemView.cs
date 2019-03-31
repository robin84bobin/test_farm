using System.Collections;
using System.Collections.Generic;
using Data;
using Data.User;
using TMPro;
using UnityEngine;
using ShopItem = Model.ShopItem;

public class ShopItemView : MonoBehaviour {

	[SerializeField] private UIWidget _itemPlaceHolder;
	[SerializeField] private UIButton _buyButton;
	[SerializeField] private UILabel _priceLabel;
	[SerializeField] private UILabel _amountLabel;
	private ShopItem _model;


	public void Init(Model.ShopItem shopItem)
	{
		_model = shopItem;
		InitView();
	}

	private void InitView()
	{
		GameObject source = (GameObject)Resources.Load("UI/FarmItems/"+_model.data.name, typeof(GameObject));
		GameObject go = Instantiate(source, _itemPlaceHolder.transform);
		go.transform.localPosition = Vector3.zero;
		go.transform.localEulerAngles = Vector3.zero;
		go.transform.localScale = Vector3.one;
	}

	public void SetUserData(Data.User.UserShopItem userShopItem)
	{
		_userShopItem = userShopItem;
		_amountLabel.text = userShopItem.Amount.ToString();
	}
}
