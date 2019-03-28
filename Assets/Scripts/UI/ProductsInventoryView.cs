using System.Collections.Generic;
using Data;
using UnityEngine;

namespace UI
{
	public class ProductsInventoryView : MonoBehaviour {

	
		[SerializeField] private UIGrid _grid;

		private Dictionary<string, ProductItemView> dictionary;

		public void Add(Product product)
		{
			ProductItemView productView = CreateProductView(product);
			productView.transform.parent = _grid.transform;
		}

		private ProductItemView CreateProductView(Product product)
		{
			GameObject source = (GameObject)Resources.Load("UI/ProductItemView", typeof(GameObject));
			GameObject go = Instantiate(source);
			go.transform.localPosition = Vector3.zero;
			go.transform.localEulerAngles = Vector3.zero;
			go.transform.localScale = Vector3.one;
			ProductItemView productView = go.GetComponent<ProductItemView>();
			//nameIndicator.Init(info);
			return productView;
		}
	}
}
