using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class ProductItemView : MonoBehaviour
{
	[SerializeField] private UIWidget _item;
	[SerializeField] private UIButton _sellButton;
	[SerializeField] private UILabel _sellPriceLabel;
	[SerializeField] private UILabel _amount;

	public event Action<ProductItemView> Remove;

	public void Init(Product product)
	{
		
	}
}
