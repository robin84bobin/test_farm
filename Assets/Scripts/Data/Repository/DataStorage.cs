﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
    public class DataStorage<T> where T : DataItem, new()
    {
        public int Count
        {
            get { return _items.Count; }
        }


        private bool ReadOnly { get; set; }

        /// <summary>
        /// имя таблицы/коллекции в базе данных
        /// </summary>
        public string CollectionName { get; private set; }

        private Dictionary<string, T> _items = new Dictionary<string, T>();
        private IDataBaseProxy _dbProxy;

        public T this[string id]
        {
            get { return Get(id); }
            set { Set(value); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectionName"> имя таблицы/коллекции в базе данных</param>
        /// <param name="readOnly"></param>

        public DataStorage(string collectionName, IDataBaseProxy dbProxy, bool readOnly = false)
        {
            CollectionName = collectionName;
            ReadOnly = readOnly;
            _dbProxy = dbProxy;
        }

        public void Remove(string id, bool now = false, Action<string> callback = null)
        {
            if (_items.ContainsKey(id))
            {
                _items.Remove(id);
                if (now)
                {
                    _dbProxy.SaveCollection(CollectionName, _items);
                }
            }
            else
                Debug.LogError(this + ":: Try to remove unexisted item: Id = " + id);
        }

        #region SET DATA METHODS

        public void Set(T item, string id = "", bool saveNow = false, Action<T> onSuccessCallback = null)
        {
            if (ReadOnly)
            {
                Debug.LogError(string.Format("Can't set data to '{0}' readOnly storage - Id:{1}",
                    typeof(T), id));
                return;
            }


            if (_items.ContainsKey(id))
            {
                _items[id] = item;
                Debug.Log(
                    string.Format("Replace data in '{0}' storage.  Id:{1} already exists", CollectionName, id));
            }
            else
            {
                if (string.IsNullOrEmpty(item.Id))
                    item.Id = id;
                _items.Add(item.Id, item);
                Debug.Log(string.Format("Add data in '{0}' storage. Id:{1}", CollectionName, id));
            }

            if (saveNow)
                SaveData();
        }


        public void SaveData()
        {
            if (ReadOnly)
            {
                Debug.LogError("Can't write into read only storage: " + GetType().Name);
                return;
            }

            _dbProxy.SaveCollection(CollectionName, _items);
        }


        public void SetData(Dictionary<string, T> items)
        {
            _items = items;
        }

        #endregion


        #region GET DATA METHODS


        public T GetRandom(Func<T, bool> condition = null)
        {
            var listToSelect = condition == null ? GetAll() : GetAll().Where(condition).ToList();
            var randomIndex = UnityEngine.Random.Range(0, listToSelect.Count - 1);
            return listToSelect[randomIndex];
        }


        public List<T> GetAll(Func<T, bool> condition)
        {
            return GetAll().Where(condition).ToList();
        }


        public List<T> GetAll()
        {
            return _items.Values.ToList();
        }


        public T Get(string id)
        {
            if (!_items.ContainsKey(id))
            {
                Debug.LogWarning(string.Format("Can't get item from '{0}' storage - Id:{1}", typeof(T), id));
                return default(T);
            }

            return _items[id];
        }


        public T Get(Func<T, bool> predicate)
        {
            return _items.Values.FirstOrDefault(predicate);
        }

        #endregion

        public bool Exists(string id)
        {
            return _items.ContainsKey(id);
        }


        public void Clear()
        {
            _items.Clear();
        }




        #region foreach methods impmementation 

        public BaseStorageEnumerator GetEnumerator()
        {
            return new BaseStorageEnumerator(_items.Values.ToArray());
        }


        public class BaseStorageEnumerator
        {
            int _currentItem;
            int _itemsLength;
            T[] _items;

            public BaseStorageEnumerator(T[] items)
            {
                _currentItem = -1;
                _itemsLength = items.Length;
                _items = items;
            }

            public T Current
            {
                get { return _items[_currentItem]; }
            }

            public bool MoveNext()
            {
                if (_currentItem == (_itemsLength - 1))
                {
                    _currentItem = 0;
                    return false;
                }

                _currentItem++;
                return true;
            }
        }

        #endregion

    }
}