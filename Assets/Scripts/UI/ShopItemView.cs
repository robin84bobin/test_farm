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
		
		_model.Amount.OnValueChange += OnAmountChange;
	}

	private void OnAmountChange(int oldvalue, int newvalue)
	{
		_amountLabel.text = newvalue.ToString();
	}

	private void InitView()
	{
		_amountLabel.text = _model.Amount.Value.ToString();
		
		GameObject source = (GameObject)Resources.Load("UI/FarmItems/"+_model.data.FarmItemId, typeof(GameObject));
		GameObject go = Instantiate(source, _itemPlaceHolder.transform);
		go.transform.localPosition = Vector3.zero;
		go.transform.localEulerAngles = Vector3.zero;
		go.transform.localScale = Vector3.one;
	}

	public void OnBuyClick()
	{
		
	}
}
