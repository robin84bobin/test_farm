using System.Collections.Generic;
using Model;
using UnityEngine;

namespace UI
{
	public class ProductsInventoryView : MonoBehaviour 
	{
		[SerializeField] private UIGrid _grid;

		private Dictionary<string, ProductItemView> dictionary;

		private ProductInventory _model;

		public void Init(ProductInventory model)
		{
			_model = model;

			foreach (var product in _model.Items.Values)
			{
				Add(product);
			}
		}

		public void Add(Model.Product product)
		{
			ProductItemView productView = CreateProductView(product);
			productView.transform.parent = _grid.transform;
			
			_grid.repositionNow = true;
			_grid.Reposition();
		}

		private ProductItemView CreateProductView(Model.Product product)
		{
			GameObject source = (GameObject)Resources.Load("UI/ProductItem", typeof(GameObject));
			GameObject go = Instantiate(source, _grid.transform);
			go.transform.localPosition = Vector3.zero;
			go.transform.localEulerAngles = Vector3.zero;
			go.transform.localScale = Vector3.one;
			ProductItemView productView = go.GetComponent<ProductItemView>();
			productView.Init(product);
			return productView;
		}
	}
}
