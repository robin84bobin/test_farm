using System;
using Model;
using UnityEngine;
using FarmItem = Model.FarmItem;

namespace UI
{
    public class FarmItemView : MonoBehaviour
    {
        [SerializeField] private UISprite _progressBar;
        [SerializeField] private UILabel _resourceLabel;
        [SerializeField] private UISprite _resourceBar;
        [SerializeField] private UILabel _productPendingCount;
        
        private FarmItem _model;

        public void Init(FarmItem model)
        {
            _model = model;
            _model.PendingCount.OnValueChange += OnPendingAmountChange;
            _model.Progress.OnValueChange += OnProgressChange;
           
            if (_model.UserData.CatalogData.ResourceTime > 0)
                _model.ResourceTime.OnValueChange += UpdateResourceView;

            _model.Fsm.OnStateChanged += OnStateChange;

            InitView();
        }

        void Start()
        {
            InitView();
        }
        
        private void InitView()
        {
            bool enableResource = _model.UserData.CatalogData.ResourceTime <= 0 || _model.ResourceTime.Value <= 0;
            _resourceLabel.text = !enableResource ? _model.ResourceTime.Value.ToString():"EMPTY";
            _resourceBar.gameObject.SetActive(!enableResource);

            _resourceBar.color = _model.IsEnoughResources ? Color.green : Color.red;
            
            RefreshPendingIndicator(_model.PendingCount.Value);
        }

        private void OnStateChange(FarmItemState state)
        {
            InitView();
            _progressBar.gameObject.SetActive(state.Name == State.PRODUCE);
        }

        private void UpdateResourceView(float oldvalue = 0, float newvalue = 0)
        {
            _resourceBar.color = _model.IsEnoughResources ? Color.green : Color.red;
            _resourceLabel.text = _model.ResourceTime.Value.ToString();
            _resourceBar.fillAmount = (float) _model.ResourceTime.Value / (float) _model.ResourceMax;
        }

        private void OnProgressChange(float oldvalue, float newvalue)
        {
            _progressBar.fillAmount = newvalue/_model.UserData.CatalogData.ProduceDuration;
        }

        private void OnPendingAmountChange(int oldValue, int amount)
        {
            RefreshPendingIndicator(amount);
        }

        void OnClick()
        {
            if (_model != null)
            {
                _model.PickUp();
            }
        }

        private void RefreshPendingIndicator(int amount)
        {
            if (_model == null)
            {
                _productPendingCount.gameObject.SetActive(false);
                return;
            }
            
            bool visible = amount > 0;
            _productPendingCount.gameObject.SetActive(visible);
            if (visible)
                _productPendingCount.text = _model.PendingCount.Value.ToString();
        }
    }
}