using UnityEngine;

namespace UI
{
	public class FarmView : MonoBehaviour
	{
		[SerializeField] private FarmTableView _table;
		[SerializeField] private ShopInventoryView _shop;
		[SerializeField] private ProductsInventoryView _products;
	
		private Model.Farm _model;
	
		void Start ()
		{
			_model = App.Instance.FarmModel;
			Init();
		}

		private void Init()
		{
			_table.InitView(_model.size.width, _model.size.height);
			_table.SetData(App.Instance.userRepository.Cells.GetAll());
		}

	}
}
