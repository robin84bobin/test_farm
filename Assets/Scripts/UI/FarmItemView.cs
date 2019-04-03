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
            _model.ResourceTime.OnValueChange += OnResourceTimeChange;

            _model.Fsm.OnStateChanged += OnStateChange;
            
            RefreshPendingIndicator(_model.PendingCount.Value);
        }

        private void OnStateChange(FarmItemState state)
        {
            switch (_model.Fsm.CurrentState.Name)
            {
                case State.IDLE:
                    _resourceLabel.text = "EMPTY";
                    _progressBar.gameObject.SetActive(false);
                    _resourceBar.gameObject.SetActive(false);
                    break;
                case State.PRODUCE:
                    _resourceLabel.text = "RESOURCES";
                    _progressBar.gameObject.SetActive(true);
                    _resourceBar.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnResourceTimeChange(int oldvalue, int newvalue)
        {
            _resourceBar.fillAmount = newvalue;
        }

        private void OnProgressChange(float oldvalue, float newvalue)
        {
            _progressBar.fillAmount = newvalue;
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
            _productPendingCount.gameObject.SetActive(visible);;
            if (visible)
                _productPendingCount.text = _model.PendingCount.ToString();
        }
    }
}