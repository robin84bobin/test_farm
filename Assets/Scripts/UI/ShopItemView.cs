using UnityEngine;
using UnityEngine.EventSystems;
using ShopItem = Model.ShopItem;

public class ShopItemView : MonoBehaviour 
{

	[SerializeField] private UIWidget _itemPlaceHolder;
	[SerializeField] private GameObject _item;
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

		_item.GetComponent<UIDragDropItem>().enabled = _model.Amount.Value > 0;
	}

	private void InitView()
	{
		_amountLabel.text = _model.Amount.Value.ToString();
		_priceLabel.text = "Price: " + _model.data.BuyPrice;
		
		GameObject source = (GameObject)Resources.Load("UI/FarmItems/"+_model.data.FarmItemId, typeof(GameObject));
		_item = Instantiate(source, _itemPlaceHolder.transform);
		_item.transform.localPosition = Vector3.zero;
		_item.transform.localEulerAngles = Vector3.zero;
		_item.transform.localScale = Vector3.one;
	}

	public void OnBuyClick()
	{
        _model.Buy();
	}

}
