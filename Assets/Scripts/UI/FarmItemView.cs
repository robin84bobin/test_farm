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
        [SerializeField] private UISprite _productPendingSprite;
        
        private FarmItem _model;

        public void Init(FarmItem model)
        {
            _model = model;
            _model.OnProduceComplete += OnProduceComplete;
            _model.Progress.OnValueChange += OnProgressChange;
            _model.ResourceTime.OnValueChange += OnResourceTimeChange;

            _model.Fsm.OnStateChanged += OnStateChange;
        }

        private void OnStateChange(FarmItemState state)
        {
            switch (_model.Fsm.CurrentState.Name)
            {
                case State.IDLE:
                    _resourceLabel.text = "EMPTY";
                    _progressBar.gameObject.SetActive(false);
                    break;
                case State.PRODUCE:
                    _resourceLabel.text = "RESOURCES";
                    _progressBar.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnResourceTimeChange(float oldvalue, float newvalue)
        {
            _resourceBar.fillAmount = newvalue;
        }

        private void OnProgressChange(float oldvalue, float newvalue)
        {
            _progressBar.fillAmount = newvalue;
        }

        private void OnProduceComplete(string s, int amount)
        {
            throw new System.NotImplementedException();
        }

        void OnClick()
        {
            Data.Product product;
            if (_model.PendingCount > 0)
            {
                if (_model.PickUp(out product))
                {
                    
                }
            }
        }
    }
}