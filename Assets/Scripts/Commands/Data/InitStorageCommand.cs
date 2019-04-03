using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Commands.Data
{

    public class InitStorageCommand<T> : Command where T : DataItem, new()
    {
        private IDataBaseProxy _dbProxy;
        private DataStorage<T> _storage;

        public InitStorageCommand(DataStorage<T> storage, IDataBaseProxy dbProxy)
        {
            _dbProxy = dbProxy;
            _storage = storage;
        }

        public override void Execute()
        {
            Debug.Log(this + " --> " + _storage.CollectionName);
            _dbProxy.Get<T>(_storage.CollectionName, OnGetData);
        }

        private void OnGetData(Dictionary<string, T> items)
        {
            _storage.SetData(items);
            foreach (var data in _storage)
            {
                data.Init();
            }
            Complete();
        }

        protected override void Release()
        {
            base.Release();
            _storage = null;
        }
    }
}