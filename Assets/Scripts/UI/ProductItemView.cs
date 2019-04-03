using System;
using UI.NGUIExtensions;
using UnityEngine;

public class ProductItemView : MonoBehaviour
{
	[SerializeField] private UIWidget _item;
	[SerializeField] private UIButton _sellButton;
	[SerializeField] private UILabel _sellPriceLabel;
	[SerializeField] private UILabel _amount;
	private Model.Product _model;

	private FarmDragDropItem _dragDropItem;
	
	public event Action<ProductItemView> Remove;

	public void Init(Model.Product product)
	{
		_model = product;
		_dragDropItem = _item.GetComponent<FarmDragDropItem>();
		_dragDropItem.Data = _model.data;
		_model.Amount.OnValueChange += OnAmountChange;
		
		InitView();
		CheckDragAvaLiable();
	}

	private void CheckDragAvaLiable()
	{
		bool draggable = _model.Amount.Value > 0;
		_dragDropItem.interactable = draggable;
		_item.alpha = draggable ? 1 : 0.5;
	}

	private void InitView()
	{
		_amount.text = _model.Amount.Value.ToString();
		_sellPriceLabel.text = "Price: " + _model.data.SellPrice;
		
		GameObject source = (GameObject)Resources.Load("UI/Products/"+_model.data.Id, typeof(GameObject));
		GameObject go = Instantiate(source, _item.transform);
		go.transform.localPosition = Vector3.zero;
		go.transform.localEulerAngles = Vector3.zero;
		go.transform.localScale = Vector3.one;
	}

	private void OnAmountChange(int oldvalue, int newvalue)
	{
		_amount.text = newvalue.ToString();
		CheckDragAvaLiable();
	}
	
	public void OnBuyClick()
	{
		_model.Sell();
	}
	
}
