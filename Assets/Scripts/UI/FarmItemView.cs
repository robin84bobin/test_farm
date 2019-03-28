using UnityEngine;

namespace UI
{
    public class FarmItemView : MonoBehaviour
    {
        private Model.FarmItem _model;

        public void Init(Model.FarmItem model)
        {
            if (_model != null)
                _model.Release();
            _model = model;

            _model.OnProduceComplete += OnProduce;
        }

        private void OnProduce(string s, int amount)
        {
            throw new System.NotImplementedException();
        }
    }
}