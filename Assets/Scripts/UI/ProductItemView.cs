using System;
using UnityEngine;
using Product = Model.Product;

public class ProductItemView : MonoBehaviour
{
	[SerializeField] private UIWidget _item;
	[SerializeField] private UIButton _sellButton;
	[SerializeField] private UILabel _sellPriceLabel;
	[SerializeField] private UILabel _amount;
	private Product _product;

	public event Action<ProductItemView> Remove;

	public void Init(Model.Product product)
	{
		_product = product;
		
		_amount.text = _product.Amount.Value.ToString();

		_product.Amount.OnValueChange += OnAmountChange;
		
		GameObject source = (GameObject)Resources.Load("UI/Products/"+_product.data.Id, typeof(GameObject));
		GameObject go = Instantiate(source, _item.transform);
		go.transform.localPosition = Vector3.zero;
		go.transform.localEulerAngles = Vector3.zero;
		go.transform.localScale = Vector3.one;
	}

	private void OnAmountChange(int oldvalue, int newvalue)
	{
		_amount.text = newvalue.ToString();
	}
}
