using UI.NGUIExtensions;
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

	private FarmDragDropItem _dragDropItem;
	
	public void Init(Model.ShopItem shopItem)
	{
		_model = shopItem;
		_model.Amount.OnValueChange += OnAmountChange;
		InitView();
		CheckDragAvaLiable();
	}


	private void CheckDragAvaLiable()
	{
		bool draggable = _model.Amount.Value > 0;
		_dragDropItem.interactable = draggable;
		_itemPlaceHolder.alpha = draggable ? 1f : 0.5f;
	}

	private void OnAmountChange(int oldvalue, int newvalue)
	{
		_amountLabel.text = newvalue.ToString();
		CheckDragAvaLiable();
	}

	private void InitView()
	{
		_amountLabel.text = _model.Amount.Value.ToString();
		_priceLabel.text = "Price: " + _model.UserData.CatalogData.BuyPrice;
		
		GameObject source = (GameObject)Resources.Load("UI/FarmItems/"+_model.UserData.CatalogData.FarmItemId, typeof(GameObject));
		_item = Instantiate(source, _itemPlaceHolder.transform);
		_item.transform.localPosition = Vector3.zero;
		_item.transform.localEulerAngles = Vector3.zero;
		_item.transform.localScale = Vector3.one;
		
		_dragDropItem = _item.GetComponent<FarmDragDropItem>();
		_dragDropItem.Data = _model.UserData.CatalogData;
	}

	public void OnBuyClick()
	{
        _model.Buy();
	}

}
