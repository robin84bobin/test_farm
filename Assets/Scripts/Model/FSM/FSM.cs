using System;
using System.Collections.Generic;
using Model.FSM;

namespace Model
{
    public interface IStateMachine<TKey>
    {
        void SetState(TKey key);
    }

    public class FSM<TKey, TState> : IStateMachine<TKey> where TState : BaseState<TKey>
    {
        public event Action<TState> OnStateChanged;
        public TState CurrentState { get; private set; }
        private Dictionary<TKey, TState> _states = new Dictionary<TKey, TState>();

        public void Add(TState state)
        {
            if (!_states.ContainsKey(state.Name))
            {
                _states.Add(state.Name, state);
            }

            _states[state.Name].SetOwner(this);
        }

        public void Remove(TKey key)
        {
            _states[key].Release();
            _states.Remove(key);
        }

        public void SetState(TKey key)
        {
            if (CurrentState != null)
            {
                CurrentState.OnExitState();
            }

            CurrentState = _states[key];
            CurrentState.OnEnterState();

            if (OnStateChanged != null)
                OnStateChanged.Invoke(CurrentState);
        }

        public void Release()
        {
            var keys = new List<TKey>(_states.Keys);
            foreach (var key in keys)
            {
                Remove(key);
            }
        }
    }

}


